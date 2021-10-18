using HoboKing.Control;
using HoboKing.Entities;
using HoboKing.Graphics;
using HoboKing.States;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;

namespace HoboKing
{
    public class HoboKingGame : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        // States
        private MenuButtons _menuButtons;
        private GameState _gameState;

        private Thread backgroundThread;
        private bool isLoading = false;
        MouseState mouseState;
        MouseState previousMouseState;

        enum GameState
        {
            MainMenu,
            OptionMenu,
            Loading,
            PlayingSingleplayer,
            PlayingMultiplayer,
            Paused
        }

        public void MouseClicked(int x, int y)
        {
            Rectangle mouseClickRect = new Rectangle(x, y, 10, 10);
            Console.WriteLine($"Mouse clicked on X Y: {x} {y}");
            if (_gameState == GameState.MainMenu)
            {
                Rectangle startSingleplayerButtonRect = new Rectangle((int)_menuButtons.startSingleplayerButtonPosition.X, (int)_menuButtons.startSingleplayerButtonPosition.Y, 600, 60);
                Rectangle startMultiplayerButtonRect = new Rectangle((int)_menuButtons.startMultiplayerButtonPosition.X, (int)_menuButtons.startMultiplayerButtonPosition.Y, 600, 60);
                Rectangle optionsButtonRect = new Rectangle((int)_menuButtons.optionsButtonPosition.X, (int)_menuButtons.optionsButtonPosition.Y, 600, 60);
                Rectangle exitButtonRect = new Rectangle((int)_menuButtons.exitButtonPosition.X, (int)_menuButtons.exitButtonPosition.Y, 600, 60);

                if (mouseClickRect.Intersects(startSingleplayerButtonRect))
                {
                    // _gameState = GameState.Loading;
                    _gameState = GameState.PlayingMultiplayer;
                    isLoading = true;
                }
                else if (mouseClickRect.Intersects(startMultiplayerButtonRect))
                {
                    // _gameState = GameState.Loading;
                    _gameState = GameState.PlayingMultiplayer;
                    isLoading = true;
                }
                else if (mouseClickRect.Intersects(optionsButtonRect))
                {
                    // _gameState = GameState.Loading;
                    _gameState = GameState.OptionMenu;
                }
                else if (mouseClickRect.Intersects(exitButtonRect))
                {
                    Exit();
                }
            }

            if (_gameState == GameState.OptionMenu)
            {
                Rectangle returnButtonRect = new Rectangle((int)_menuButtons.returnButtonPosition.X, (int)_menuButtons.returnButtonPosition.Y, 600, 60);

                if (mouseClickRect.Intersects(returnButtonRect))
                {
                    // _gameState = GameState.Loading;
                    _gameState = GameState.MainMenu;
                }
            }
        }


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
        private bool isMultiplayer = false;

        private Connector connector;

        private float timer = 0.1f;
        const float TIMER = 0.1f;

        private bool showBoundingBox = true;

        public HoboKingGame()
        {
            IsMouseVisible = true;
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            connector = Connector.getInstance();
        }

        protected override void Initialize()
        {
            // State bs
            _gameState = new GameState();
            _menuButtons = new MenuButtons(GraphicsDevice);
            IsMouseVisible = true;

            _gameState = GameState.MainMenu;

            // Get the mouse state
            mouseState = Mouse.GetState();
            previousMouseState = mouseState;

            // Original
            base.Initialize();
            _graphics.PreferredBackBufferWidth = GAME_WINDOW_WIDTH;
            _graphics.PreferredBackBufferHeight = GAME_WINDOW_HEIGHT;
            _graphics.IsFullScreen = false;
            _graphics.ApplyChanges();
        }

        protected void ConnectToServer()
        {
            if (isMultiplayer) connector.Connect();
        }

        protected override void LoadContent()
        {
            if (_gameState == GameState.PlayingMultiplayer) isMultiplayer = true;
            
            if (isMultiplayer) connector.Connect();

            _spriteBatch = new SpriteBatch(GraphicsDevice); 

            map = new Map(_graphics.GraphicsDevice, GAME_WINDOW_WIDTH, GAME_WINDOW_HEIGHT);
            map.ReadMapData();
            map.LoadEntityContent(Content);

            player = map.CreateMainPlayer(connector, isMultiplayer);
            
            map.CreateDebugCritter();

            map.CreateMap();

            map.UpdateTextures();

            if (isMultiplayer) Console.WriteLine($"Main player's connection ID: {connector.GetConnectionId()}");
        }

        protected override void Update(GameTime gameTime)
        {
            // States
            mouseState = Mouse.GetState();
            if (previousMouseState.LeftButton == ButtonState.Pressed && mouseState.LeftButton == ButtonState.Released)
            {
                MouseClicked(mouseState.X, mouseState.Y);
            }
            previousMouseState = mouseState;

            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                _gameState = GameState.MainMenu;

            // check for state loading && playing



            base.Update(gameTime);

            InputController.Update();

            if (InputController.KeyPressed(Keys.F1))
            {
                ToggleBoundingBoxes();
            }

            map.Update(gameTime);

            if (isMultiplayer)
            {
                SendData(gameTime);
                map.AddConnectedPlayers(connector);
                map.UpdateConnectedPlayers(connector);
                map.RemoveConnectedPlayers(connector);
            }
        }

        private void ToggleBoundingBoxes()
        {
            showBoundingBox = !showBoundingBox;
            map.ShowBoundingBoxes(showBoundingBox);
        }

        protected override void Draw(GameTime gameTime)
        {

            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();

            // States
            if (_gameState == GameState.MainMenu)
            {
                _spriteBatch.Draw(ContentLoader.StartSingleplayerButton, _menuButtons.startSingleplayerButtonPosition, Color.White);
                _spriteBatch.Draw(ContentLoader.StartMultiplayerButton, _menuButtons.startMultiplayerButtonPosition, Color.White);
                _spriteBatch.Draw(ContentLoader.OptionsButton, _menuButtons.optionsButtonPosition, Color.White);
                _spriteBatch.Draw(ContentLoader.ExitButton, _menuButtons.exitButtonPosition, Color.White);
            }

            if (_gameState == GameState.OptionMenu)
            {
                _spriteBatch.Draw(ContentLoader.ReturnButton, _menuButtons.returnButtonPosition, Color.White);
            }

            if (_gameState == GameState.PlayingMultiplayer)
            {
                map.DrawEntities(_spriteBatch);
                isLoading = false;
            }
            
            _spriteBatch.End();

            base.Draw(gameTime);
        }

        // Send data every time interval
        private void SendData(GameTime gameTime)
        {
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            timer -= elapsed;
            if (timer < 0)
            {
                connector.SendCoordinates(player.Sprite.Position);
                timer = TIMER;
            }
        }
    }
}
