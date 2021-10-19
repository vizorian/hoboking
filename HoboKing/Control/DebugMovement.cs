using HoboKing.Entities;
using Microsoft.Xna.Framework;
using System;
using tainicom.Aether.Physics2D.Dynamics;

namespace HoboKing.Control
{
    class DebugMovement : Movement
    {
        private const int VERTICAL_SPEED = 2;
        private const int HORIZONTAL_SPEED = 2;

        public DebugMovement(Player player, World world) : base(player)
        {
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

        }
        public override void Down(GameTime gameTime)
        {
        }

        public override void Walk(string direction, GameTime gameTime)
        {
            if (direction == "left")
            {
                player.Sprite.body.SetTransform(new Vector2(-2f, player.Sprite.body.Position.Y), 0f);
            }
            if (direction == "right")
            {
                player.Sprite.body.SetTransform(new Vector2(2f, player.Sprite.body.Position.Y), 0f);
            }
        }
    }
}
