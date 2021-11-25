using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using tainicom.Aether.Physics2D.Dynamics;

namespace HoboKing.Factory
{
    internal class FakeTile : Tile
    {
        public FakeTile(Texture2D texture, Vector2 position, int tileSize, World world) : base(texture, position,
            tileSize, world)
        {
            var pos = new Vector2(0, 0);
            Body.Position = pos * 0;
        }

        public override void ChangePosition(Vector2 newPosition)
        {
            Position = newPosition;
            sizeRectangle = tileSize != 0
                ? new Rectangle((int) Position.X, (int) Position.Y, tileSize, tileSize)
                : new Rectangle((int) Position.X, (int) Position.Y, Texture.Width, Texture.Height);
        }
    }
}