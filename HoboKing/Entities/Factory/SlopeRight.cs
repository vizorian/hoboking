using HoboKing.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HoboKing.Entities.Factory
{
	public abstract class SlopeRight : Tile
	{
		public SlopeRight(GraphicsDevice graphics, Texture2D texture, Vector2 position, int tileSize) : base(graphics, texture, position, tileSize)
		{
		}
	}
	
}
