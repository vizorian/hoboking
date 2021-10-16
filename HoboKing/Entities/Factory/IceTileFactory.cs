
using HoboKing.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HoboKing.Entities.Factory
{
    public class IceTileFactory : AbstractTileFactory
    {
        public override Tile CreateStandard(GraphicsDevice graphics, Texture2D texture, Vector2 position, int tileSize)
        {
            return new IceStandardTile(graphics, texture, position, tileSize);
        }

        public override Tile CreateSlopeLeft(GraphicsDevice graphics, Vector2 position, int tileSize)
        {
            return new IceSlopeLeft(graphics, ContentLoader.IceLeft, position, tileSize);
        }

        public override Tile CreateSlopeRight(GraphicsDevice graphics, Vector2 position, int tileSize)
        {
            return new IceSlopeRight(graphics, ContentLoader.IceRight, position, tileSize);
        }

    }

}
