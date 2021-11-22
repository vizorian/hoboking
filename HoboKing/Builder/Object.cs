using HoboKing.Control;
using HoboKing.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using tainicom.Aether.Physics2D.Dynamics;

namespace HoboKing.Entities
{
    class Object : GameEntity
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
