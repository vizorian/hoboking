using System.Linq;
using HoboKing.Components;
using HoboKing.Control;
using HoboKing.Graphics;
using HoboKing.State;
using HoboKing.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HoboKing
{
    internal class HoboKingGame : Game
    {
        public const int GAME_WINDOW_WIDTH = 1280;
        public const int GAME_WINDOW_HEIGHT = 1000;

        public GameState gameState;
        private readonly GraphicsDeviceManager graphics;
        public SpriteBatch SpriteBatch;
        private GameState currentGame;
        private GameComponent PlayingState;
        public GameComponent MenuState;

        public HoboKingGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// Switches game scene
        public void ChangeStateAndDestroy(GameState newState)
        {
            gameState.Destroy();
            gameState = newState;
        }

        /// Switches game scene
        public void ChangeState(GameState newState)
        {
            gameState = newState;
        }

        protected override void Initialize()
        {
            graphics.PreferredBackBufferWidth = GAME_WINDOW_WIDTH;
            graphics.PreferredBackBufferHeight = GAME_WINDOW_HEIGHT;
            graphics.IsFullScreen = false;
            graphics.ApplyChanges();
            base.Initialize();
        }

        protected override void LoadContent()
        {
            SpriteBatch = new SpriteBatch(GraphicsDevice);
            gameState = new Menu(this, GraphicsDevice);
            ContentLoader.LoadContent(Content);
        }

        protected override void Update(GameTime gameTime)
        {
            InputController.Update();
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            base.Draw(gameTime);
        }
    }
}