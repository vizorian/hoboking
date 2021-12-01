using Microsoft.Xna.Framework;

namespace HoboKing.Composite
{
    public interface IComponent
    {
        public void Add(IComponent component);
        public void Remove(IComponent component);
        public IComponent GetChild(int i);
        public bool IsComposite();
        public int GetCount();

        public string Text { get; set; }
        public Vector2 Position { get; set; }
        public float Size { get; set; }

        public void Print();
    }
}