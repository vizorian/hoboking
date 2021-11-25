using HoboKing.Entities;
using HoboKing.Graphics;
using Microsoft.Xna.Framework;

namespace HoboKing.Control
{
    abstract class Movement
    {
        protected GameEntity target;

        public Movement(GameEntity target)
        {
            this.target = target;
        }

        public abstract void Up();
        public abstract void Down();
        public abstract void Left();
        public abstract void Right();
        public abstract void AcceptInputs(GameTime gameTime);

        protected void SetVelocity(Vector2 newVelocity)
        {
            target.Body.LinearVelocity = newVelocity;
        }
    }
}
