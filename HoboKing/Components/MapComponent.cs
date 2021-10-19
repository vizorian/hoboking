using HoboKing.Control;
using HoboKing.Entities;
using HoboKing.Entities.Factory;
using HoboKing.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using tainicom.Aether.Physics2D.Dynamics;
using tainicom.Aether.Physics2D.Diagnostics;


namespace HoboKing
{
    class MapComponent : DrawableGameComponent
    {
        public const int HOBO_START_POSITION_X = 20;
        public const int HOBO_START_POSITION_Y = 93;

        public const int TILE_SIZE = 20;
        public const int MAP_WIDTH = 64;
        public const int MAP_HEIGHT = 100;

        private EntityManager EntityManager;
        private CritterBuilder CritterBuilder;
        private GraphicsDevice Graphics;

        private Camera camera;

        private HoboKingGame hoboKingGame;
        private Texture2D background = ContentLoader.Background;

        public int VisibleTilesX { get; set; }
        public int VisibleTilesY { get; set; }
        public string Level { get; set; }

        readonly Dictionary<char, Tile> tiles = new Dictionary<char, Tile>();

        private enum GameState
        {
            Playing,
            Pause,
        }

        private GameState gameState;

        private World world;
        private DebugView debugView;
        private Player player;

        public MapComponent(HoboKingGame hoboKingGame) : base(hoboKingGame)
        {
            this.hoboKingGame = hoboKingGame;
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

        // Creates main player for singleplayer
        public Player CreateMainPlayer()
        {
            Player player = new Player(Graphics, ContentLoader.BatChest, new Vector2(HOBO_START_POSITION_X, HOBO_START_POSITION_Y), false, world);

            EntityManager.AddEntity(player);
            player.SetMovementStrategy(new PlayerMovement(player, world));
            return player;
        }

        public Critter CreateDebugCritter()
        {
            Critter critter = CritterBuilder.AddTexture(ContentLoader.Woodcutter, new Vector2(HOBO_START_POSITION_X + 16, HOBO_START_POSITION_Y + 36), 100, world)
                .AddMovement(new DebugMovement(null, world)).AddSpeech("Hello baj, I seek shelter.", 20).Build();

            EntityManager.AddEntity(critter);
            return critter;
        }

        //// Add Player objects for all other connected players
        //public void AddConnectedPlayers(Connector connector)
        //{
        //    foreach (var id in connector.ConnectionsIds)
        //    {
        //        if (entityManager.players.Find(p => p.ConnectionId == id) == null)
        //        {
        //            Player p = new Player(graphics, ContentLoader.BatChest, new Vector2(HOBO_START_POSITION_X, HOBO_START_POSITION_Y), id, true);

        //            Console.WriteLine($"Added a new player with ID {id}");
        //            entityManager.AddEntity(p);
        //        }
        //    }
        //}

        //// Update player positions by cycling through input list
        //public void UpdateConnectedPlayers(Connector connector)
        //{
        //    foreach (Coordinate coordinate in connector.UnprocessedInputs)
        //    {
        //        // Handle first input and remove it
        //        Player p = entityManager.players.Find(p => p.ConnectionId == coordinate.ConnectionID);
        //        if (p != null)
        //        {
        //            p.Sprite.Position = new Vector2(coordinate.X, coordinate.Y);
        //            connector.UnprocessedInputs.Remove(coordinate);
        //            break;
        //        }
        //        // Remove first input with no users (if user left)
        //        else
        //        {
        //            connector.UnprocessedInputs.Remove(coordinate);
        //            break;
        //        }
        //    }
        //}

        //// Remove Player objects that don't have an owner and are not the main player
        //public void RemoveConnectedPlayers(Connector connector)
        //{
        //    foreach (var player in entityManager.players)
        //    {
        //        if (player.IsOtherPlayer)
        //        {
        //            if (!connector.ConnectionsIds.Contains(player.ConnectionId))
        //            {
        //                entityManager.RemoveEntity(player);
        //                break;
        //            }
        //        }
        //    }
        //}

        // This is where you can set tile types for now
        private void CreateTileTypes()
        {
            tiles.Add('#', new Standard(ContentLoader.TileTexture, new Vector2(0, 0), TILE_SIZE, world));
            tiles.Add('<', new SlopeLeft(ContentLoader.TileTexture, new Vector2(0, 0), TILE_SIZE, world));
            tiles.Add('>', new SlopeRight(ContentLoader.TileTexture, new Vector2(0, 0), TILE_SIZE, world));
        }

        public void DrawEntities(SpriteBatch spriteBatch)
        {
            EntityManager.Draw(spriteBatch);
        }

        public void CreateMap()
        {
            for (int x = 0; x < VisibleTilesX; x++)
            {
                for (int y = 0; y < MAP_HEIGHT; y++)
                {
                    char TileID = GetTile(x, y);
                    switch (TileID)
                    {
                        case '.':
                            // Empty blocks (background tiles)
                            break;
                        case '#':
                            AddTile(x, y, TileID, "standard");
                            break;
                        case '<':
                            AddTile(x, y, TileID, "slopeleft");
                            break;
                        case '>':
                            AddTile(x, y, TileID, "sloperight");
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
            List<Tile> standardTiles = EntityManager.GetStandardTiles();
            // Tile specificTile = standardTiles.Find(o => o.Sprite.Position.X == 2 && o.Sprite.Position.Y == 2);
            for (int x = 0; x < MAP_WIDTH; x++)
            {
                for (int y = 0; y < MAP_HEIGHT; y++)
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

                        if (GetTile(x, y - 1) != '.') hasNorth = true;
                        if (GetTile(x + 1, y) != '.') hasEast = true;
                        if (GetTile(x, y + 1) != '.') hasSouth = true;
                        if (GetTile(x - 1, y) != '.') hasWest = true;

                        if (GetTile(x - 1, y - 1) != '.') hasNW = true;
                        if (GetTile(x + 1, y - 1) != '.') hasNE = true;
                        if (GetTile(x - 1, y + 1) != '.') hasSW = true;
                        if (GetTile(x + 1, y + 1) != '.') hasSE = true;

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
                            if (!hasNW && hasNE && hasSW && hasSE)
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

        private void AddTile(int x, int y, char TileID, string tileType)
        {
            // Draw platform blocks here
            if (tiles.ContainsKey(TileID))
            {
                Tile tile = tiles[TileID];

                if (tileType == "standard")
                {
                    Tile newTile = new Standard(tile.Sprite.Texture, new Vector2(x * TILE_SIZE, y * TILE_SIZE), tile.TileSize, world);
                    EntityManager.AddEntity(newTile);
                }
                else if (tileType == "slopeleft")
                {
                    Tile newTile = new SlopeLeft(tile.Sprite.Texture, new Vector2(x * TILE_SIZE, y * TILE_SIZE), tile.TileSize, world);
                    EntityManager.AddEntity(newTile);
                }
                else if (tileType == "sloperight")
                {
                    Tile newTile = new SlopeRight(tile.Sprite.Texture, new Vector2(x * TILE_SIZE, y * TILE_SIZE), tile.TileSize, world);
                    EntityManager.AddEntity(newTile);
                }

            }
            else
            {
                throw new Exception($"Tile type ({TileID}) doesn't exist");
            }
        }

        public override void Initialize()
        {
            world = new World(Vector2.UnitY * 9.8f);
            debugView = new DebugView(world);
            EntityManager = new EntityManager();
            CritterBuilder = new CritterBuilder();
            Graphics = hoboKingGame.graphics.GraphicsDevice;

            VisibleTilesX = HoboKingGame.GAME_WINDOW_WIDTH / TILE_SIZE; // 64
            VisibleTilesY = HoboKingGame.GAME_WINDOW_HEIGHT / TILE_SIZE; // 50

            ReadMapData();
            
            base.Initialize();
        }

        protected override void LoadContent()
        {
            ContentLoader.LoadContent(hoboKingGame.Content);
            CreateTileTypes();
            CreateMap();
            player = CreateMainPlayer();
            CreateDebugCritter();
            UpdateTextures();
            camera = new Camera();
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            if (InputController.KeyPressed(Keys.Escape))
                hoboKingGame.SwitchScene(hoboKingGame.menuScene);

            world.Step((float)gameTime.ElapsedGameTime.TotalSeconds);
            camera.Follow(EntityManager.mainPlayer.Sprite);
            EntityManager.Update(gameTime);
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            hoboKingGame.spriteBatch.Begin(transformMatrix: camera.Transform);
            hoboKingGame.spriteBatch.Draw(ContentLoader.Background, new Rectangle(0, 0, HoboKingGame.GAME_WINDOW_WIDTH, HoboKingGame.GAME_WINDOW_HEIGHT), Color.White);
            EntityManager.Draw(hoboKingGame.spriteBatch);
            EntityManager.DrawDebug(debugView, Graphics, hoboKingGame.Content);
            hoboKingGame.spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}