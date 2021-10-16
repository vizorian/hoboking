using HoboKing.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HoboKing.Entities.Factory
{
	public abstract class SlopeLeft : Tile
	{
		public SlopeLeft(GraphicsDevice graphics, Texture2D texture, Vector2 position, int tileSize) : base(graphics, texture, position, tileSize)
		{
		}
	}
	
}
