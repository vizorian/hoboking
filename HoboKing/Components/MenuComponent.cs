using HoboKing.Components;
using HoboKing.Control;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace HoboKing.States
{
    class MenuComponent : DrawableGameComponent
    {
        private HoboKingGame hoboKingGame;
        private MenuItemsComponent menuItems;

        public MenuComponent(HoboKingGame hoboKingGame, MenuItemsComponent menuItems) : base(hoboKingGame)
        {
            this.hoboKingGame = hoboKingGame;
            this.menuItems = menuItems;
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            base.LoadContent();
        }

        [ExcludeFromCodeCoverage]
        public override void Update(GameTime gameTime)
        {
            if (InputController.KeyPressed(Keys.Enter))
                switch (menuItems.selectedMenuItem.text)
                {
                    case "Start Singleplayer":
                        hoboKingGame.SwitchScene(hoboKingGame.singleplayerScene);
                        hoboKingGame.gameState = HoboKingGame.GameState.Singleplayer;
                        break;
                    case "Start Multiplayer":
                        hoboKingGame.SwitchScene(hoboKingGame.multiplayerScene);
                        break;
                    case "Options":
                        hoboKingGame.SwitchScene(hoboKingGame.optionsScene);
                        break;
                    case "Return":
                        hoboKingGame.SwitchScene(hoboKingGame.menuScene);
                        break;
                    case "Exit Game":
                        hoboKingGame.Exit();
                        break;
                    default:
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
