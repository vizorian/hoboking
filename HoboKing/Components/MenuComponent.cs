using System.Diagnostics.CodeAnalysis;
using HoboKing.Control;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace HoboKing.Components
{
    internal class MenuComponent : DrawableGameComponent
    {
        private readonly HoboKingGame hoboKingGame;
        private readonly MenuItemsComponent menuItems;

        public MenuComponent(HoboKingGame hoboKingGame, MenuItemsComponent menuItems) : base(hoboKingGame)
        {
            this.hoboKingGame = hoboKingGame;
            this.menuItems = menuItems;
        }

        [ExcludeFromCodeCoverage]
        public override void Update(GameTime gameTime)
        {
            if (InputController.KeyPressed(Keys.Enter))
                switch (menuItems.SelectedMenuItem.Text)
                {
                    case "Start Singleplayer":
                        hoboKingGame.SwitchScene(hoboKingGame.SingleplayerScene);
                        hoboKingGame.GState = HoboKingGame.GameState.Singleplayer;
                        break;
                    case "Start Multiplayer":
                        hoboKingGame.SwitchScene(hoboKingGame.MultiplayerScene);
                        break;
                    case "Options":
                        hoboKingGame.SwitchScene(hoboKingGame.OptionsScene);
                        break;
                    case "Return":
                        hoboKingGame.SwitchScene(hoboKingGame.MenuScene);
                        break;
                    case "Exit Game":
                        hoboKingGame.Exit();
                        break;
                }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            base.Draw(gameTime);
        }
    }
}