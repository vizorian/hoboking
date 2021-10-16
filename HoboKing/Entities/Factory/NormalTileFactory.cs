using HoboKing.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HoboKing.Entities.Factory
{
	public class NormalTileFactory : AbstractTileFactory
	{
		public override Tile CreateSlopeLeft(GraphicsDevice graphics, Texture2D texture, Vector2 position, int tileSize)
        {
			return new NormalSlopeLeft(graphics, texture, position, tileSize);
        }

		public override Tile CreateStandard(GraphicsDevice graphics, Texture2D texture, Vector2 position, int tileSize)
		{
			return new NormalStandardTile(graphics, texture, position, tileSize);
		}

		public override Tile CreateSlopeRight(GraphicsDevice graphics, Texture2D texture, Vector2 position, int tileSize)
		{
			return new NormalSlopeRight(graphics, texture, position, tileSize);
		}
	}
	
}
