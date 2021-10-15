using HoboKing.Entities;
using Microsoft.Xna.Framework;

namespace HoboKing.Control
{
    abstract class Movement
    {
        public int Gravity { get; set; }
        protected Player player;

        public Movement(Player player)
        {
            this.player = player;
        }
        public abstract void Walk(string direction, GameTime gameTime);
        public abstract void Jump(long jumpStrength, int xDirection);
        public abstract bool BeginCharge(GameTime gameTime);
        public abstract void Down(GameTime gameTime);
        public abstract void Up(GameTime gameTime);
    }
}
