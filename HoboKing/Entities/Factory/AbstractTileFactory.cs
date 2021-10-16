using HoboKing.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HoboKing.Entities.Factory
{
	public abstract class AbstractTileFactory
	{
		public abstract Tile CreateSlopeLeft(GraphicsDevice graphics, Vector2 position, int tileSize);

		public abstract Tile CreateStandard(GraphicsDevice graphics, Texture2D texture, Vector2 position, int tileSize);

		public abstract Tile CreateSlopeRight(GraphicsDevice graphics, Vector2 position, int tileSize);
		
	}
	
}
