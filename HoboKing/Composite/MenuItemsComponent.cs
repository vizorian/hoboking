using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

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
    }
}