using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using tainicom.Aether.Physics2D.Dynamics;

namespace HoboKing.Factory
{
    class SpikeTile : Tile
    {
        public SpikeTile(Texture2D texture, Vector2 position, int tileSize, World world) : base(texture, position, tileSize, world)
        {
        }
    }
}
