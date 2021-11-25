using System.Linq;
using HoboKing.Components;
using HoboKing.Control;
using HoboKing.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HoboKing
{
    internal class HoboKingGame : Game
    {
        public enum GameState
        {
            Singleplayer,
            Multiplayer,
            Options,
            Menu,
            Initialising,
            Loading,
            Unloading
        }

        // The app window size
        public const int WINDOW_WIDTH = 1920;
        public const int WINDOW_HEIGHT = 1080;

        // The game window size inside the app (sides are black bars)
        public const int GAME_WINDOW_WIDTH = 1280;

        // should be 1080, reduced for fitting in screen
        public const int GAME_WINDOW_HEIGHT = 1000;

        public GameState GState;
        public GraphicsDeviceManager Graphics;

        public GameScene MenuScene, OptionsScene, SingleplayerScene, MultiplayerScene;
        public MapComponent MultiplayerGame;
        public MapComponent SingleplayerGame;
        public SpriteBatch SpriteBatch;

        public HoboKingGame()
        {
            GState = GameState.Initialising;
            Graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        // Changes component state
        private void ChangeComponentState(GameComponent component, bool state)
        {
            component.Enabled = state;
            if (component is DrawableGameComponent gameComponent)
                gameComponent.Visible = state;
        }

        /// Switches game scene
        public void SwitchScene(GameScene scene)
        {
            var usedComponents = scene.ReturnComponents();
            foreach (var gameComponent in Components)
            {
                var component = (GameComponent) gameComponent;
                var isUsed = usedComponents.Contains(component);
                ChangeComponentState(component, isUsed);
            }

            InputController.PreviousKeyboardState = InputController.KeyboardState;
        }

        protected override void Initialize()
        {
            var menuPosition = new Vector2(GraphicsDevice.Viewport.Width / 4, 600);

            // Creating components
            // Creating connector component
            var connector = new ConnectorComponent();

            // Creating map component
            SingleplayerGame = new MapComponent(this);
            MultiplayerGame = new MapComponent(this, connector);

            // Creating MAIN MENU items and components
            var mainMenuItems = new MenuItemsComponent(this, menuPosition, Color.White, Color.Green, 1);
            mainMenuItems.AddMenuItem("Start Singleplayer");
            mainMenuItems.AddMenuItem("Start Multiplayer");
            mainMenuItems.AddMenuItem("Options");
            mainMenuItems.AddMenuItem("Exit Game");
            var mainMenu = new MenuComponent(this, mainMenuItems);

            // Creating OPTIONS MENU items and components
            var optionsMenuItems = new MenuItemsComponent(this, menuPosition, Color.White, Color.Green, 1);
            optionsMenuItems.AddMenuItem("Return");
            var optionsMenu = new MenuComponent(this, optionsMenuItems);

            // Game scenes
            MenuScene = new GameScene(this, mainMenu, mainMenuItems);
            OptionsScene = new GameScene(this, optionsMenu, optionsMenuItems);

            //singleplayerScene = new GameScene(this, singleplayerGame);
            MultiplayerScene = new GameScene(this, MultiplayerGame);

            // Disabling components
            foreach (var gameComponent in Components)
            {
                var component = (GameComponent) gameComponent;
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
            SwitchScene(MenuScene);
            GState = GameState.Menu;
        }

        protected override void Update(GameTime gameTime)
        {
            //Console.WriteLine($"Current state is: {GState}");

            if (GState == GameState.Unloading)
                // Doesn't work
                //singleplayerScene.ReturnComponents().ToList().ForEach(o => o.Dispose());
                //singleplayerScene.ReturnComponents().ToList().ForEach(o => o.Initialize());
                //multiplayerScene.ReturnComponents().ToList().ForEach(o => o.Dispose());

                GState = GameState.Menu;
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