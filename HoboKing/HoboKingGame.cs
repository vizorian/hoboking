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

        public const int WINDOW_WIDTH = 512;
        public const int WINDOW_HEIGHT = 1024;

        public const int HOBO_START_POSITION_X = 2;
        public const int HOBO_START_POSITION_Y = 8;

        public const int TILE_SIZE = 30;

        private const string ASSET_NAME_HOBO = "batchest";
        private const string ASSET_NAME_TILE = "ground";
        private const string ASSET_NAME_SFX_JUMP = "jump";

        private SpriteFont font;
        private string playerCount;
        private double hoboHeight;

        private Texture2D tileTexture;
        private Texture2D hoboTexture;
        private SoundEffect jumpSound;

        private Map map;
        private Player player;

        private PlayerManager playerManager;

        private InputController inputController;
        private EntityManager entityManager;
        private Connector connector;

        private float timer = 1.0f;
        const float TIMER = 1.0f;

        public HoboKingGame()
        {
            IsMouseVisible = true;
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            entityManager = new EntityManager();
            playerManager = new PlayerManager(entityManager);
            connector = new Connector();
        }

        protected override void Initialize()
        {
            base.Initialize();
            _graphics.PreferredBackBufferWidth = WINDOW_WIDTH;
            _graphics.PreferredBackBufferHeight = WINDOW_HEIGHT;
            _graphics.ApplyChanges();
             connector.Connect();
        }


        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            LoadTextures();

            map = new Map(WINDOW_WIDTH, WINDOW_HEIGHT, TILE_SIZE, TILE_SIZE);
            map.AddTileType('#', tileTexture);

            player = new Player(hoboTexture, new Vector2(HOBO_START_POSITION_X, HOBO_START_POSITION_Y), jumpSound, map);

            inputController = new InputController(player);
            entityManager.AddEntity(map);
            entityManager.AddEntity(player);
        }

        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            base.Update(gameTime);

            inputController.ProcessControls(gameTime);
            entityManager.Update(gameTime);

            playerCount = playerManager.PlayerCount.ToString();
            hoboHeight = player.Position.Y;

            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            timer -= elapsed;
            if(timer < 0)
            {
                connector.Send(player.Position);
                connector.IDs.ForEach(s => Console.WriteLine(s));
                timer = TIMER;
            }

            Random random = new Random();
            // Add required player
            foreach (var id in connector.IDs)
            {
                if (playerManager.players.Find(p => p.ConnectionId == id) == null)
                {
                    OtherPlayer p = new OtherPlayer(hoboTexture,
                        new Vector2(HOBO_START_POSITION_X + random.Next(0, 15), 
                        HOBO_START_POSITION_Y),
                        id, map);
                    playerManager.CreatePlayer(p);
                }
            }

            // Update player positions
            if (connector.coords.Count != 0)
            {
                Coordinate coordinate = connector.coords.First();
                OtherPlayer p = playerManager.players.Find(p => p.ConnectionId == coordinate.ConnectionID);
                if (p != null)
                {
                    p.Position = new Vector2(coordinate.X, coordinate.Y);
                    connector.coords.Remove(coordinate);
                }
            }

            // Remove required player
            foreach (var player in playerManager.players)
            {
                if (!connector.IDs.Contains(player.ConnectionId))
                {
                    playerManager.DeletePlayer(player);
                    break;
                }
            }

        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();
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
            jumpSound = Content.Load<SoundEffect>(ASSET_NAME_SFX_JUMP);
            font = Content.Load<SpriteFont>("Debug");
        }
    }
}
