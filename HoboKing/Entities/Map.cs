using HoboKing.Control;
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

        public int VisibleTilesX { get; set; }
        public int VisibleTilesY { get; set; } 
        public string Level { get; set; }

        readonly Dictionary<char, Tile> tiles = new Dictionary<char, Tile>();

        public Map(GraphicsDevice g, int windowWidth, int windowHeight)
        {
            graphics = g;
            entityManager = new EntityManager();
            VisibleTilesX = windowWidth / TILE_SIZE;
            VisibleTilesY = windowHeight / TILE_SIZE;

            // Read map data from file before drawing
            ReadMapData();
        }

        // This is where you can set tile types for now
        private void CreateTileTypes()
        {
            AddTileType('#', contentLoader.TileTexture);
            AddTileType('<', contentLoader.TileLeftTexture);
            AddTileType('>', contentLoader.TileRightTexture);
        }

        // Reads map data from file
        private void ReadMapData()
        {
            using (var stream = TitleContainer.OpenStream("map.txt"))
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
            player.SetPlayerMovement(new PlayerMovement(player));
            return player;
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

        public void DrawEntities(SpriteBatch spriteBatch) 
        {
            entityManager.Draw(spriteBatch);
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
            } else
            {
                throw new ArgumentOutOfRangeException();
            }
        }

        public void AddTileType(char key, Texture2D texture)
        {
            tiles.Add(key, new Tile(graphics, texture, new Vector2(0,0), TILE_SIZE));
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
                            AddTile(x, y, TileID);
                            break;
                        case '<':
                            AddTile(x, y, TileID);
                            break;
                        case '>':
                            AddTile(x, y, TileID);
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        private void AddTile(int x, int y, char TileID)
        {
            // Draw platform blocks here
            if (tiles.ContainsKey(TileID))
            {
                Tile tile = tiles[TileID];
                Tile newTile = new Tile(graphics, tile.Sprite.Texture, new Vector2(x * TILE_SIZE, y * TILE_SIZE), tile.TileSize);
                entityManager.AddEntity(newTile);
            }
            else
            {
                throw new Exception($"Tile type ({TileID}) doesn't exist");
            }
        }
    }
}
