using System.Collections.Generic;
using HoboKing.Control;
using HoboKing.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace HoboKing.Components
{
    internal class MenuItemsComponent : DrawableGameComponent
    {
        private readonly HoboKingGame hoboKingGame;
        private readonly Color menuItemColor;

        private readonly List<MenuItem> menuItems;

        private readonly Vector2 position;
        private readonly Color selectedMenuItemColor;
        private readonly int textScale;
        public MenuItem SelectedMenuItem;

        public MenuItemsComponent(HoboKingGame hoboKingGame, Vector2 position, Color menuItemColor,
            Color selectedMenuItemColor, int textScale)
            : base(hoboKingGame)
        {
            this.hoboKingGame = hoboKingGame;
            this.position = position;
            this.menuItemColor = menuItemColor;
            this.selectedMenuItemColor = selectedMenuItemColor;
            this.textScale = textScale;

            menuItems = new List<MenuItem>();
            SelectedMenuItem = null;
        }

        public void AddMenuItem(string text)
        {
            // Setting up the position according to the item collection index
            var p = new Vector2(position.X, position.Y + menuItems.Count * textScale * 70);
            var item = new MenuItem(text, p);
            menuItems.Add(item);

            // Selecting the first item
            SelectedMenuItem ??= item;
        }

        public void SelectNext()
        {
            var index = menuItems.IndexOf(SelectedMenuItem);
            if (index < menuItems.Count - 1)
                SelectedMenuItem = menuItems[index + 1];
            else
                SelectedMenuItem = menuItems[0];
        }

        public void SelectPrevious()
        {
            var index = menuItems.IndexOf(SelectedMenuItem);
            if (index > 0)
                SelectedMenuItem = menuItems[index - 1];
            else
                SelectedMenuItem = menuItems[^1];
        }

        public override void Update(GameTime gameTime)
        {
            if (InputController.KeyPressed(Keys.Up))
                SelectPrevious();
            if (InputController.KeyPressed(Keys.Down))
                SelectNext();
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            hoboKingGame.SpriteBatch.Begin();

            foreach (var menuItem in menuItems)
            {
                var color = menuItemColor;
                // Broken Highlighter
                if (menuItem == SelectedMenuItem)
                    //if (!menuItem.text.StartsWith(">"))
                    //    menuItem.text = ">" + menuItem.text;
                    color = selectedMenuItemColor;
                //else
                //{
                //if (menuItem.text.StartsWith(">"))
                //menuItem.text = menuItem.text[1..];
                //}
                hoboKingGame.SpriteBatch.DrawString(ContentLoader.MenuFont, menuItem.Text, menuItem.Position, color,
                    0.0f, new Vector2(0, 0), textScale, SpriteEffects.None, 0);
            }

            hoboKingGame.SpriteBatch.End();
            base.Draw(gameTime);
        }
    }
}