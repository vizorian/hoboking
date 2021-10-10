using HoboKing.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace HoboKing.Entities
{
    class Tile : IGameEntity
    {
        public int TileSize { get; set; }
        public Sprite Sprite { get; set; }

        public Tile(GraphicsDevice graphics, Texture2D texture, int tileSize, Vector2 position)
        {
            TileSize = tileSize;
            Sprite = new Sprite(graphics, texture, position, tileSize);
        }

        public Tile(GraphicsDevice graphics, Texture2D texture, int tileSize)
        {
            TileSize = tileSize;
            Sprite = new Sprite(graphics, texture, new Vector2(0, 0), tileSize);
        }

        public void SetPosition(Vector2 position)
        {
            Sprite.Position = position;
        }

        public void Update(GameTime gameTime)
        {
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Sprite.Draw(spriteBatch, TileSize);
        }
    }
}
