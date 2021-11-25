using System.Diagnostics.CodeAnalysis;
using HoboKing.Control.Strategy;

namespace HoboKing.Entities
{
    [ExcludeFromCodeCoverage]
    internal class Item : GameEntity
    {
        private Movement movement;

        //public Critter(GraphicsDevice graphics, Texture2D texture, Vector2 position)
        //{
        //    Sprite = new Sprite(graphics, texture, position, 40);

        //    // Recalculates tiles to absolute coordinates
        //    float realPosX = Sprite.Position.X * Map.TILE_SIZE;
        //    float realPosY = Sprite.Position.Y * Map.TILE_SIZE;
        //    Sprite.Position = new Vector2(realPosX, realPosY);
        //}

        public void SetMovementStrategy(Movement movementStrategy)
        {
            movement = movementStrategy;
        }


        public override GameEntity ShallowCopy()
        {
            return MemberwiseClone() as Item;
        }

        public override GameEntity DeepCopy()
        {
            var clone = MemberwiseClone() as Item;
            return clone;
        }
    }
}