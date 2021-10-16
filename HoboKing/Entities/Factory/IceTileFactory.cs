
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HoboKing.Entities.Factory
{
    public class IceTileFactory : AbstractTileFactory
    {
        public override Tile CreateSlopeLeft(GraphicsDevice graphics, Texture2D texture, Vector2 position, int tileSize)
        {
            return new IceSlopeLeft(graphics, texture, position, tileSize);
        }

        public override Tile CreateSlopeRight(GraphicsDevice graphics, Texture2D texture, Vector2 position, int tileSize)
        {
            return new IceSlopeRight(graphics, texture, position, tileSize);
        }

        public override Tile CreateStandard(GraphicsDevice graphics, Texture2D texture, Vector2 position, int tileSize)
        {
            return new IceStandardTile(graphics, texture, position, tileSize);
        }
    }

}
