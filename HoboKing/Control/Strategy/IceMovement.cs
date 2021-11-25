using HoboKing.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace HoboKing.Control.Strategy
{
    class IceMovement : Movement
    {
        private const float MAX_JUMP = 2.5f;
        private const float HORIZONTAL_SPEED = 3f;

        public IceMovement(Player player) : base(player)
        {
            player.UseGravity(true);
            player.Fixture.Friction = 0.4f;
        }

        public override void AcceptInputs(GameTime gameTime)
        {
            // UP
            if (InputController.KeyPressed(Keys.Space) || InputController.KeyPressed(Keys.W))
            {
                Up();
            }

            // DOWN
            if (InputController.KeyPressedDown(Keys.S))
            {
                Down();
            }

            // LEFT
            if (InputController.KeyPressedDown(Keys.A))
            {
                Left();
            }

            // RIGHT
            if (InputController.KeyPressedDown(Keys.D))
            {
                Right();
            }
        }

        public override void Down()
        {
        }

        public override void Left()
        {
            SetVelocity(new Vector2(-HORIZONTAL_SPEED, target.Body.LinearVelocity.Y));
        }

        public override void Right()
        {
            SetVelocity(new Vector2(HORIZONTAL_SPEED, target.Body.LinearVelocity.Y));
        }

        public override void Up()
        {
            target.Body.ApplyLinearImpulse(new Vector2(0, -MAX_JUMP));
        }
    }
}
