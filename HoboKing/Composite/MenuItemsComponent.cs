using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using HoboKing.Control;
using HoboKing.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace HoboKing.Composite
{
    // Composite
    public class MenuItemsComponent : IComponent
    {
        

        public string Text { get; set; }
        public Vector2 Position { get; set; }
        public float Size { get; set; }

        private readonly List<IComponent> menuItems;
        public IComponent SelectedMenuItem;

        public MenuItemsComponent(string text, Vector2 position, float size)
        {
            Text = text;
            Position = position;
            Size = size;
            menuItems = new List<IComponent>();
            SelectedMenuItem = null;
        }

        public void Add(IComponent component)
        {
            menuItems.Add(component);

            // Selecting the first item
            SelectedMenuItem ??= component;
        }

        public void Remove(IComponent component)
        {
            menuItems.Remove(component);
        }

        public void Next()
        {
            var index = menuItems.IndexOf(SelectedMenuItem);
            SelectedMenuItem = index < menuItems.Count - 1 ? menuItems[index + 1] : menuItems[0];
        }

        public void Previous()
        {
            var index = menuItems.IndexOf(SelectedMenuItem);
            SelectedMenuItem = index > 0 ? menuItems[index - 1] : menuItems[^1];
        }

        public IComponent GetChild(int i)
        {
            return menuItems[i];
        }

        public bool IsComposite()
        {
            return true;
        }

        public int GetCount()
        {
            return menuItems.Count;
        }

        public void Print()
        {
            Console.WriteLine($"{Text}");
            Console.WriteLine("-------------------------");

            foreach (var component in menuItems)
            {
                component.Print();
            }
        }

        //private readonly HoboKingGame hoboKingGame;
        //private readonly Color menuItemColor;

        //private readonly List<IComponent> menuItems;

        //private readonly Vector2 Position;
        //private readonly Color selectedMenuItemColor;
        //private readonly int textScale;
        //public IComponent SelectedMenuItem;

        //public MenuItemsComponent(HoboKingGame hoboKingGame, Vector2 position, Color menuItemColor,
        //    Color selectedMenuItemColor, int textScale)
        //    : base(hoboKingGame)
        //{
        //    this.hoboKingGame = hoboKingGame;
        //    this.Position = position;
        //    this.menuItemColor = menuItemColor;
        //    this.selectedMenuItemColor = selectedMenuItemColor;
        //    this.textScale = textScale;

        //    menuItems = new List<IComponent>();
        //    SelectedMenuItem = null;
        //}

        //public void AddItem(string text)
        //{
        //    // Setting up the position according to the item collection index
        //    var p = new Vector2(Position.X, Position.Y + menuItems.Count * textScale * 70);
        //    var item = new MenuItem(text, p);
        //    menuItems.Add(item);

        //    // Selecting the first item
        //    SelectedMenuItem ??= item;
        //}

        //public void SelectNext()
        //{
        //    var index = menuItems.IndexOf(SelectedMenuItem);
        //    if (index < menuItems.Count - 1)
        //        SelectedMenuItem = menuItems[index + 1] as MenuItem;
        //    else
        //        SelectedMenuItem = menuItems[0] as MenuItem;
        //}

        //public void SelectPrevious()
        //{
        //    var index = menuItems.IndexOf(SelectedMenuItem);
        //    if (index > 0)
        //        SelectedMenuItem = menuItems[index - 1] as MenuItem;
        //    else
        //        SelectedMenuItem = menuItems[^1] as MenuItem;
        //}

        //public override void Update(GameTime gameTime)
        //{
        //    if (InputController.KeyPressed(Keys.Up))
        //        SelectPrevious();
        //    if (InputController.KeyPressed(Keys.Down))
        //        SelectNext();
        //    base.Update(gameTime);
        //}

        //public override void Draw(GameTime gameTime)
        //{
        //    hoboKingGame.SpriteBatch.Begin();

        //    foreach (var menuItem in menuItems)
        //    {
        //        var color = menuItemColor;
        //        if (menuItem == SelectedMenuItem)
        //            color = selectedMenuItemColor;

        //        hoboKingGame.SpriteBatch.DrawString(ContentLoader.MenuFont, menuItem., menuItem.GetPosition(), color,
        //            0.0f, new Vector2(0, 0), textScale, SpriteEffects.None, 0);
        //    }

        //    hoboKingGame.SpriteBatch.End();
        //    base.Draw(gameTime);
        //}

    }
}