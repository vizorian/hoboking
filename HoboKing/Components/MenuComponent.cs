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
        private MenuItemsComponent oldMenu;
        private MenuItemsComponent menuItems;

        public MenuComponent(HoboKingGame hoboKingGame, MenuItemsComponent menuItems) : base(hoboKingGame)
        {
            this.hoboKingGame = hoboKingGame;
            this.menuItems = menuItems;
            oldMenu = null;
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
                        switch (oldMenu)
                        {
                            case null:
                                hoboKingGame.ChangeStateAndDestroy(new Playing(hoboKingGame));
                                break;
                            default:
                                menuItems = oldMenu;
                                oldMenu = null;
                                break;
                        }
                        break;
                    case "Options":
                        oldMenu = menuItems;
                        menuItems = menuItems.SelectedMenuItem as MenuItemsComponent;
                        break;
                    case "Exit Game":
                        hoboKingGame.Exit();
                        break;
                    case "Exit To Menu":
                        hoboKingGame.ChangeStateAndDestroy(new Menu(hoboKingGame, hoboKingGame.GraphicsDevice));
                        break;
                }
            if (InputController.KeyPressed(Keys.Down))
                menuItems.Next();
            if (InputController.KeyPressed(Keys.Up))
                menuItems.Previous();
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            hoboKingGame.SpriteBatch.Begin();

            var color = Color.White;
            hoboKingGame.SpriteBatch.DrawString(ContentLoader.MenuFont, menuItems.Text, new Vector2(menuItems.GetChild(0).Position.X, menuItems.GetChild(0).Position.Y - 70), color);

            for (int i = 0; i < menuItems.GetCount(); i++)
            {
                color = Color.White;
                var item = menuItems.GetChild(i);
                if (item == menuItems.SelectedMenuItem)
                    color = Color.Green;

                hoboKingGame.SpriteBatch.DrawString(ContentLoader.MenuFont, item.Text, item.Position, color);
            }
            hoboKingGame.SpriteBatch.End();
            base.Draw(gameTime);
        }
    }
}