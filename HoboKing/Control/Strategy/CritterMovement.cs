﻿using HoboKing.Entities;
using HoboKing.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace HoboKing.Control.Strategy
{
    class CritterMovement : Movement
    {
        const float HORIZONTAL_SPEED = 0.5f;

        const float COUNT_DURATION = 4f; //every  4s.
        const float CHANGE_DIRECTION = 2f; //every  2s.
        float currentTime = 0f;

        public CritterMovement(GameEntity target) : base(target)
        {
        }

        public override void AcceptInputs(GameTime gameTime)
        {
            currentTime += (float)gameTime.ElapsedGameTime.TotalSeconds; //Time passed since last Update() 

            if (currentTime >= CHANGE_DIRECTION)
            {
                // Change direction
                Right();
            }
            else
            {
                Left();
            }

            // Timer reset
            if (currentTime >= COUNT_DURATION)
            {
                currentTime -= COUNT_DURATION;
            }
        }

        [ExcludeFromCodeCoverage]
        public override void Down()
        {
        }

        public override void Left()
        {
            target.ChangePosition(target.Position += new Vector2(-HORIZONTAL_SPEED, 0));
        }

        public override void Right()
        {
            target.ChangePosition(target.Position += new Vector2(HORIZONTAL_SPEED, 0));
        }

        [ExcludeFromCodeCoverage]
        public override void Up()
        {
        }
    }
}
