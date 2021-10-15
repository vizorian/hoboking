using HoboKing.Entities;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace HoboKing.Control
{
    class PlayerMovement : Movement
    {
        private const int MAX_JUMP = 1000;
        private const int MIN_JUMP = 1;
        private const int HORIZONTAL_SPEED = 5000;

        public PlayerMovement(Player player) : base(player)
        {
            Gravity = 300;
        }

        public override void Jump(long jumpStrength, int xDirection)
        {
            if (jumpStrength > MAX_JUMP)
                jumpStrength = MAX_JUMP;
            if (jumpStrength < MIN_JUMP)
                jumpStrength = MIN_JUMP;
            if (player.VelocityY == 0)
            {
                player.VelocityY = -jumpStrength;
                player.State = PlayerState.Jumping;
            }
            switch (xDirection)
            {
                case -1:
                    Console.WriteLine($"Jump strenght is {jumpStrength}, LEFT");
                    player.VelocityX = xDirection * HORIZONTAL_SPEED;
                    return;
                case 1:
                    Console.WriteLine($"Jump strenght is {jumpStrength}, RIGHT");
                    player.VelocityX = xDirection * HORIZONTAL_SPEED;
                    return;
                default:
                    Console.WriteLine($"Jump strenght is {jumpStrength}, STRAIGHT UP");
                    return;
            }
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

        public override bool BeginCharge(GameTime gameTime)
        {
            if (player.State == PlayerState.Jumping || player.State == PlayerState.Falling)
            {
                return false;
            }
            player.State = PlayerState.Charging;
            return true;
        }

        public override void Down(GameTime gameTime)
        {
        }

        public override void Up(GameTime gameTime)
        {
        }
    }
}
