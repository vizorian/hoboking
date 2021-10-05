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
        private Player player;
        Stopwatch stopwatch;
        public InputController(Player player)
        {
            this.player = player;
            stopwatch = new Stopwatch();
        }

        public void ProcessControls(GameTime gameTime)
        {
            KeyboardState keyboardState = Keyboard.GetState();

            if (keyboardState.IsKeyDown(Keys.Space))
            {
                if (player.State != PlayerState.Jumping || player.State != PlayerState.Falling)
                {
                    stopwatch.Start();
                    player.BeginCharge(gameTime);
                }
            }
            else if (keyboardState.IsKeyUp(Keys.Space) && keyboardState.IsKeyDown(Keys.A))
            {
                if (player.State == PlayerState.Charging)
                {
                    player.Jump(stopwatch.ElapsedMilliseconds, -1);
                    stopwatch.Reset();
                }
            }
            else if (keyboardState.IsKeyUp(Keys.Space) && keyboardState.IsKeyDown(Keys.D))
            {
                if (player.State == PlayerState.Charging)
                {
                    player.Jump(stopwatch.ElapsedMilliseconds, 1);
                    stopwatch.Reset();
                }
            }
            else if(keyboardState.IsKeyUp(Keys.Space))
            {
                if (player.State == PlayerState.Charging)
                {
                    player.Jump(stopwatch.ElapsedMilliseconds, 0);
                    stopwatch.Reset();
                }
            }
            if (keyboardState.IsKeyDown(Keys.A) || keyboardState.IsKeyDown(Keys.Left))
            {
                if (player.State == PlayerState.Idle || player.State == PlayerState.Walking)
                {
                    player.Walk("left", gameTime);
                }
            }
            if (keyboardState.IsKeyDown(Keys.D) || keyboardState.IsKeyDown(Keys.Right))
            {
                if (player.State == PlayerState.Idle || player.State == PlayerState.Walking)
                {
                    player.Walk("right", gameTime);
                }
            }
            if (keyboardState.IsKeyUp(Keys.A) && keyboardState.IsKeyUp(Keys.D))
            {
                if (player.State == PlayerState.Walking)
                {
                    player.Idle(); 
                }
            }

        }
    }
}
