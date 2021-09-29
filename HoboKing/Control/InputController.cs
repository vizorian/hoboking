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
        private Player Hobo;
        Stopwatch stopwatch;
        public InputController(Player hobo)
        {
            this.Hobo = hobo;
            stopwatch = new Stopwatch();
        }

        public void ProcessControls(GameTime gameTime)
        {
            KeyboardState keyboardState = Keyboard.GetState();

            if (keyboardState.IsKeyDown(Keys.Space))
            {
                if (Hobo.State != PlayerState.Jumping || Hobo.State != PlayerState.Falling)
                {
                    stopwatch.Start();
                    Hobo.BeginCharge(gameTime);
                }
            }
            else if (keyboardState.IsKeyUp(Keys.Space) && keyboardState.IsKeyDown(Keys.A))
            {
                if (Hobo.State == PlayerState.Charging)
                {
                    Hobo.Jump(stopwatch.ElapsedMilliseconds, -1);
                    stopwatch.Reset();
                }
            }
            else if (keyboardState.IsKeyUp(Keys.Space) && keyboardState.IsKeyDown(Keys.D))
            {
                if (Hobo.State == PlayerState.Charging)
                {
                    Hobo.Jump(stopwatch.ElapsedMilliseconds, 1);
                    stopwatch.Reset();
                }
            }
            else if(keyboardState.IsKeyUp(Keys.Space))
            {
                if (Hobo.State == PlayerState.Charging)
                {
                    Hobo.Jump(stopwatch.ElapsedMilliseconds, 0);
                    stopwatch.Reset();
                }
            }
            if (keyboardState.IsKeyDown(Keys.A) || keyboardState.IsKeyDown(Keys.Left))
            {
                if (Hobo.State == PlayerState.Idle || Hobo.State == PlayerState.Walking)
                {
                    Hobo.Walk("left", gameTime);
                }
            }
            if (keyboardState.IsKeyDown(Keys.D) || keyboardState.IsKeyDown(Keys.Right))
            {
                if (Hobo.State == PlayerState.Idle || Hobo.State == PlayerState.Walking)
                {
                    Hobo.Walk("right", gameTime);
                }
            }
            if (keyboardState.IsKeyUp(Keys.A) && keyboardState.IsKeyUp(Keys.D))
            {
                if (Hobo.State == PlayerState.Walking)
                {
                    Hobo.Idle(); 
                }
            }

        }
    }
}
