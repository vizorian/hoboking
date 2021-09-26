using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace HoboKing.Entities
{
    class Tile : IGameEntity
    {
        public Vector2 Position { get; set; }
        public Texture2D Texture { get; set; }
        public int TileSize { get; set; }
        public int DrawOrder { get; set; }
        public const int TILE_SIZE = 50;
        public Tile(Texture2D texture, int tileSize, Vector2 position)
        {
            TileSize = tileSize;
            Position = position;
            Texture = texture;
        }

        public Tile(Texture2D texture, int tileSize)
        {
            Position = new Vector2(0, 0);
            TileSize = tileSize;
            Texture = texture;
        }

        public void Update(GameTime gameTime)
        {
        }

        public void Draw(SpriteBatch spriteBatch, GameTime gameTime)
        {
            spriteBatch.Draw(Texture, new Rectangle((int)Position.X, (int)Position.Y, TileSize, TileSize), Color.White);
        }


    }
}
