using HoboKing.Control;
using HoboKing.Entities;
using HoboKing.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace HoboKing.Components
{
    class MenuItemsComponent : Microsoft.Xna.Framework.DrawableGameComponent
    {
        private HoboKingGame hoboKingGame;

        private List<MenuItem> menuItems;
        public MenuItem selectedMenuItem;
        
        private Vector2 position;
        private Color menuItemColor;
        private Color selectedMenuItemColor;
        private int textScale;

        public MenuItemsComponent(HoboKingGame hoboKingGame, Vector2 position, Color menuItemColor, Color selectedMenuItemColor, int textScale)
            : base(hoboKingGame)
        {
            this.hoboKingGame = hoboKingGame;
            this.position = position;
            this.menuItemColor = menuItemColor;
            this.selectedMenuItemColor = selectedMenuItemColor;
            this.textScale = textScale;

            menuItems = new List<MenuItem>();
            selectedMenuItem = null;
        }

        public void AddMenuItem(string text)
        {
            // Setting up the position according to the item collection index
            Vector2 p = new Vector2(position.X, position.Y + menuItems.Count * textScale * 70);
            MenuItem item = new MenuItem(text, p);
            menuItems.Add(item);

            // Selecting the first item
            if (selectedMenuItem == null)
                selectedMenuItem = item;
        }

        public void SelectNext()
        {
            int index = menuItems.IndexOf(selectedMenuItem);
            if (index < menuItems.Count - 1)
                selectedMenuItem = menuItems[index + 1];
            else
                selectedMenuItem = menuItems[0];
        }

        public void SelectPrevious()
        {
            int index = menuItems.IndexOf(selectedMenuItem);
            if (index > 0)
                selectedMenuItem = menuItems[index - 1];
            else
                selectedMenuItem = menuItems[menuItems.Count - 1];
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            base.LoadContent();
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

            foreach (MenuItem menuItem in menuItems)
            {
                Color color = menuItemColor;
                // Broken Highlighter
                if (menuItem == selectedMenuItem)
                {
                    //if (!menuItem.text.StartsWith(">"))
                    //    menuItem.text = ">" + menuItem.text;
                    color = selectedMenuItemColor;
                }
                //else
                //{
                    //if (menuItem.text.StartsWith(">"))
                        //menuItem.text = menuItem.text[1..];
                //}
                hoboKingGame.SpriteBatch.DrawString(ContentLoader.MenuFont, menuItem.text, menuItem.position, color, 0.0f, new Vector2(0, 0), textScale, SpriteEffects.None, 0);
            }

            hoboKingGame.SpriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
