using HoboKing.Control;
using HoboKing.Entities.Factory;
using HoboKing.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace HoboKing.Entities
{
    class Map
    {
        public const int HOBO_START_POSITION_X = 2;
        public const int HOBO_START_POSITION_Y = 8;

        public const int TILE_SIZE = 20;
        public const int MAP_WIDTH = 64;
        public const int MAP_HEIGHT = 54;

        private EntityManager entityManager;
        private ContentLoader contentLoader;
        private GraphicsDevice graphics;
        private CritterBuilder critterBuilder;

        public int VisibleTilesX { get; set; }
        public int VisibleTilesY { get; set; } 
        public string Level { get; set; }

        readonly Dictionary<char, Tile> tiles = new Dictionary<char, Tile>();

        public Map(GraphicsDevice g, int windowWidth, int windowHeight)
        {
            graphics = g;
            entityManager = new EntityManager();
            critterBuilder = new CritterBuilder();
            VisibleTilesX = windowWidth / TILE_SIZE;
            VisibleTilesY = windowHeight / TILE_SIZE;
        }
 
        // Reads map data from file
        public void ReadMapData()
        {
            using (var stream = TitleContainer.OpenStream("map/map1.txt"))
            {
                using (var reader = new StreamReader(stream))
                {
                    while (!reader.EndOfStream)
                    {
                        Level += reader.ReadLine();
                    }

                }
            }
        }

        public void Print()
        {
            Console.WriteLine("Map Width: " + MAP_WIDTH);
            Console.WriteLine("Map Height: " + MAP_HEIGHT);
            Console.WriteLine("Tile Width: " + TILE_SIZE);
            Console.WriteLine("Tile Height: " + TILE_SIZE);
            Console.WriteLine("VisibleTilesX: " + VisibleTilesX);
            Console.WriteLine("VisibleTilesY: " + VisibleTilesY);
        }

        // Creates main player for multiplayer
        // Edit later for singleplayer as well
        public Player CreateMainPlayer(Connector connector)
        {
            Player player = new Player(graphics, contentLoader.BatChest, new Vector2(HOBO_START_POSITION_X, HOBO_START_POSITION_Y), 
                connector.GetConnectionId(), false);
            entityManager.AddEntity(player);
            player.SetMovementStrategy(new DebugMovement(player));
            return player;
        }

        public Critter CreateDebugCritter()
        {
            Critter critter = critterBuilder.AddTexture(graphics, contentLoader.Woodcutter, new Vector2(HOBO_START_POSITION_X+8, HOBO_START_POSITION_Y+36), 100)
                .AddMovement(new DebugMovement(null)).AddSpeech("Hello baj, I seek shelter.", 20).Build();
            
            entityManager.AddEntity(critter);
            return critter;
        }

        // Add Player objects for all other connected players
        public void AddConnectedPlayers(Connector connector)
        {
            foreach (var id in connector.ConnectionsIds)
            {
                if (entityManager.players.Find(p => p.ConnectionId == id) == null)
                {
                    Player p = new Player(graphics, contentLoader.BatChest, new Vector2(HOBO_START_POSITION_X, HOBO_START_POSITION_Y), id, true);

                    Console.WriteLine($"Added a new player with ID {id}");
                    entityManager.AddEntity(p);
                }
            }
        }

        // Update player positions by cycling through input list
        public void UpdateConnectedPlayers(Connector connector)
        {
            foreach (Coordinate coordinate in connector.UnprocessedInputs)
            {
                // Handle first input and remove it
                Player p = entityManager.players.Find(p => p.ConnectionId == coordinate.ConnectionID);
                if (p != null)
                {
                    p.Sprite.Position = new Vector2(coordinate.X, coordinate.Y);
                    connector.UnprocessedInputs.Remove(coordinate);
                    break;
                }
                // Remove first input with no users (if user left)
                else
                {
                    connector.UnprocessedInputs.Remove(coordinate);
                    break;
                }
            }
        }

        // Remove Player objects that don't have an owner and are not the main player
        public void RemoveConnectedPlayers(Connector connector)
        {
            foreach (var player in entityManager.players)
            {
                if (player.IsOtherPlayer)
                {
                    if (!connector.ConnectionsIds.Contains(player.ConnectionId))
                    {
                        entityManager.RemoveEntity(player);
                        break;
                    }
                }
            }
        }



        public void LoadEntityContent(ContentManager contentManager)
        {
            contentLoader = new ContentLoader(contentManager);
            CreateTileTypes();
        }

        // This is where you can set tile types for now
        private void CreateTileTypes()
        {
            NormalTileFactory normalTileFactory = new NormalTileFactory();
            tiles.Add('#', normalTileFactory.CreateStandard(graphics, contentLoader.TileTexture, new Vector2(0, 0), TILE_SIZE));
            tiles.Add('<', normalTileFactory.CreateSlopeLeft(graphics, contentLoader.TileTexture, new Vector2(0, 0), TILE_SIZE));
            tiles.Add('>', normalTileFactory.CreateSlopeRight(graphics, contentLoader.TileTexture, new Vector2(0, 0), TILE_SIZE));
        }

        public void DrawEntities(SpriteBatch spriteBatch) 
        {
            entityManager.Draw(spriteBatch);
        }

        public void ShowBoundingBoxes(bool show)
        {
            entityManager.SetShowBoundingBox(show);
        }

        public void Update(GameTime gameTime)
        {
            entityManager.Update(gameTime);
        }

        public void CreateMap()
        {
            NormalTileFactory normalTileFactory = new NormalTileFactory();

            for (int x = 0; x < VisibleTilesX; x++)
            {
                for (int y = 0; y < VisibleTilesY; y++)
                {
                    char TileID = GetTile(x, y);
                    switch (TileID)
                    {
                        case '.':
                            // Empty blocks (background tiles)
                            break;
                        case '#':
                            //AddTile(x, y, TileID, normalTileFactory);
                            if (tiles.ContainsKey(TileID))
                            {
                                Tile tile = tiles[TileID];
                                Tile newTile = normalTileFactory.CreateStandard(graphics, tile.Sprite.Texture, new Vector2(x * TILE_SIZE, y * TILE_SIZE), tile.TileSize);
                                entityManager.AddEntity(newTile);
                            }
                            else
                            {
                                throw new Exception($"Tile type ({TileID}) doesn't exist");
                            }
                            break;
                        case '<':
                            //AddTile(x, y, TileID, normalTileFactory);
                            if (tiles.ContainsKey(TileID))
                            {
                                Tile tile = tiles[TileID];
                                Tile newTile = normalTileFactory.CreateSlopeLeft(graphics, tile.Sprite.Texture, new Vector2(x * TILE_SIZE, y * TILE_SIZE), tile.TileSize);
                                entityManager.AddEntity(newTile);
                            }
                            else
                            {
                                throw new Exception($"Tile type ({TileID}) doesn't exist");
                            }
                            break;
                        case '>':
                            //AddTile(x, y, TileID, normalTileFactory);
                            if (tiles.ContainsKey(TileID))
                            {
                                Tile tile = tiles[TileID];
                                Tile newTile = normalTileFactory.CreateSlopeRight(graphics, tile.Sprite.Texture, new Vector2(x * TILE_SIZE, y * TILE_SIZE), tile.TileSize);
                                entityManager.AddEntity(newTile);
                            }
                            else
                            {
                                throw new Exception($"Tile type ({TileID}) doesn't exist");
                            }
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        public char GetTile(int x, int y)
        {
            if (x >= 0 && x < MAP_WIDTH && y >= 0 && y < MAP_HEIGHT)
            {
                return Level[y * MAP_WIDTH + x];
            }
            else return ' ';
        }

        public string SetTile(int x, int y, char newTile)
        {
            StringBuilder sb = new StringBuilder(Level);
            if (x >= 0 && x < MAP_WIDTH && y >= 0 && y < MAP_HEIGHT)
            {
                sb[y * MAP_WIDTH + x] = newTile;
                return sb.ToString();
            }
            else
            {
                throw new ArgumentOutOfRangeException();
            }
        }

        //private void AddTile(int x, int y, char TileID, AbstractTileFactory tileFactory)
        //{
        //    // Draw platform blocks here
        //    if (tiles.ContainsKey(TileID))
        //    {
        //        Tile tile = tiles[TileID];
        //        Tile newTile = new Tile(graphics, tile.Sprite.Texture, new Vector2(x * TILE_SIZE, y * TILE_SIZE), tile.TileSize);
        //        /*Tile newTile = tileFactory*/
        //        entityManager.AddEntity(newTile);
        //    }
        //    else
        //    {
        //        throw new Exception($"Tile type ({TileID}) doesn't exist");
        //    }
        //}

    
    }
}
