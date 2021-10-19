using HoboKing.Control;
using HoboKing.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HoboKing.Entities
{
    class Critter : IGameEntity
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

        public Critter(){ }

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
            // Loop through all Tiles and check for collisions
            //foreach (var entity in EntityManager.Entities)
            //{
            //    if (entity is Tile)
            //    {
            //        if (Sprite.Collision(entity.Sprite))
            //        {
            //            if (Sprite.Rectangle.Bottom < entity.Sprite.Rectangle.Top)
            //            {
            //                Sprite.Position = new Vector2(Sprite.Position.X, previousPosition.Y);
            //                VelocityY = 0.01f;
            //            }

            //            if (Sprite.Rectangle.Top < entity.Sprite.Rectangle.Bottom)
            //            {
            //                Sprite.Position = new Vector2(Sprite.Position.X, previousPosition.Y);
            //                VelocityY = 0.01f;
            //            }

            //            if (Sprite.Rectangle.Right > entity.Sprite.Rectangle.Left)
            //            {
            //                Sprite.Position = new Vector2(previousPosition.X, Sprite.Position.Y);
            //                VelocityX = 0.01f;
            //            }

            //            if (Sprite.Rectangle.Left < entity.Sprite.Rectangle.Right)
            //            {
            //                Sprite.Position = new Vector2(previousPosition.X, Sprite.Position.Y);
            //                VelocityX = 0.01f;
            //            }
            //        }
            //    }
            //}
        }
    }
}
