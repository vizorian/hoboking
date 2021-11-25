using Microsoft.Xna.Framework;

namespace HoboKing.Graphics
{
    public class MenuItem
    {
        public Vector2 Position;
        public float Size;
        public string Text;

        public MenuItem(string text, Vector2 position)
        {
            Text = text;
            Position = position;
            Size = 0.6f;
        }
    }
}