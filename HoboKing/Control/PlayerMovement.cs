using HoboKing.Entities;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace HoboKing.Control
{
    class PlayerMovement : Movement
    {
        // CONSTANTS
        private const float GRAVITY = 10;
        private const int MAX_JUMP = 10;
        private const int MIN_JUMP = 1;
        private const int HORIZONTAL_SPEED = 100;

        public Player Player { get; set; }
        public PlayerMovement(Player player)
        {
            Player = player;
        }

        public override void Jump(long jumpStrength, int xDirection)
        {
            if (jumpStrength > MAX_JUMP)
                jumpStrength = MAX_JUMP;
            if (jumpStrength < MIN_JUMP)
                jumpStrength = MIN_JUMP;
            if (Player.PlayerVelocityY == 0)
            {
                Player.PlayerVelocityY = -jumpStrength;
                Player.State = PlayerState.Jumping;
            }
            switch (xDirection)
            {
                case -1:
                    Console.WriteLine($"Jump strenght is {jumpStrength}, LEFT");
                    Player.PlayerVelocityX = xDirection * HORIZONTAL_SPEED;
                    return;
                case 1:
                    Console.WriteLine($"Jump strenght is {jumpStrength}, RIGHT");
                    Player.PlayerVelocityX = xDirection * HORIZONTAL_SPEED;
                    return;
                default:
                    Console.WriteLine($"Jump strenght is {jumpStrength}, STRAIGHT UP");
                    return;
            }
        }

        public override void Walk(string direction, GameTime gameTime)
        {
            Player.State = PlayerState.Walking;
            if (direction == "left")
            {
                Player.PlayerVelocityX += -HORIZONTAL_SPEED * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
            if (direction == "right")
            {
                Player.PlayerVelocityX += HORIZONTAL_SPEED * (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
        }

        public override bool BeginCharge(GameTime gameTime)
        {
            if (Player.State == PlayerState.Jumping || Player.State == PlayerState.Falling)
            {
                return false;
            }
            Player.State = PlayerState.Charging;
            return true;
        }

        public override void Idle()
        {
            Player.State = PlayerState.Idle;
        }
    }
}
