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
using HoboKing.Control.Strategy;

namespace HoboKing
{
    class MapComponent : DrawableGameComponent
    {
        public const int PLAYER_START_POSITION_X = 20;
        public const int PLAYER_START_POSITION_Y = 93;

        // approximate size to get a 1280x1080 game with side black bars
        public const int TILE_SIZE = 20;

        public const int MAP_WIDTH = 64;
        public const int MAP_HEIGHT = 100;

        private HoboKingGame hoboKingGame;

        private EntityManager EntityManager;
        private CritterBuilder CritterBuilder;
        private GraphicsDevice Graphics;

        private ConnectorComponent Connector;

        private Camera Camera;

        private int VisibleTilesX { get; set; }
        private int VisibleTilesY { get; set; }
        private string Level { get; set; }

        readonly Dictionary<char, Tile> Tiles = new Dictionary<char, Tile>();

        public enum GameState
        {
            NotPlaying,
            Playing,
        }

        public GameState gameState;

        private bool hasConnected = false;

        private World World;
        private Player Player;

        public MapComponent(HoboKingGame hoboKingGame) : base(hoboKingGame)
        {
            this.hoboKingGame = hoboKingGame;
            gameState = GameState.NotPlaying;
        }

        public MapComponent(HoboKingGame hoboKingGame, ConnectorComponent connector) : base(hoboKingGame)
        {
            this.hoboKingGame = hoboKingGame;
            this.Connector = connector;
            gameState = GameState.NotPlaying;
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
            Player player = new Player(ContentLoader.BatChest, new Vector2(PLAYER_START_POSITION_X, PLAYER_START_POSITION_Y), null, false, World);
            EntityManager.AddEntity(player);
            return player;
        }

        public Critter CreateDebugCritter()
        {
            Critter critter = CritterBuilder.AddTexture(ContentLoader.Woodcutter, new Vector2(PLAYER_START_POSITION_X + 16, PLAYER_START_POSITION_Y - 2), 100)
                .AddMovement().AddSpeech("Hello baj, I seek shelter.", 20).Build();

            EntityManager.AddEntity(critter);
            return critter;
        }

        // Add Player objects for all other connected players
        public void AddConnectedPlayers()
        {
            foreach (var id in Connector.ConnectionsIds)
            {
                if (EntityManager.players.Find(p => p.ConnectionId == id) == null)
                {
                    Player p = new Player(ContentLoader.BatChest, new Vector2(PLAYER_START_POSITION_X, PLAYER_START_POSITION_Y), id, true, World);

                    Console.WriteLine($"Added a new player with ID {id}");
                    EntityManager.AddEntity(p);
                }
            }
        }

        // Update player positions by cycling through input list
        public void UpdateConnectedPlayers()
        {
            foreach (Coordinate coordinate in Connector.UnprocessedInputs)
            {
                // Handle first input and remove it
                Player p = EntityManager.players.Find(p => p.ConnectionId == coordinate.ConnectionID);
                if (p != null)
                {
                    p.Sprite.Position = new Vector2(coordinate.X, coordinate.Y);
                    Connector.UnprocessedInputs.Remove(coordinate);
                    break;
                }
                // Remove first input with no users (if user left)
                else
                {
                    Connector.UnprocessedInputs.Remove(coordinate);
                    break;
                }
            }
        }

        // Remove Player objects that don't have an owner and are not the main player
        public void RemoveConnectedPlayers()
        {
            foreach (var player in EntityManager.players)
            {
                if (player.IsOtherPlayer)
                {
                    if (!Connector.ConnectionsIds.Contains(player.ConnectionId))
                    {
                        EntityManager.RemoveEntity(player);
                        break;
                    }
                }
            }
        }

        // This is where you can set tile types for now
        private void CreateTileTypes()
        {
            Tiles.Add('#', new Standard(ContentLoader.TileTexture, new Vector2(0, 0), TILE_SIZE, World));
            Tiles.Add('<', new SlopeLeft(ContentLoader.TileTexture, new Vector2(0, 0), TILE_SIZE, World));
            Tiles.Add('>', new SlopeRight(ContentLoader.TileTexture, new Vector2(0, 0), TILE_SIZE, World));
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

        private void AddTile(int x, int y, char tileID, string tileType)
        {
            // Draw platform blocks here
            if (Tiles.ContainsKey(tileID))
            {
                Tile tile = Tiles[tileID];

                if (tileType == "standard")
                {
                    Tile newTile = new Standard(tile.Sprite.Texture, new Vector2(x * TILE_SIZE, y * TILE_SIZE), tile.TileSize, World);
                    EntityManager.AddEntity(newTile);
                }
                else if (tileType == "slopeleft")
                {
                    Tile newTile = new SlopeLeft(ContentLoader.GrassLeft, new Vector2(x * TILE_SIZE, y * TILE_SIZE), tile.TileSize, World);
                    EntityManager.AddEntity(newTile);
                }
                else if (tileType == "sloperight")
                {
                    Tile newTile = new SlopeRight(ContentLoader.GrassRight, new Vector2(x * TILE_SIZE, y * TILE_SIZE), tile.TileSize, World);
                    EntityManager.AddEntity(newTile);
                }
            }
            else
            {
                throw new Exception($"Tile type ({tileID}) doesn't exist");
            }
        }

        public override void Initialize()
        {
            World = new World(Vector2.UnitY * 9.8f);
            EntityManager = new EntityManager();
            CritterBuilder = new CritterBuilder();
            Graphics = hoboKingGame.Graphics.GraphicsDevice;

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
            Player = CreateMainPlayer();
            CreateDebugCritter();
            UpdateTextures();
            Camera = new Camera();
            gameState = GameState.Playing;

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            if (InputController.KeyPressed(Keys.Escape))
                hoboKingGame.SwitchScene(hoboKingGame.menuScene);
            // future pause menu
            //hoboKingGame.menuScene.AddComponent(hoboKingGame.pauseComponent);


            if (Connector != null && !hasConnected)
            {
                Connector.Connect();
                Player.ConnectionId = Connector.GetConnectionId();
                hasConnected = true;
            }

            if (Connector != null && hasConnected)
            {
                Connector.SendData(gameTime, Player.Sprite.Position);
                AddConnectedPlayers();
                UpdateConnectedPlayers();
                RemoveConnectedPlayers();
            }

            World.Step((float)gameTime.ElapsedGameTime.TotalSeconds);
            Camera.Follow(EntityManager.mainPlayer.Sprite);
            EntityManager.Update(gameTime);
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            hoboKingGame.SpriteBatch.Begin(transformMatrix: Camera.Transform);
            hoboKingGame.SpriteBatch.Draw(ContentLoader.Background, new Rectangle(0, 0, HoboKingGame.GAME_WINDOW_WIDTH, HoboKingGame.GAME_WINDOW_HEIGHT), Color.White);
            EntityManager.Draw(hoboKingGame.SpriteBatch);
            hoboKingGame.SpriteBatch.End();
            base.Draw(gameTime);
        }
    }
}