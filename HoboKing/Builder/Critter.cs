using HoboKing.Control;
using HoboKing.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HoboKing.Entities
{
    class Critter : GameEntity
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
