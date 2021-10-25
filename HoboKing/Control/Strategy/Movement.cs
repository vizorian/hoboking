using HoboKing.Entities;
using Microsoft.Xna.Framework;

namespace HoboKing.Control
{
    abstract class Movement
    {
        public int Gravity { get; set; }
        protected IGameEntity target;

        public Movement(IGameEntity target)
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
            target.Sprite.body.LinearVelocity = newVelocity;
        }
    }
}
