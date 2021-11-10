﻿using HoboKing.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace HoboKing.Control.Strategy
{
    class IceMovement : Movement
    {
        private const float MAX_JUMP = 2.5f;
        private const float HORIZONTAL_SPEED = 3f;
        private const float MAX_HORIZONTAL_SPEED = 1f;

        public IceMovement(Player player) : base(player)
        {
            player.UseGravity(true);
            player.fixture.Friction = 0.4f;
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

            if (InputController.KeyReleased(Keys.A))
            {
            }

            // RIGHT
            if (InputController.KeyPressedDown(Keys.D))
            {
                Right();
            }

            if (InputController.KeyReleased(Keys.D))
            {
            }
        }

        public override void Down()
        {
        }

        public override void Left()
        {
            SetVelocity(new Vector2(-HORIZONTAL_SPEED, target.body.LinearVelocity.Y));
            //if (target.body.LinearVelocity.X > -MAX_HORIZONTAL_SPEED)
            //{
            //    target.body.ApplyLinearImpulse(new Vector2(-HORIZONTAL_SPEED, 0));
            //}
        }

        public override void Right()
        {
            SetVelocity(new Vector2(HORIZONTAL_SPEED, target.body.LinearVelocity.Y));

            if (target.body.LinearVelocity.X < MAX_HORIZONTAL_SPEED)
            {
                target.body.ApplyLinearImpulse(new Vector2(HORIZONTAL_SPEED, 0));
            }
        }

        public override void Up()
        {
            target.body.ApplyLinearImpulse(new Vector2(0, -MAX_JUMP));
        }
    }
}
