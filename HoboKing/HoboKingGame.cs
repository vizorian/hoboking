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

        // approximate size to get a 1280x1080 game with side black bars
        public const int TILE_SIZE = 20;

        private Map map;
        private Player player;

        private InputController inputController;
        private Connector connector;

        private float timer = 0.1f;
        const float TIMER = 0.1f;

        public HoboKingGame()
        {
            IsMouseVisible = true;
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            connector = Connector.getInstance();
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

            map = new Map(GAME_WINDOW_WIDTH, GAME_WINDOW_HEIGHT, TILE_SIZE, TILE_SIZE);
            map.LoadEntityContent(Content);

            player = map.CreateMainPlayer(connector);

            Console.WriteLine($"Main player's connection ID: {connector.GetConnectionId()}");

            inputController = new InputController(player);
        }

        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            base.Update(gameTime);

            inputController.ProcessControls(gameTime);
            map.Update(gameTime);
            SendData(gameTime);

            map.AddConnectedPlayers(connector);
            map.UpdateConnectedPlayers(connector);
            map.RemoveConnectedPlayers(connector);
        }

        // Send data every time interval
        private void SendData(GameTime gameTime)
        {
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            timer -= elapsed;
            if (timer < 0)
            {
                connector.SendCoordinates(player.Position);
                timer = TIMER;
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();
            map.Draw(gameTime, _spriteBatch);

            // Was EntityManager.Draw go fix :)
            map.DrawEntities(gameTime, _spriteBatch);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
