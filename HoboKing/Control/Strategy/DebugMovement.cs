using HoboKing.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace HoboKing.Control.Strategy
{
    internal class DebugMovement : Movement
    {
        private const int VERTICAL_SPEED = 3;
        private const int HORIZONTAL_SPEED = 3;

        public DebugMovement(Player player) : base(player)
        {
            player.UseGravity(false);
        }

        public override void Up()
        {
            SetVelocity(new Vector2(target.Body.LinearVelocity.X, -VERTICAL_SPEED));
        }

        public override void Down()
        {
            SetVelocity(new Vector2(target.Body.LinearVelocity.X, VERTICAL_SPEED));
        }

        public override void Left()
        {
            SetVelocity(new Vector2(-HORIZONTAL_SPEED, target.Body.LinearVelocity.Y));
        }

        public override void Right()
        {
            SetVelocity(new Vector2(HORIZONTAL_SPEED, target.Body.LinearVelocity.Y));
        }

        public override void AcceptInputs(GameTime gameTime)
        {
            // UP
            if (InputController.KeyPressedDown(Keys.W)) Up();

            if (InputController.KeyReleased(Keys.W)) SetVelocity(new Vector2(target.Body.LinearVelocity.X, 0));

            // DOWN
            if (InputController.KeyPressedDown(Keys.S)) Down();

            if (InputController.KeyReleased(Keys.S)) SetVelocity(new Vector2(target.Body.LinearVelocity.X, 0));

            // LEFT
            if (InputController.KeyPressedDown(Keys.A)) Left();

            if (InputController.KeyReleased(Keys.A)) SetVelocity(new Vector2(0, target.Body.LinearVelocity.Y));

            // RIGHT
            if (InputController.KeyPressedDown(Keys.D)) Right();

            if (InputController.KeyReleased(Keys.D)) SetVelocity(new Vector2(0, target.Body.LinearVelocity.Y));
        }
    }
}