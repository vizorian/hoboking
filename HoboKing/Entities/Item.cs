using HoboKing.Control;
using HoboKing.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace HoboKing.Entities
{
    class Item : IGameEntity
    {
        public Sprite Sprite { get; set; }
        private Movement movement;

        //public Critter(GraphicsDevice graphics, Texture2D texture, Vector2 position)
        //{
        //    Sprite = new Sprite(graphics, texture, position, 40);

        //    // Recalculates tiles to absolute coordinates
        //    float realPosX = Sprite.Position.X * Map.TILE_SIZE;
        //    float realPosY = Sprite.Position.Y * Map.TILE_SIZE;
        //    Sprite.Position = new Vector2(realPosX, realPosY);
        //}

        public Item() { }

        public void SetMovementStrategy(Movement movementStrategy)
        {
            movement = movementStrategy;
        }

        public void SetSprite(Sprite sprite)
        {
            Sprite = sprite;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Sprite.Draw(spriteBatch);
        }

        public void Update(GameTime gameTime)
        {
            Sprite.Update();
        }

        public IGameEntity ShallowCopy()
        {
            return MemberwiseClone() as Item;
        }

        public IGameEntity DeepCopy()
        {
            var clone = MemberwiseClone() as Item;
            clone.Sprite = new Sprite(Sprite.Texture, Sprite.Position, 60);
            return clone;
        }
    }
}
