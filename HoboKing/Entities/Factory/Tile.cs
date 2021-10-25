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

        private World world;

        public Tile(Texture2D texture, Vector2 position, int tileSize, World world)
        {
            TileSize = tileSize;
            Sprite = new Sprite(texture, position, world, tileSize);

            this.world = world;
        }

        public void Update(GameTime gameTime)
        {
            Sprite.Update();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Sprite.Draw(spriteBatch);
        }

        public IGameEntity ShallowCopy()
        {
            return MemberwiseClone() as Tile;
        }

        public IGameEntity DeepCopy()
        {
            var clone = MemberwiseClone() as Tile;
            clone.Sprite = new Sprite(Sprite.Texture, Sprite.Position, world, TileSize);
            return clone;
        }
    }
}
