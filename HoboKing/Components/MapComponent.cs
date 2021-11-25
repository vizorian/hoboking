using HoboKing.Control;
using HoboKing.Entities;
using HoboKing.Factory;
using HoboKing.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.IO;
using tainicom.Aether.Physics2D.Dynamics;

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

        private readonly HoboKingGame hoboKingGame;

        private EntityManager entityManager;

        public CritterBuilder CritterBuilder;

        public ConnectorComponent Connector;

        private Camera camera;

        private int visibleTilesX { get; set; }
        private int visibleTilesY { get; set; }
        private string level { get; set; }

        readonly Dictionary<char, Tile> Tiles = new Dictionary<char, Tile>();

        private bool hasConnected = false;

        private World world;
        private Player player;

        public MapComponent(HoboKingGame hoboKingGame) : base(hoboKingGame)
        {
            this.hoboKingGame = hoboKingGame;
        }

        public MapComponent(HoboKingGame hoboKingGame, ConnectorComponent connector) : base(hoboKingGame)
        {
            this.hoboKingGame = hoboKingGame;
            this.Connector = connector;
            Connector.CreateListeners();
        }

        /// <summary>
        /// Reads map data from file
        /// </summary>
        public void ReadMapData()
        {
            using var stream = TitleContainer.OpenStream("map/map1.txt");
            using var reader = new StreamReader(stream);
            while (!reader.EndOfStream)
            {
                level += reader.ReadLine();
            }
            reader.Close();
        }

        /// <summary>
        /// Prints map data to console
        /// </summary>
        public void Print()
        {
            Console.WriteLine("Map Width: " + MAP_WIDTH);
            Console.WriteLine("Map Height: " + MAP_HEIGHT);
            Console.WriteLine("Tile Width: " + TILE_SIZE);
            Console.WriteLine("Tile Height: " + TILE_SIZE);
            Console.WriteLine("VisibleTilesX: " + visibleTilesX);
            Console.WriteLine("VisibleTilesY: " + visibleTilesY);
        }

        /// <summary>
        /// Creates main player for singleplayer
        /// </summary>
        /// <returns>Main player object</returns>
        public Player CreateMainPlayer()
        {
            var player = new Player(ContentLoader.BatChest, new Vector2(PLAYER_START_POSITION_X, PLAYER_START_POSITION_Y), null, false, world);
            entityManager.AddEntity(player);
            return player;
        }

        /// <summary>
        /// Creates a critter for debugging
        /// </summary>
        /// <returns>New critter object</returns>
        public Critter CreateDebugCritter()
        {
            //Entities.Object @object = ObjectBuilder.AddTexture(ContentLoader.Woodcutter, new Vector2(PLAYER_START_POSITION_X + 16, PLAYER_START_POSITION_Y - 2), 100)
            //    .AddMovement().Build() as Entities.Object;  

            //EntityManager.AddEntity(@object);

            var critter = (Critter)CritterBuilder.AddTexture(ContentLoader.Woodcutter, new Vector2(PLAYER_START_POSITION_X + 16, MAP_HEIGHT-9), 100).AddMovement().Build();
            entityManager.AddEntity(critter);
            return critter;
        }

        /// <summary>
        /// Adds Player objects for all other connected players if they aren't added in already
        /// </summary>
        public void AddConnectedPlayers()
        {
            foreach (var id in Connector.ConnectionsIds)
            {
                if (entityManager.players.Find(p => p.ConnectionId == id) == null)
                {
                    var p = new Player(ContentLoader.BatChest, new Vector2(PLAYER_START_POSITION_X, PLAYER_START_POSITION_Y), id, true, world);

                    Console.WriteLine($"Added a new player with ID {id}");
                    entityManager.AddEntity(p);
                }
            }
        }

        /// <summary>
        /// Updates player positions by cycling through the input list, clearing unasigned inputs
        /// </summary>
        public void UpdateConnectedPlayers()
        {
            foreach (var coordinate in Connector.UnprocessedInputs)
            {
                // Handle first input and remove it
                var p = entityManager.players.Find(p => p.ConnectionId == coordinate.ConnectionID);
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

        /// <summary>
        /// Removes player objects that do not have an owner (they disconnected)
        /// </summary>
        public void RemoveConnectedPlayers()
        {
            foreach (var player in entityManager.players)
            {
                if (player.IsOtherPlayer && !Connector.ConnectionsIds.Contains(player.ConnectionId))
                {
                    entityManager.RemoveEntity(player);
                    break;
                }
            }
        }

        /// <summary>
        /// Sets tile types
        /// </summary>
        private void CreateTileTypes()
        {
            Tiles.Add('#', new NormalTile(ContentLoader.TileTexture, new Vector2(0, 0), TILE_SIZE, world));
            Tiles.Add('<', new LeftTile(ContentLoader.TileTexture, new Vector2(0, 0), TILE_SIZE, world));
            Tiles.Add('>', new RightTile(ContentLoader.TileTexture, new Vector2(0, 0), TILE_SIZE, world));
            Tiles.Add('?', new FakeTile(ContentLoader.TileTexture, new Vector2(0, 0), TILE_SIZE, world));
            //Tiles.Add('!', new NormalTile(ContentLoader.TileTexture, new Vector2(0, 0), TILE_SIZE, World));
        }

        /// <summary>
        /// Draws all entities from in manager
        /// </summary>
        /// <param name="spriteBatch"></param>
        public void DrawEntities(SpriteBatch spriteBatch)
        {
            entityManager.Draw(spriteBatch);
        }

        /// <summary>
        /// Builds the map after loading
        /// </summary>
        public void CreateMap()
        {
            for (int x = 0; x < visibleTilesX; x++)
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
                        default:
                            break;
                    }
                }
            }

            PrototypeDemo();
        }

        /// <summary>
        /// Prepares and displays the Prototype design pattern demo
        /// </summary>
        private void PrototypeDemo()
        {
            Console.WriteLine("PROTOTYPE");
            Console.WriteLine("--------------------------------------");

            var deep1 = Tiles['#'].DeepCopy() as NormalTile;
            deep1.ChangePosition(new Vector2(2 * TILE_SIZE, (MAP_HEIGHT-10) * TILE_SIZE));

            var deep2 = deep1.DeepCopy() as NormalTile;
            deep2.ChangePosition(new Vector2(5 * TILE_SIZE, (MAP_HEIGHT - 10) * TILE_SIZE));
            deep2.ChangeTexture(ContentLoader.IceCenter);
            Console.WriteLine("Gravity before change");
            Console.WriteLine(String.Format("{0, -15} : {1, -20}", "Deep2.gravity", deep2.Body.IgnoreGravity));
            Console.WriteLine("--------------------------------------");

            var shallow = deep2.ShallowCopy() as NormalTile;
            shallow.ChangePosition(new Vector2((MAP_WIDTH - 6) * TILE_SIZE, (MAP_HEIGHT - 11) * TILE_SIZE));
            shallow.Body.IgnoreGravity = true;

            Console.WriteLine(String.Format("{0, -15} : {1, -20}", "Deep1", deep1.GetHashCode()));
            Console.WriteLine(String.Format("{0, -15} : {1, -20}", "Deep1.texture", deep1.Texture.GetHashCode()));
            Console.WriteLine(String.Format("{0, -15} : {1, -20}", "Deep1.body", deep1.Body.GetHashCode()));
            Console.WriteLine("--------------------------------------");
            Console.WriteLine(String.Format("{0, -15} : {1, -20}", "Deep2", deep2.GetHashCode()));
            Console.WriteLine(String.Format("{0, -15} : {1, -20}", "Deep2.texture", deep2.Texture.GetHashCode()));
            Console.WriteLine(String.Format("{0, -15} : {1, -20}", "Deep2.body", deep2.Body.GetHashCode()));
            Console.WriteLine(String.Format("{0, -15} : {1, -20}", "Deep2.gravity", deep2.Body.IgnoreGravity));
            Console.WriteLine("--------------------------------------");
            Console.WriteLine(String.Format("{0, -15} : {1, -20}", "Shallow", shallow.GetHashCode()));
            Console.WriteLine(String.Format("{0, -15} : {1, -20}", "Shallow.texture", shallow.Texture.GetHashCode()));
            Console.WriteLine(String.Format("{0, -15} : {1, -20}", "Shallow.body", shallow.Body.GetHashCode()));
            Console.WriteLine("Gravity is changed here");
            Console.WriteLine(String.Format("{0, -15} : {1, -20}", "Shallow.gravity", shallow.Body.IgnoreGravity));
            Console.WriteLine("--------------------------------------");


            entityManager.AddEntity(deep1);
            entityManager.AddEntity(deep2);
            entityManager.AddEntity(shallow);
        }

        /// <summary>
        /// Gets a tile at specific coordinates
        /// </summary>
        /// <param name="x">X coordinate</param>
        /// <param name="y">Y coordinate</param>
        /// <returns>Symbol of the tile</returns>
        public char GetTile(int x, int y)
        {
            if (x >= 0 && x < MAP_WIDTH && y >= 0 && y < MAP_HEIGHT)
            {
                return level[y * MAP_WIDTH + x];
            }
            else return ' ';
        }

        /// <summary>
        /// Adds a tile entity at specific coordinates
        /// </summary>
        /// <param name="x">X coordinate</param>
        /// <param name="y">Y coordinates</param>
        /// <param name="tileID">Tile type</param>
        private void AddTile(int x, int y, char tileID)
        {
            var demo = false;
            var tilePosition = new Vector2(x * TILE_SIZE, y * TILE_SIZE);
            // Draw platform blocks here
            if (Tiles.ContainsKey(tileID))
            {
                var tile = Tiles[tileID];

                switch (tile)
                {
                    case NormalTile _:
                        {
                            var prototype = !demo ? tile.DeepCopy() as NormalTile : tile.ShallowCopy() as NormalTile;
                            prototype.ChangePosition(tilePosition);
                            entityManager.AddEntity(prototype);
                            break;
                        }

                    case LeftTile _:
                        {
                            var prototype = !demo ? tile.DeepCopy() as LeftTile : tile.ShallowCopy() as LeftTile;
                            prototype.ChangePosition(tilePosition);
                            entityManager.AddEntity(prototype);
                            break;
                        }

                    case RightTile _:
                        {
                            var prototype = !demo ? tile.DeepCopy() as RightTile : tile.ShallowCopy() as RightTile;
                            prototype.ChangePosition(tilePosition);
                            entityManager.AddEntity(prototype);
                            break;
                        }

                    case FakeTile _:
                        {
                            var prototype = !demo ? tile.DeepCopy() as FakeTile : tile.ShallowCopy() as FakeTile;
                            prototype.ChangePosition(tilePosition);
                            entityManager.AddEntity(prototype);
                            break;
                        }
                }
            }
            else
            {
                throw new Exception($"Tile type ({tileID}) doesn't exist");
            }
        }

        /// <summary>
        /// Adds different sections of the map at specific heights
        /// </summary>
        public void AddSections()
        {
            var creator = new MapCreator();

            var standardTiles = entityManager.GetTiles();

            var sandSection = creator.CreateMapSection(standardTiles, level, MAP_WIDTH, MAP_HEIGHT, 0, 50);
            sandSection.UpdateTextures();

            var iceSection = creator.CreateMapSection(standardTiles, level, MAP_WIDTH, MAP_HEIGHT, 50, 100);
            iceSection.UpdateTextures();

            var grassSection = creator.CreateMapSection(standardTiles, level, MAP_WIDTH, MAP_HEIGHT, 100, 150);
            grassSection.UpdateTextures();

        }

        /// <summary>
        /// Loads the component's non-graphical resources
        /// </summary>
        public override void Initialize()
        {
            hoboKingGame.gameState = HoboKingGame.GameState.Loading;

            world = new World(Vector2.UnitY * 9.8f);
            entityManager = new EntityManager();
            CritterBuilder = new CritterBuilder();

            visibleTilesX = HoboKingGame.GAME_WINDOW_WIDTH / TILE_SIZE; // 64
            visibleTilesY = HoboKingGame.GAME_WINDOW_HEIGHT / TILE_SIZE; // 50

            ReadMapData();
            
            base.Initialize();
        }

        /// <summary>
        /// Loads the component's graphical resources
        /// </summary>
        protected override void LoadContent()
        {
            ContentLoader.LoadContent(hoboKingGame.Content);
            CreateTileTypes();
            CreateMap();
            AddSections();
            player = CreateMainPlayer();
            CreateDebugCritter();
            //UpdateTextures();
            camera = new Camera();
            base.LoadContent();
        }

        /// <summary>
        /// Updates the component
        /// </summary>
        /// <param name="gameTime">Time of the game</param>
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
                // !!!
                Connector.Connect();
                player.ConnectionId = Connector.GetConnectionId();
                hasConnected = true;
            }

            if (Connector != null && hasConnected)
            {
                Connector.SendData(gameTime, player.Position);
                AddConnectedPlayers();
                UpdateConnectedPlayers();
                RemoveConnectedPlayers();
            }

            world.Step((float)gameTime.ElapsedGameTime.TotalSeconds);
            
            if(entityManager.mainPlayer != null)
                camera.Follow(entityManager.mainPlayer);

            entityManager.Update(gameTime);
            base.Update(gameTime);
        }

        /// <summary>
        /// Draws the component
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Draw(GameTime gameTime)
        {
            hoboKingGame.SpriteBatch.Begin(transformMatrix: camera.Transform);
            hoboKingGame.SpriteBatch.Draw(ContentLoader.Background, new Rectangle(0, 0, HoboKingGame.GAME_WINDOW_WIDTH, HoboKingGame.GAME_WINDOW_HEIGHT), Color.White);
            entityManager.Draw(hoboKingGame.SpriteBatch);
            hoboKingGame.SpriteBatch.End();
            base.Draw(gameTime);
        }
    }
}