
using HoboKing.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HoboKing.Entities.Factory
{
    public class SlowTileFactory : AbstractTileFactory
    {
        public override Tile CreateStandard(GraphicsDevice graphics, Texture2D texture, Vector2 position, int tileSize)
        {
            return new SlowStandardTile(graphics, texture, position, tileSize);
        }

        public override Tile CreateSlopeLeft(GraphicsDevice graphics, Vector2 position, int tileSize)
        {
            return new SlowSlopeLeft(graphics, ContentLoader.SandLeft, position, tileSize);
        }

        public override Tile CreateSlopeRight(GraphicsDevice graphics, Vector2 position, int tileSize)
        {
            return new SlowSlopeRight(graphics, ContentLoader.SandRight, position, tileSize);
        }
    }

}
