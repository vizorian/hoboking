using HoboKing.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace HoboKing.Control
{
    class InputController
    {
        private Hobo Hobo;
        Stopwatch stopwatch;
        public InputController(Hobo hobo)
        {
            this.Hobo = hobo;
            stopwatch = new Stopwatch();
        }

        public void ProcessControls(GameTime gameTime)
        {
            KeyboardState keyboardState = Keyboard.GetState();

            if (keyboardState.IsKeyDown(Keys.Space))
            {
                if (Hobo.State != HoboState.Jumping || Hobo.State != HoboState.Falling)
                {
                    stopwatch.Start();
                    Hobo.BeginCharge(gameTime);
                }
            }
            else if (keyboardState.IsKeyUp(Keys.Space) && keyboardState.IsKeyDown(Keys.A))
            {
                if (Hobo.State == HoboState.Charging)
                {
                    Hobo.Jump(stopwatch.ElapsedMilliseconds, -1);
                    stopwatch.Reset();
                }
            }
            else if (keyboardState.IsKeyUp(Keys.Space) && keyboardState.IsKeyDown(Keys.D))
            {
                if (Hobo.State == HoboState.Charging)
                {
                    Hobo.Jump(stopwatch.ElapsedMilliseconds, 1);
                    stopwatch.Reset();
                }
            }
            else if(keyboardState.IsKeyUp(Keys.Space))
            {
                if (Hobo.State == HoboState.Charging)
                {
                    Hobo.Jump(stopwatch.ElapsedMilliseconds, 0);
                    stopwatch.Reset();
                }
            }
            if (keyboardState.IsKeyDown(Keys.A) || keyboardState.IsKeyDown(Keys.Left))
            {
                if (Hobo.State == HoboState.Idle || Hobo.State == HoboState.Walking)
                {
                    Hobo.Walk("left", gameTime);
                }
            }
            if (keyboardState.IsKeyDown(Keys.D) || keyboardState.IsKeyDown(Keys.Right))
            {
                if (Hobo.State == HoboState.Idle || Hobo.State == HoboState.Walking)
                {
                    Hobo.Walk("right", gameTime);
                }
            }
            if (keyboardState.IsKeyUp(Keys.A) && keyboardState.IsKeyUp(Keys.D))
            {
                if (Hobo.State == HoboState.Walking)
                {
                    Hobo.Idle(); 
                }
            }

        }
    }
}
