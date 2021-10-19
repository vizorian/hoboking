using HoboKing.Entities;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using tainicom.Aether.Physics2D.Dynamics;

namespace HoboKing.Control
{
    class PlayerMovement : Movement
    {
        private const float MAX_JUMP = 2.5f;
        private const float HORIZONTAL_SPEED = 2f;

        public PlayerMovement(Player player, World world) : base(player)
        {

        }

        public override void Jump(long jumpStrength, int xDirection)
        {
            player.Sprite.body.ApplyLinearImpulse(new Vector2(0, -MAX_JUMP));
        }

        public override void Walk(string direction, GameTime gameTime)
        {
            if (direction == "left")
            {
                player.Sprite.body.ApplyForce(new Vector2(-HORIZONTAL_SPEED, 0));

            }
            if (direction == "right")
            {
                player.Sprite.body.ApplyForce(new Vector2(HORIZONTAL_SPEED, 0));
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
            player.Sprite.body.ApplyLinearImpulse(new Vector2(0, -MAX_JUMP));
        }
    }
}
