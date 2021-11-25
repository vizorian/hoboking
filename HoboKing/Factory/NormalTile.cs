using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using tainicom.Aether.Physics2D.Dynamics;

namespace HoboKing.Factory
{
    public class NormalTile : Tile
    {
        public NormalTile(Texture2D texture, Vector2 position, int tileSize, World world) : base(texture, position,
            tileSize, world)
        {
        }
    }
}