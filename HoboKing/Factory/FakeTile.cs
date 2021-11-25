using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using tainicom.Aether.Physics2D.Dynamics;

namespace HoboKing.Factory
{
    class FakeTile : Tile
    {
        public FakeTile(Texture2D texture, Vector2 position, int tileSize, World world) : base(texture, position, tileSize, world)
        {
            Vector2 pos = new Vector2(0, 0);
            Body.Position = pos * 0;
        }

        public override void ChangePosition(Vector2 newPosition)
        {
            Position = newPosition;
            sizeRectangle = tileSize != 0 ? new Rectangle((int)Position.X, (int)Position.Y, tileSize, tileSize) : new Rectangle((int)Position.X, (int)Position.Y, Texture.Width, Texture.Height);       
        }
    }
}
