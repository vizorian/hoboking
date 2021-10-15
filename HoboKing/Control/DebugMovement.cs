using HoboKing.Entities;
using Microsoft.Xna.Framework;
using System;

namespace HoboKing.Control
{
    class DebugMovement : Movement
    {
        private const int VERTICAL_SPEED = 10000;
        private const int HORIZONTAL_SPEED = 10000;

        public DebugMovement(Player player) : base(player)
        {
            Gravity = 0;
        }

        public override bool BeginCharge(GameTime gameTime)
        {
            return true;
        }

        public override void Jump(long jumpStrength, int xDirection)
        {

        }

        public override void Up(GameTime gameTime)
        {
            player.VelocityY = -VERTICAL_SPEED * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }
        public override void Down(GameTime gameTime)
        {
            player.VelocityY = VERTICAL_SPEED * (float)gameTime.ElapsedGameTime.TotalSeconds;
        }

        public override void Walk(string direction, GameTime gameTime)
        {
            player.State = PlayerState.Walking;
            if (direction == "left")
            {
                player.VelocityX = -HORIZONTAL_SPEED * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            if (direction == "right")
            {
                player.VelocityX = HORIZONTAL_SPEED * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
        }
    }
}
