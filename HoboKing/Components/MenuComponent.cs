using System.Diagnostics.CodeAnalysis;
using HoboKing.Composite;
using HoboKing.Control;
using HoboKing.Graphics;
using HoboKing.State;
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
                switch (menuItems.SelectedMenuItem.Text){
                    case "Start":
                        hoboKingGame.ChangeStateAndDestroy(new Playing(hoboKingGame));
                        break;
                    case "Return":
                        hoboKingGame.ChangeStateAndDestroy(new Playing(hoboKingGame));
                        break;
                    case "Options":
                        //hoboKingGame.ChangeStateAndDestroy(new Options(hoboKingGame));
                        break;
                    case "Exit Game":
                        hoboKingGame.Exit();
                        break;
                }
            if(InputController.KeyPressed(Keys.Down))
                menuItems.Next();
            if (InputController.KeyPressed(Keys.Up))
                menuItems.Previous();
        }

        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            hoboKingGame.SpriteBatch.Begin();
            for (int i = 0; i < menuItems.GetCount(); i++)
            {
                var item = menuItems.GetChild(i);
                var color = Color.White;
                if (item == menuItems.SelectedMenuItem)
                    color = Color.Green;

                hoboKingGame.SpriteBatch.DrawString(ContentLoader.MenuFont, item.Text, item.Position, color);
            }
            hoboKingGame.SpriteBatch.End();
            base.Draw(gameTime);
        }
    }
}