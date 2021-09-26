using HoboKing.Control;
using HoboKing.Entities;
using HoboKing.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Diagnostics;

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

        private const string ASSET_NAME_HOBO = "batchest";
        private const string ASSET_NAME_TILE = "ground";
        private const string ASSET_NAME_SFX_JUMP = "jump";
        private SpriteFont font;
        private string hoboState;
        private double hoboHeight;

        private Texture2D _tileTexture;
        private Texture2D _hoboTexture;
        private SoundEffect _jumpSound;

        private Hobo _hobo;
        private InputController inputController;

        private Map _map;

        private EntityManager entityManager;

        private Connector connector;

        float timer = 1.0f;
        const float TIMER = 1.0f;
        public HoboKingGame()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            entityManager = new EntityManager();
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

            _hoboTexture = Content.Load<Texture2D>(ASSET_NAME_HOBO);
            _tileTexture = Content.Load<Texture2D>(ASSET_NAME_TILE);
            _jumpSound = Content.Load<SoundEffect>(ASSET_NAME_SFX_JUMP);
            font = Content.Load<SpriteFont>("Debug");

            _map = new Map(WINDOW_WIDTH, WINDOW_HEIGHT, 30, 30);
            _map.AddTileType('#', _tileTexture);

            _hobo = new Hobo(_hoboTexture, new Vector2(HOBO_START_POSITION_X, HOBO_START_POSITION_Y), _jumpSound, _map);

            inputController = new InputController(_hobo);
            entityManager.AddEntity(_map);
            entityManager.AddEntity(_hobo);

        }

        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            base.Update(gameTime);

            inputController.ProcessControls(gameTime);
            entityManager.Update(gameTime);
            hoboState = _hobo.State.ToString();
            hoboHeight = _hobo.Position.Y;

            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            timer -= elapsed;
            if(timer < 0)
            {
                connector.Send(_hobo.Position);
                timer = TIMER;
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();
            entityManager.Draw(gameTime, _spriteBatch);
            _spriteBatch.DrawString(font, hoboState, new Vector2(450, 10), Color.Black);
            _spriteBatch.DrawString(font, "Y:" + Math.Round(hoboHeight, 2), new Vector2(450, 30), Color.Black);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
