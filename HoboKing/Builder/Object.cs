using HoboKing.Control.Strategy;
using HoboKing.Entities;

namespace HoboKing.Builder
{
    internal class Object : GameEntity
    {
        private Movement movement;

        

        public void SetMovementStrategy(Movement movementStrategy)
        {
            movement = movementStrategy;
        }

        public override GameEntity ShallowCopy()
        {
            return MemberwiseClone() as Object;
        }

        public override GameEntity DeepCopy()
        {
            return ShallowCopy() as Object;
        }
    }
}