using HoboKing.Control;
using HoboKing.Entities;
using HoboKing.Factory;
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
        public const int MAP_HEIGHT = 150;

        private HoboKingGame hoboKingGame;

        private EntityManager EntityManager;
        private ObjectBuilder ObjectBuilder;

        public CritterBuilder CritterBuilder;

        private GraphicsDevice Graphics;

        private ConnectorComponent Connector;

        private Camera Camera;

        private int VisibleTilesX { get; set; }
        private int VisibleTilesY { get; set; }
        private string Level { get; set; }

        readonly Dictionary<char, Tile> Tiles = new Dictionary<char, Tile>();

        private bool hasConnected = false;

        private World World;
        private Player Player;

        public MapComponent(HoboKingGame hoboKingGame) : base(hoboKingGame)
        {
            this.hoboKingGame = hoboKingGame;
        }

        public MapComponent(HoboKingGame hoboKingGame, ConnectorComponent connector) : base(hoboKingGame)
        {
            this.hoboKingGame = hoboKingGame;
            this.Connector = connector;
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
            //Entities.Object @object = ObjectBuilder.AddTexture(ContentLoader.Woodcutter, new Vector2(PLAYER_START_POSITION_X + 16, PLAYER_START_POSITION_Y - 2), 100)
            //    .AddMovement().Build() as Entities.Object;  

            //EntityManager.AddEntity(@object);

            Critter critter = (Critter)CritterBuilder.AddTexture(ContentLoader.Woodcutter, new Vector2(PLAYER_START_POSITION_X + 16, PLAYER_START_POSITION_Y - 2), 100).AddMovement().Build();
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
                    p.ChangePosition(new Vector2(coordinate.X, coordinate.Y));
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
            Tiles.Add('#', new NormalTile(ContentLoader.TileTexture, new Vector2(0, 0), TILE_SIZE, World));
            Tiles.Add('<', new LeftTile(ContentLoader.TileTexture, new Vector2(0, 0), TILE_SIZE, World));
            Tiles.Add('>', new RightTile(ContentLoader.TileTexture, new Vector2(0, 0), TILE_SIZE, World));
            Tiles.Add('?', new FakeTile(ContentLoader.TileTexture, new Vector2(0, 0), TILE_SIZE, World));
            //Tiles.Add('!', new NormalTile(ContentLoader.TileTexture, new Vector2(0, 0), TILE_SIZE, World));
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
                            AddTile(x, y, TileID);
                            break;
                        case '<':
                            AddTile(x, y, TileID);
                            break;
                        case '>':
                            AddTile(x, y, TileID);
                            break;
                        case '?':
                            AddTile(x, y, TileID);
                            break;
                        case '!':
                            AddTile(x, y, TileID);
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

        private void AddTile(int x, int y, char tileID)
        {
            bool demo = false;
            Vector2 tilePosition = new Vector2(x * TILE_SIZE, y * TILE_SIZE);
            // Draw platform blocks here
            if (Tiles.ContainsKey(tileID))
            {
                Tile tile = Tiles[tileID];

                if (tile is NormalTile)
                {
                    Tile prototype = !demo ? tile.DeepCopy() as NormalTile : tile.ShallowCopy() as NormalTile;
                    prototype.ChangePosition(tilePosition);
                    EntityManager.AddEntity(prototype);
                }
                else if (tile is LeftTile)
                {
                    Tile prototype = !demo ? tile.DeepCopy() as LeftTile : tile.ShallowCopy() as LeftTile;
                    prototype.ChangePosition(tilePosition);
                    EntityManager.AddEntity(prototype);
                }
                else if (tile is RightTile)
                {
                    Tile prototype = !demo ? tile.DeepCopy() as RightTile : tile.ShallowCopy() as RightTile;
                    prototype.ChangePosition(tilePosition);
                    EntityManager.AddEntity(prototype);
                }
                else if (tile is FakeTile)
                {
                    Tile prototype = !demo ? tile.DeepCopy() as FakeTile : tile.ShallowCopy() as FakeTile;
                    prototype.ChangePosition(tilePosition);
                    EntityManager.AddEntity(prototype);
                }
                else if (tile is SpikeTile)
                {
                    Tile prototype = !demo ? tile.DeepCopy() as SpikeTile : tile.ShallowCopy() as SpikeTile;
                    prototype.ChangePosition(tilePosition);
                    EntityManager.AddEntity(prototype);
                }
            }
            else
            {
                throw new Exception($"Tile type ({tileID}) doesn't exist");
            }
        }

        public void AddSections()
        {
            Creator creator = new MapCreator();
            
            Section sandSection = creator.CreateMapSection(EntityManager, Level, MAP_WIDTH, MAP_HEIGHT, 0, 50);
            sandSection.UpdateTextures();

            Section iceSection = creator.CreateMapSection(EntityManager, Level, MAP_WIDTH, MAP_HEIGHT, 50, 100);
            iceSection.UpdateTextures();

            Section grassSection = creator.CreateMapSection(EntityManager, Level, MAP_WIDTH, MAP_HEIGHT, 100, 150);
            grassSection.UpdateTextures();

        }

        public override void Initialize()
        {
            hoboKingGame.gameState = HoboKingGame.GameState.Loading;

            World = new World(Vector2.UnitY * 9.8f);
            EntityManager = new EntityManager();
            ObjectBuilder = new ObjectBuilder();
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
            AddSections();
            Player = CreateMainPlayer();
            CreateDebugCritter();
            //UpdateTextures();
            Camera = new Camera();
            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            if (InputController.KeyPressed(Keys.Escape))
            {
                hoboKingGame.SwitchScene(hoboKingGame.menuScene);
                hoboKingGame.gameState = HoboKingGame.GameState.Unloading;
            }

            // future pause menu
            //hoboKingGame.menuScene.AddComponent(hoboKingGame.pauseComponent);


            if (Connector != null && !hasConnected)
            {
                hoboKingGame.gameState = HoboKingGame.GameState.Multiplayer;
                Connector.Connect();
                Player.ConnectionId = Connector.GetConnectionId();
                hasConnected = true;
            }

            if (Connector != null && hasConnected)
            {
                Connector.SendData(gameTime, Player.Position);
                AddConnectedPlayers();
                UpdateConnectedPlayers();
                RemoveConnectedPlayers();
            }

            World.Step((float)gameTime.ElapsedGameTime.TotalSeconds);
            
            if(EntityManager.mainPlayer != null)
                Camera.Follow(EntityManager.mainPlayer);

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