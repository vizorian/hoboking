using HoboKing.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace HoboKing.Entities
{
    public abstract class Tile : IGameEntity
    {
        public int TileSize { get; set; }
        public Sprite Sprite { get; set; }

        public Tile(GraphicsDevice graphics, Texture2D texture, Vector2 position, int tileSize)
        {
            TileSize = tileSize;
            Sprite = new Sprite(graphics, texture, position, tileSize);
        }

        public void Update(GameTime gameTime)
        {
            Sprite.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Sprite.Draw(spriteBatch);
        }
    }
}
