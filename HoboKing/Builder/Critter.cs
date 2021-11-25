using HoboKing.Control.Strategy;
using HoboKing.Entities;
using Microsoft.Xna.Framework;

namespace HoboKing.Builder
{
    internal class Critter : GameEntity
    {
        private Movement movement;

        public void SetMovementStrategy(Movement movementStrategy)
        {
            movement = movementStrategy;
        }

        public void Update(GameTime gameTime)
        {
            movement.AcceptInputs(gameTime);
        }

        public override GameEntity ShallowCopy()
        {
            return MemberwiseClone() as Critter;
        }

        public override GameEntity DeepCopy()
        {
            return ShallowCopy() as Critter;
        }
    }
}