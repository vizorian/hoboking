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
            Player player = new Player(graphics, ContentLoader.BatChest, new Vector2(HOBO_START_POSITION_X, HOBO_START_POSITION_Y), 
                connector.GetConnectionId(), false);
            entityManager.AddEntity(player);
            player.SetMovementStrategy(new DebugMovement(player));
            return player;
        }

        public Critter CreateDebugCritter()
        {
            Critter critter = critterBuilder.AddTexture(graphics, ContentLoader.Woodcutter, new Vector2(HOBO_START_POSITION_X+16, HOBO_START_POSITION_Y+36), 100)
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
                    Player p = new Player(graphics, ContentLoader.BatChest, new Vector2(HOBO_START_POSITION_X, HOBO_START_POSITION_Y), id, true);

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
            ContentLoader.LoadContent(contentManager);
            CreateTileTypes();
        }

        // This is where you can set tile types for now
        private void CreateTileTypes()
        {
            NormalTileFactory normalTileFactory = new NormalTileFactory();
            tiles.Add('#', normalTileFactory.CreateStandard(graphics, ContentLoader.TileTexture, new Vector2(0, 0), TILE_SIZE));
            tiles.Add('<', normalTileFactory.CreateSlopeLeft(graphics, new Vector2(0, 0), TILE_SIZE));
            tiles.Add('>', normalTileFactory.CreateSlopeRight(graphics, new Vector2(0, 0), TILE_SIZE));
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
            IceTileFactory iceTileFactory = new IceTileFactory();
            SlowTileFactory slowTileFactory = new SlowTileFactory();

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
                            AddTile(x, y, TileID, normalTileFactory, "standard");            
                            break;
                        case '<':
                            AddTile(x, y, TileID, normalTileFactory, "slopeleft");
                            break;
                        case '>':
                            AddTile(x, y, TileID, normalTileFactory, "sloperight");
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        // Cycle through standard tile entities and apply appropriate texture
        public void UpdateTextures()
        {
            List<Tile> standardTiles = entityManager.GetStandardTiles();
            // Tile specificTile = standardTiles.Find(o => o.Sprite.Position.X == 2 && o.Sprite.Position.Y == 2);
            for (int x = 0; x < VisibleTilesX; x++)
            {
                for (int y = 0; y < VisibleTilesY; y++)
                {
                    char TileID = GetTile(x, y);
                    if (TileID == '#')
                    {
                        Tile specificTile = standardTiles.Find(o => o.Sprite.Position.X == x * 20 && o.Sprite.Position.Y == y * 20);
                        
                        bool hasNorth = false;
                        bool hasEast = false;
                        bool hasSouth = false;
                        bool hasWest = false;
                        bool hasNW = false;
                        bool hasNE = false;
                        bool hasSW = false;
                        bool hasSE = false;

                        if (GetTile(x, y-1) != '.') hasNorth = true;
                        if (GetTile(x+1, y) != '.') hasEast = true;
                        if (GetTile(x, y+1) != '.') hasSouth = true;
                        if (GetTile(x-1, y) != '.') hasWest = true;

                        if (GetTile(x-1, y-1) != '.') hasNW = true;
                        if (GetTile(x+1, y-1) != '.') hasNE = true;
                        if (GetTile(x-1, y+1) != '.') hasSW = true;
                        if (GetTile(x+1, y+1) != '.') hasSE = true;

                        // NW
                        if (!hasNorth && hasEast && hasSouth && !hasWest)
                        {
                            specificTile.Sprite.ChangeTexture(ContentLoader.GrassNW); continue;
                        }
                        // N
                        else if (!hasNorth && hasEast && hasSouth && hasWest)
                        {
                            specificTile.Sprite.ChangeTexture(ContentLoader.GrassN); continue;
                        }
                        // NE
                        else if (!hasNorth && !hasEast && hasSouth && hasWest)
                        {
                            specificTile.Sprite.ChangeTexture(ContentLoader.GrassNE); continue;
                        }
                        // W
                        else if (hasNorth && hasEast && hasSouth && !hasWest)
                        {
                            specificTile.Sprite.ChangeTexture(ContentLoader.GrassW); continue;
                        }
                        // Center
                        else if (hasNorth && hasEast && hasSouth && hasWest)
                        {
                            // Check for corners
                            // Corner NW
                            if(!hasNW && hasNE && hasSW && hasSE)
                            {
                                specificTile.Sprite.ChangeTexture(ContentLoader.GrassCornerNW); continue;
                            }
                            // Corner ME
                            else if (hasNW && !hasNE && hasSW && hasSE)
                            {
                                specificTile.Sprite.ChangeTexture(ContentLoader.GrassCornerNE); continue;
                            }
                            // Corner SW
                            else if (hasNW && hasNE && !hasSW && hasSE)
                            {
                                specificTile.Sprite.ChangeTexture(ContentLoader.GrassCornerSW); continue;
                            }
                            // Corner SE
                            else if (hasNW && hasNE && hasSW && !hasSE)
                            {
                                specificTile.Sprite.ChangeTexture(ContentLoader.GrassCornerSE); continue;
                            }
                            else
                            {
                                // No corner tiles
                                specificTile.Sprite.ChangeTexture(ContentLoader.GrassCenter); continue;
                            }
                        }
                        // E
                        else if (hasNorth && !hasEast && hasSouth && hasWest)
                        {
                            specificTile.Sprite.ChangeTexture(ContentLoader.GrassE); continue;
                        }
                        // SW
                        else if (hasNorth && hasEast && !hasSouth && !hasWest)
                        {
                            specificTile.Sprite.ChangeTexture(ContentLoader.GrassSW); continue;
                        }
                        // S
                        else if (hasNorth && hasEast && !hasSouth && hasWest)
                        {
                            specificTile.Sprite.ChangeTexture(ContentLoader.GrassS); continue;
                        }
                        // SE
                        else if (hasNorth && !hasEast && !hasSouth && hasWest)
                        {
                            specificTile.Sprite.ChangeTexture(ContentLoader.GrassSE); continue;
                        }
                        else
                        {
                            // Default value N for now
                            // To dynamically change according to current tile type, make default value a new blank texture and look for it do discern
                            specificTile.Sprite.ChangeTexture(ContentLoader.GrassN);
                        }
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

        private void AddTile(int x, int y, char TileID, AbstractTileFactory tileFactory, string tileType)
        {        
            // Draw platform blocks here
            if (tiles.ContainsKey(TileID))
            {
                Tile tile = tiles[TileID];

                if (tileType == "standard")
                {
                    Tile newTile = tileFactory.CreateStandard(graphics, tile.Sprite.Texture, new Vector2(x * TILE_SIZE, y * TILE_SIZE), tile.TileSize);
                    entityManager.AddEntity(newTile);
                }
                else if (tileType == "slopeleft")
                {
                    Tile newTile = tileFactory.CreateSlopeLeft(graphics, new Vector2(x * TILE_SIZE, y * TILE_SIZE), tile.TileSize);
                    entityManager.AddEntity(newTile);
                }
                else if (tileType == "sloperight")
                {
                    Tile newTile = tileFactory.CreateSlopeRight(graphics, new Vector2(x * TILE_SIZE, y * TILE_SIZE), tile.TileSize);
                    entityManager.AddEntity(newTile);
                }
                
            }
            else
            {
                throw new Exception($"Tile type ({TileID}) doesn't exist");
            }
        }


    }
}
