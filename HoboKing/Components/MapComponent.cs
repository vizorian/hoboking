using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using HoboKing.Builder;
using HoboKing.Control;
using HoboKing.Entities;
using HoboKing.Factory;
using HoboKing.Graphics;
using HoboKing.Mediator;
using HoboKing.State;
using HoboKing.Memento;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using tainicom.Aether.Physics2D.Dynamics;

namespace HoboKing.Components
{
    class MapComponent : DrawableGameComponent
    {
        // Mediator stuff
        protected ConcreteMediator mediator;

        public const int PLAYER_START_POSITION_X = 20;
        public const int PLAYER_START_POSITION_Y = 93;

        // approximate size to get a 1280x1080 game with side black bars
        public const int TILE_SIZE = 20;

        public const int MAP_WIDTH = 64;
        public const int MAP_HEIGHT = 150;

        private readonly HoboKingGame hoboKingGame;

        private readonly Dictionary<char, Tile> tiles = new Dictionary<char, Tile>();

        public Connector Connector;

        // Implement Mediator
        private CritterBuilder critterBuilder;
        private EntityManager entityManager;
        
        private bool hasConnected;
        private string level;
        private int visibleTilesX;
        private int visibleTilesY;
        private float currentTime = 0f;
        private const float backupDuration = 1f;

        private World world;

        public MapComponent(HoboKingGame hoboKingGame, Connector connector) : base(hoboKingGame)
        {
            this.hoboKingGame = hoboKingGame;
            Connector = connector;
            Connector.CreateListeners();
        }

        //public void SetMediator(IMediator mediator)
        //{
        //    this.mediator = mediator;
        //}

        /// <summary>
        ///     Reads map data from file
        /// </summary>
        public void ReadMapData()
        {
            using var stream = TitleContainer.OpenStream("map/map1.txt");
            using var reader = new StreamReader(stream);
            while (!reader.EndOfStream) level += reader.ReadLine();
            reader.Close();
        }

        ///// <summary>
        /////     Prints map data to console
        ///// </summary>
        //public void Print()
        //{
        //    Console.WriteLine("Map Width: " + MAP_WIDTH);
        //    Console.WriteLine("Map Height: " + MAP_HEIGHT);
        //    Console.WriteLine("Tile Width: " + TILE_SIZE);
        //    Console.WriteLine("Tile Height: " + TILE_SIZE);
        //    Console.WriteLine("VisibleTilesX: " + visibleTilesX);
        //    Console.WriteLine("VisibleTilesY: " + visibleTilesY);
        //}

        /// <summary>
        ///     Creates main player for singleplayer
        /// </summary>
        /// <returns>Main player object</returns>
        public Player CreateMainPlayer()
        {
            var mainPlayer = new Player(ContentLoader.BatChest,
                new Vector2(PLAYER_START_POSITION_X, PLAYER_START_POSITION_Y), null, false, world);
            
            entityManager.AddEntity(mainPlayer);
            return mainPlayer;
        }

        /// <summary>
        ///     Creates a critter for debugging
        /// </summary>
        /// <returns>New critter object</returns>
        public Critter CreateDebugCritter()
        {
            //EntitiesNum.Object @object = ObjectBuilder.AddTexture(ContentLoader.Woodcutter, new Vector2(PLAYER_START_POSITION_X + 16, PLAYER_START_POSITION_Y - 2), 100)
            //    .AddMovement().Build() as EntitiesNum.Object;  

            //EntityManager.AddEntity(@object);

            var critter = (Critter) critterBuilder
                .AddTexture(ContentLoader.Woodcutter, new Vector2(PLAYER_START_POSITION_X + 16, MAP_HEIGHT - 9), 100)
                .AddMovement().Build();
            entityManager.AddEntity(critter);
            return critter;
        }

        /// <summary>
        ///     Adds Player objects for all other connected players if they aren't added in already
        /// </summary>
        public void AddConnectedPlayers()
        {
            foreach (var id in Connector.ConnectionsIds)
                if (entityManager.Players.Find(p => p.ConnectionId == id) == null)
                {
                    var p = new Player(ContentLoader.BatChest,
                        new Vector2(PLAYER_START_POSITION_X, PLAYER_START_POSITION_Y), id, true, world);

                    Console.WriteLine($"Added a new player with ID {id}");
                    entityManager.AddEntity(p);
                }
        }

        /// <summary>
        ///     Updates player positions by cycling through the input list, clearing unasigned inputs
        /// </summary>
        public void UpdateConnectedPlayers()
        {
            foreach (var coordinate in Connector.UnprocessedInputs)
            {
                // Handle first input and remove it
                var p = entityManager.Players.Find(p => p.ConnectionId == coordinate.ConnectionId);
                if (p != null)
                {
                    p.ChangePosition(new Vector2(coordinate.X, coordinate.Y));
                    Connector.UnprocessedInputs.Remove(coordinate);
                    break;
                }
                // Remove first input with no users (if user left)

                Connector.UnprocessedInputs.Remove(coordinate);
                break;
            }
        }

        /// <summary>
        ///     Removes player objects that do not have an owner (they disconnected)
        /// </summary>
        public void RemoveConnectedPlayers()
        {
            foreach (var p in entityManager.Players.Where(p => p.IsOtherPlayer && !Connector.ConnectionsIds.Contains(p.ConnectionId)))
            {
                entityManager.RemoveEntity(p);
                break;
            }
        }

        /// <summary>
        ///     Sets tile types
        /// </summary>
        private void CreateTileTypes()
        {
            tiles.Add('#', new NormalTile(ContentLoader.TileTexture, new Vector2(0, 0), TILE_SIZE, world));
            tiles.Add('<', new LeftTile(ContentLoader.TileTexture, new Vector2(0, 0), TILE_SIZE, world));
            tiles.Add('>', new RightTile(ContentLoader.TileTexture, new Vector2(0, 0), TILE_SIZE, world));
            tiles.Add('?', new FakeTile(ContentLoader.TileTexture, new Vector2(0, 0), TILE_SIZE, world));
            //tiles.Add('!', new NormalTile(ContentLoader.TileTexture, new Vector2(0, 0), TILE_SIZE, World));
        }

        ///// <summary>
        /////     Draws all entities from in manager
        ///// </summary>
        ///// <param name="spriteBatch"></param>
        //public void DrawEntities(SpriteBatch spriteBatch)
        //{
        //    entityManager.Draw(spriteBatch);
        //}

        /// <summary>
        ///     Builds the map after loading
        /// </summary>
        public void CreateMap()
        {
            for (var x = 0; x < visibleTilesX; x++)
                for (var y = 0; y < MAP_HEIGHT; y++)
                {
                    var tileId = GetTile(x, y);
                    switch (tileId)
                    {
                        case '.':
                            // Empty blocks (background tiles)
                            break;
                        case '#':
                            AddTile(x, y, tileId);
                            break;
                        case '<':
                            AddTile(x, y, tileId);
                            break;
                        case '>':
                            AddTile(x, y, tileId);
                            break;
                        case '?':
                            AddTile(x, y, tileId);
                            break;
                    }
                }

            //PrototypeDemo();
        }

        ///// <summary>
        /////     Prepares and displays the Prototype design pattern demo
        ///// </summary>
        //private void PrototypeDemo()
        //{
        //    Console.WriteLine("PROTOTYPE");
        //    Console.WriteLine("--------------------------------------");

        //    var deep1 = tiles['#'].DeepCopy() as NormalTile;
        //    if (deep1 != null)
        //    {
        //        deep1.ChangePosition(new Vector2(2 * TILE_SIZE, (MAP_HEIGHT - 10) * TILE_SIZE));

        //        var deep2 = deep1.DeepCopy() as NormalTile;
        //        deep2.ChangePosition(new Vector2(5 * TILE_SIZE, (MAP_HEIGHT - 10) * TILE_SIZE));
        //        deep2.ChangeTexture(ContentLoader.IceCenter);
        //        Console.WriteLine("Gravity before change");
        //        Console.WriteLine("{0, -15} : {1, -20}", "Deep2.gravity", deep2.Body.IgnoreGravity);
        //        Console.WriteLine("--------------------------------------");

        //        var shallow = deep2.ShallowCopy() as NormalTile;
        //        shallow.ChangePosition(new Vector2((MAP_WIDTH - 6) * TILE_SIZE, (MAP_HEIGHT - 11) * TILE_SIZE));
        //        shallow.Body.IgnoreGravity = true;

        //        Console.WriteLine("{0, -15} : {1, -20}", "Deep1", deep1.GetHashCode());
        //        Console.WriteLine("{0, -15} : {1, -20}", "Deep1.texture", deep1.Texture.GetHashCode());
        //        Console.WriteLine("{0, -15} : {1, -20}", "Deep1.body", deep1.Body.GetHashCode());
        //        Console.WriteLine("--------------------------------------");
        //        Console.WriteLine("{0, -15} : {1, -20}", "Deep2", deep2.GetHashCode());
        //        Console.WriteLine("{0, -15} : {1, -20}", "Deep2.texture", deep2.Texture.GetHashCode());
        //        Console.WriteLine("{0, -15} : {1, -20}", "Deep2.body", deep2.Body.GetHashCode());
        //        Console.WriteLine("{0, -15} : {1, -20}", "Deep2.gravity", deep2.Body.IgnoreGravity);
        //        Console.WriteLine("--------------------------------------");
        //        Console.WriteLine("{0, -15} : {1, -20}", "Shallow", shallow.GetHashCode());
        //        Console.WriteLine("{0, -15} : {1, -20}", "Shallow.texture", shallow.Texture.GetHashCode());
        //        Console.WriteLine("{0, -15} : {1, -20}", "Shallow.body", shallow.Body.GetHashCode());
        //        Console.WriteLine("Gravity is changed here");
        //        Console.WriteLine("{0, -15} : {1, -20}", "Shallow.gravity", shallow.Body.IgnoreGravity);
        //        Console.WriteLine("--------------------------------------");


        //        entityManager.AddEntity(deep1);
        //        entityManager.AddEntity(deep2);
        //        entityManager.AddEntity(shallow);
        //    }
        //}

        /// <summary>
        ///     Gets a tile at specific coordinates
        /// </summary>
        /// <param name="x">X coordinate</param>
        /// <param name="y">Y coordinate</param>
        /// <returns>Symbol of the tile</returns>
        public char GetTile(int x, int y)
        {
            if (x >= 0 && x < MAP_WIDTH && y >= 0 && y < MAP_HEIGHT)
                return level[y * MAP_WIDTH + x];
            return ' ';
        }

        /// <summary>
        ///     Adds a tile entity at specific coordinates
        /// </summary>
        /// <param name="x">X coordinate</param>
        /// <param name="y">Y coordinates</param>
        /// <param name="tileId">Tile type</param>
        private void AddTile(int x, int y, char tileId)
        {
            var tilePosition = new Vector2(x * TILE_SIZE, y * TILE_SIZE);
            // Draw platform blocks here
            if (tiles.ContainsKey(tileId))
            {
                var tile = tiles[tileId];

                switch (tile)
                {
                    case NormalTile _:
                    {
                        if (tile.DeepCopy() is NormalTile prototype)
                        {
                            prototype.ChangePosition(tilePosition);
                            entityManager.AddEntity(prototype);
                        }

                        break;
                    }

                    case LeftTile _:
                    {
                        if (tile.DeepCopy() is LeftTile prototype)
                        {
                            prototype.ChangePosition(tilePosition);
                            entityManager.AddEntity(prototype);
                        }

                        break;
                    }

                    case RightTile _:
                    {
                        if (tile.DeepCopy() is RightTile prototype)
                        {
                            prototype.ChangePosition(tilePosition);
                            entityManager.AddEntity(prototype);
                        }

                        break;
                    }

                    case FakeTile _:
                    {
                        if (tile.DeepCopy() is FakeTile prototype)
                        {
                            prototype.ChangePosition(tilePosition);
                            entityManager.AddEntity(prototype);
                        }

                        break;
                    }
                }
            }
            else
            {
                throw new Exception($"Tile type ({tileId}) doesn't exist");
            }
        }

        /// <summary>
        ///     Adds different sections of the map at specific heights
        /// </summary>
        public void AddSections()
        {
            var creator = new MapCreator();

            var standardTiles = entityManager.GetTiles();

            var sandSection = creator.CreateMapSection(standardTiles, level, MAP_WIDTH, MAP_HEIGHT, 0, 50);
            sandSection.TemplateMethod();

            var iceSection = creator.CreateMapSection(standardTiles, level, MAP_WIDTH, MAP_HEIGHT, 50, 100);
            iceSection.TemplateMethod();

            var grassSection = creator.CreateMapSection(standardTiles, level, MAP_WIDTH, MAP_HEIGHT, 100, 150);
            grassSection.TemplateMethod();
        }

        /// <summary>
        ///     Loads the component's non-graphical resources
        /// </summary>
        public override void Initialize()
        {
            //hoboKingGame.GState = HoboKingGame.GameState.Loading;

            world = new World(Vector2.UnitY * 9.8f);
            entityManager = new EntityManager();
            critterBuilder = new CritterBuilder();

            visibleTilesX = HoboKingGame.GAME_WINDOW_WIDTH / TILE_SIZE; // 64
            visibleTilesY = HoboKingGame.GAME_WINDOW_HEIGHT / TILE_SIZE; // 50

            ReadMapData();

            base.Initialize();
        }

        /// <summary>
        ///     Loads the component's graphical resources
        /// </summary>
        protected override void LoadContent()
        {
            CreateTileTypes();
            CreateMap();
            AddSections();

            var player = CreateMainPlayer();
            mediator = new ConcreteMediator(this, player, new Caretaker());
            mediator.Notify(this, "assignPlayer");
            mediator.Notify(this, "load");

            CreateDebugCritter();
            EntityManager.Reset();
            //UpdateTextures();
            base.LoadContent();
        }

        /// <summary>
        ///     Updates the component
        /// </summary>
        /// <param name="gameTime">Time of the game</param>
        public override void Update(GameTime gameTime)
        {
            if (InputController.KeyPressed(Keys.Escape) && hoboKingGame.gameState is Playing)
            {
                //hoboKingGame.ChangeStateAndDestroy(new PauseMenu(hoboKingGame, GraphicsDevice));
                //hoboKingGame.gameState.SetVisible(true);
                //hoboKingGame.GState = HoboKingGame.GameState.Unloading;
            }

            // future pause menu
            //hoboKingGame.menuScene.AddComponent(hoboKingGame.pauseComponent);


            if (!hasConnected)
            {
                //hoboKingGame.GState = HoboKingGame.GameState.Multiplayer;
                // !!!
                _ = Connector.Connect();
                mediator.SetId(Connector.GetConnectionId());
                hasConnected = true;
            }
            else
            {
                _ = Connector.SendData(gameTime, mediator.GetPosition());
                AddConnectedPlayers();
                UpdateConnectedPlayers();
                RemoveConnectedPlayers();
            }

            currentTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (mediator.GetId() != null)
            {
                if (currentTime >= backupDuration)
                {
                    currentTime -= backupDuration;
                    mediator.Notify(this, "save");
                }
            }

            world.Step((float) gameTime.ElapsedGameTime.TotalSeconds);

            mediator.Notify(this, "cameraFollow");

            entityManager.Update(gameTime);
            base.Update(gameTime);
        }

        /// <summary>
        ///     Draws the component
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Draw(GameTime gameTime)
        {
            hoboKingGame.SpriteBatch.Begin(transformMatrix: mediator.GetMatrix());
            GraphicsDevice.Clear(Color.AliceBlue);
            hoboKingGame.SpriteBatch.Draw(ContentLoader.Background,
                new Rectangle(0, 0, HoboKingGame.GAME_WINDOW_WIDTH, HoboKingGame.GAME_WINDOW_HEIGHT), Color.White);
            entityManager.Draw(hoboKingGame.SpriteBatch);
            hoboKingGame.SpriteBatch.End();
            base.Draw(gameTime);
        }
    }
}