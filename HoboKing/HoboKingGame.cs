using HoboKing.Components;
using HoboKing.Control;
using HoboKing.Entities;
using HoboKing.Graphics;
using HoboKing.Scenes;
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
        public GraphicsDeviceManager Graphics;
        public SpriteBatch SpriteBatch;

        // The app window size
        public const int WINDOW_WIDTH = 1920;
        public const int WINDOW_HEIGHT = 1080;

        // The game window size inside the app (sides are black bars)
        public const int GAME_WINDOW_WIDTH = 1280;

        // should be 1080, reduced for fitting in screen
        public const int GAME_WINDOW_HEIGHT = 1000;

        public GameScene menuScene, optionsScene, singleplayerScene, multiplayerScene;

        public enum GameState
        {
            Singleplayer,
            Multiplayer,
            Options,
            Menu,
            Initialising,
            Loading,
            Unloading,
        }

        public GameState gameState;

        public HoboKingGame()
        {
            gameState = GameState.Initialising;
            Graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        // Changes component state
        private void ChangeComponentState(GameComponent component, bool state)
        {
            component.Enabled = state;
            if (component is DrawableGameComponent)
                ((DrawableGameComponent)component).Visible = state;
        }

        /// Switches game scene
        public void SwitchScene(GameScene scene)
        {
            GameComponent[] usedComponents = scene.ReturnComponents();
            foreach (GameComponent component in Components)
            {
                bool isUsed = usedComponents.Contains(component);
                ChangeComponentState(component, isUsed);
            }
            InputController.PreviousKeyboardState = InputController.KeyboardState;
        }

        protected override void Initialize()
        {
            Vector2 MenuPosition = new Vector2(GraphicsDevice.Viewport.Width / 4, 600);

            // Creating components
            // Creating connector component
            ConnectorComponent connector = new ConnectorComponent(this);

            // Creating map component
            MapComponent singleplayerGame = new MapComponent(this);
            MapComponent multiplayerGame = new MapComponent(this, connector);

            // Creating MAIN MENU items and components
            MenuItemsComponent mainMenuItems = new MenuItemsComponent(this, MenuPosition, Color.White, Color.Green, 1);
            mainMenuItems.AddMenuItem("Start Singleplayer");
            mainMenuItems.AddMenuItem("Start Multiplayer");
            mainMenuItems.AddMenuItem("Options");
            mainMenuItems.AddMenuItem("Exit Game");
            MenuComponent mainMenu = new MenuComponent(this, mainMenuItems);

            // Creating OPTIONS MENU items and components
            MenuItemsComponent optionsMenuItems = new MenuItemsComponent(this, MenuPosition, Color.White, Color.Green, 1);
            optionsMenuItems.AddMenuItem("Return");
            MenuComponent optionsMenu = new MenuComponent(this, optionsMenuItems);

            // Game scenes
            menuScene = new GameScene(this, mainMenu, mainMenuItems);
            optionsScene = new GameScene(this, optionsMenu, optionsMenuItems);
            singleplayerScene = new GameScene(this, singleplayerGame);
            multiplayerScene = new GameScene(this, multiplayerGame, connector);

            // Disabling components
            foreach (GameComponent component in Components)
            {
                ChangeComponentState(component, false);
            }

            Graphics.PreferredBackBufferWidth = GAME_WINDOW_WIDTH;
            Graphics.PreferredBackBufferHeight = GAME_WINDOW_HEIGHT;
            Graphics.IsFullScreen = false;
            Graphics.ApplyChanges();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            SpriteBatch = new SpriteBatch(GraphicsDevice);
            SwitchScene(menuScene);
            gameState = GameState.Menu;
        }

        protected override void Update(GameTime gameTime)
        {
            Console.WriteLine($"Current state is: {gameState}");

            if(gameState == GameState.Unloading)
            {
                // Doesn't work
                // singleplayerScene.ReturnComponents().ToList().ForEach(o => o.Dispose());
                // multiplayerScene.ReturnComponents().ToList().ForEach(o => o.Dispose());
                gameState = GameState.Menu;
            }
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
