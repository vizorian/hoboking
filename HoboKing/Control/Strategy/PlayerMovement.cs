using HoboKing.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using tainicom.Aether.Physics2D.Dynamics;

namespace HoboKing.Control.Strategy
{
    internal class PlayerMovement : Movement
    {
        private const float MAX_JUMP = 2.5f;
        private const float HORIZONTAL_SPEED = 2f;

        public PlayerMovement(Player player) : base(player)
        {
            player.UseGravity(true);
        }

        public override void AcceptInputs(GameTime gameTime)
        {
            // UP
            if (InputController.KeyPressed(Keys.Space) || InputController.KeyPressed(Keys.W)) Up();

            // DOWN
            if (InputController.KeyPressedDown(Keys.S)) Down();

            if (InputController.KeyPressedDown(Keys.A)) Left();

            if (InputController.KeyReleased(Keys.A)) SetVelocity(new Vector2(0, target.Body.LinearVelocity.Y));

            // RIGHT
            if (InputController.KeyPressedDown(Keys.D)) Right();

            if (InputController.KeyReleased(Keys.D)) SetVelocity(new Vector2(0, target.Body.LinearVelocity.Y));
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