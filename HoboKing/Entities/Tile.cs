using HoboKing.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using tainicom.Aether.Physics2D.Dynamics;

namespace HoboKing.Entities
{
    public abstract class Tile : IGameEntity
    {
        public int TileSize { get; set; }
        public Sprite Sprite { get; set; }

        public Tile(Texture2D texture, Vector2 position, int tileSize, World world)
        {
            TileSize = tileSize;
            Sprite = new Sprite(texture, position, world, tileSize);
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
