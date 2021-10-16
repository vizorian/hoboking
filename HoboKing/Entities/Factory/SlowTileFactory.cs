
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HoboKing.Entities.Factory
{
    public class SlowTileFactory : AbstractTileFactory
    {
        public override Tile CreateSlopeLeft(GraphicsDevice graphics, Texture2D texture, Vector2 position, int tileSize)
        {
            return new SlowSlopeLeft(graphics, texture, position, tileSize);
        }

        public override Tile CreateSlopeRight(GraphicsDevice graphics, Texture2D texture, Vector2 position, int tileSize)
        {
            return new SlowSlopeRight(graphics, texture, position, tileSize);
        }

        public override Tile CreateStandard(GraphicsDevice graphics, Texture2D texture, Vector2 position, int tileSize)
        {
            return new SlowStandardTile(graphics, texture, position, tileSize);
        }
    }

}
