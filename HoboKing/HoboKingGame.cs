using HoboKing.Control;
using HoboKing.Entities;
using HoboKing.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Diagnostics;
using System.Linq;

namespace HoboKing
{
    public class HoboKingGame : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        // The app window size
        public const int WINDOW_WIDTH = 1920;
        public const int WINDOW_HEIGHT = 1080;

        // The game window size inside the app (sides are black bars)
        public const int GAME_WINDOW_WIDTH = 1280;

        // should be 1080, reduced for fitting in screen
        public const int GAME_WINDOW_HEIGHT = 1000;

        public const int HOBO_START_POSITION_X = 2;
        public const int HOBO_START_POSITION_Y = 8;

        // approximate size to get a 1280x1080 game with side black bars
        public const int TILE_SIZE = 20;

        private const string ASSET_NAME_HOBO = "batchest";
        private const string ASSET_NAME_TILE = "ground";
        private const string ASSET_NAME_TILE_LEFT = "ground_left";
        private const string ASSET_NAME_TILE_RIGHT = "ground_right";
        private const string ASSET_NAME_SFX_JUMP = "jump";

        private SpriteFont font;
        private string playerCount;
        private double hoboHeight;

        private Texture2D tileTexture;
        private Texture2D tileLeftTexture;
        private Texture2D tileRightTexture;
        private Texture2D hoboTexture;
        private SoundEffect jumpSound;

        private Map map;
        private Player player;

        private InputController inputController;
        private EntityManager entityManager;
        private Connector connector;

        private float timer = 0.1f;
        const float TIMER = 0.1f;

        public HoboKingGame()
        {
            IsMouseVisible = true;
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            entityManager = new EntityManager();
           
            connector = new Connector();
        }

        protected override void Initialize()
        {
            base.Initialize();
            _graphics.PreferredBackBufferWidth = GAME_WINDOW_WIDTH;
            _graphics.PreferredBackBufferHeight = GAME_WINDOW_HEIGHT;
            _graphics.IsFullScreen = false;
            _graphics.ApplyChanges();
        }


        protected override void LoadContent()
        {
            connector.Connect();
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            LoadTextures();

            map = new Map(GAME_WINDOW_WIDTH, GAME_WINDOW_HEIGHT, TILE_SIZE, TILE_SIZE);
            map.AddTileType('#', tileTexture);
            map.AddTileType('<', tileLeftTexture);
            map.AddTileType('>', tileRightTexture);


            Console.WriteLine($"Main player's connection ID: {connector.GetConnectionID()}");
            player = new Player(hoboTexture, new Vector2(HOBO_START_POSITION_X, HOBO_START_POSITION_Y), jumpSound, connector.GetConnectionID(), false, map);
            inputController = new InputController(player);
            entityManager.AddEntity(player);
        }

        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            base.Update(gameTime);

            inputController.ProcessControls(gameTime);
            entityManager.Update(gameTime);

            playerCount = entityManager.PlayerCount.ToString();
            hoboHeight = player.Position.Y;

            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            timer -= elapsed;
            if (timer < 0)
            {
                connector.SendCoordinates(player.Position);
                timer = TIMER;
            }

            AddConnectedPlayers();
            UpdateConnectedPlayers();
            RemoveConnectedPlayers();
        }

        private void AddConnectedPlayers()
        {
            // Add Player objects for all other connected players
            foreach (var id in connector.Connections)
            {
                if (entityManager.players.Find(p => p.ConnectionId == id) == null)
                {
                    Player p = new Player(hoboTexture,
                        new Vector2(HOBO_START_POSITION_X,
                        HOBO_START_POSITION_Y), jumpSound,
                        id, true, map);


                    Console.WriteLine($"Added a new player with ID {id}");
                    entityManager.AddEntity(p);
                }
            }
        }


        private void UpdateConnectedPlayers()
        {
            // Update player positions by cycling through input list
            foreach (Coordinate coordinate in connector.Inputs)
            {
                // Handle first input and remove it
                Player p = entityManager.players.Find(p => p.ConnectionId == coordinate.ConnectionID);
                if (p != null)
                {
                    p.Position = new Vector2(coordinate.X, coordinate.Y);
                    connector.Inputs.Remove(coordinate);
                    break;
                }
                // Remove first input with no users (if user left)
                else
                {
                    connector.Inputs.Remove(coordinate);
                    break;
                }
            }
        }

        private void RemoveConnectedPlayers()
        {
            // Remove Player objects that don't have an owner and are not the main player
            foreach (var player in entityManager.players)
            {
                if (player.IsOtherPlayer)
                {
                    if (!connector.Connections.Contains(player.ConnectionId))
                    {
                        entityManager.RemoveEntity(player);
                        break;
                    }
                }
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();
            map.Draw(_spriteBatch, gameTime);

            entityManager.Draw(gameTime, _spriteBatch);

            _spriteBatch.DrawString(font, playerCount, new Vector2(450, 10), Color.Black);
            _spriteBatch.DrawString(font, "Y:" + Math.Round(hoboHeight, 2), new Vector2(450, 30), Color.Black);

            _spriteBatch.End();

            base.Draw(gameTime);
        }

        // Loads game textures
        void LoadTextures()
        {
            hoboTexture = Content.Load<Texture2D>(ASSET_NAME_HOBO);
            tileTexture = Content.Load<Texture2D>(ASSET_NAME_TILE);
            tileLeftTexture = Content.Load<Texture2D>(ASSET_NAME_TILE_LEFT);
            tileRightTexture = Content.Load<Texture2D>(ASSET_NAME_TILE_RIGHT);
            jumpSound = Content.Load<SoundEffect>(ASSET_NAME_SFX_JUMP);
            font = Content.Load<SpriteFont>("Debug");
        }
    }
}
