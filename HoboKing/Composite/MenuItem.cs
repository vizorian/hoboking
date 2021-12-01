using System;
using Microsoft.Xna.Framework;

namespace HoboKing.Composite
{
    public class MenuItem : IComponent
    {
        public string Text { get; set; }
        public Vector2 Position { get; set; }
        public float Size { get; set; }

        public MenuItem(string text, Vector2 position, float size)
        {
            Text = text;
            Position = position;
            Size = size;
        }

        public void Add(IComponent component){}

        public void Remove(IComponent component){}

        public IComponent GetChild(int i)
        {
            return null;
        }

        public bool IsComposite()
        {
            return false;
        }

        public int GetCount()
        {
            return 1;
        }

        public void Print()
        {
            Console.WriteLine($"\t{Text}");
        }
    }
}