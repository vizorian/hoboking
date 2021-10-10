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
        public KeyboardState KeyState { get; set; }
        public KeyboardState PreviousKeyState { get; set; }

        private Player player;
        Stopwatch stopwatch;
        public InputController(Player player)
        {
            this.player = player;
            stopwatch = new Stopwatch();
        }

        public void Update()
        {
            PreviousKeyState = KeyState;
            KeyState = Keyboard.GetState();
        }

        public bool KeyPressed(Keys key)
        {
            if (KeyState.IsKeyDown(key) && PreviousKeyState.IsKeyUp(key))
                return true;
            return false;
        }

        public bool KeyReleased(Keys key)
        {
            if (KeyState.IsKeyUp(key) && PreviousKeyState.IsKeyDown(key))
                return true;
            return false;
        }

        public bool KeyDown(Keys key)
        {
            if (KeyState.IsKeyDown(key))
            {
                return true;
            }
            return false;
        }


        public void ProcessControls(GameTime gameTime)
        {
            if (KeyPressed(Keys.Space))
            {
                if (player.State != PlayerState.Jumping || player.State != PlayerState.Falling)
                {
                    Console.WriteLine("Charging for a jump");
                    stopwatch.Start();
                    player.BeginCharge(gameTime);
                }
            }
            if (KeyReleased(Keys.Space) && KeyDown(Keys.A))
            {
                Console.WriteLine("Jumping left");

                player.Jump(stopwatch.ElapsedMilliseconds, -1);
                stopwatch.Reset();
            }
            else if (KeyReleased(Keys.Space) && KeyDown(Keys.D))
            {
                Console.WriteLine("Jumping right");

                player.Jump(stopwatch.ElapsedMilliseconds, 1);
                stopwatch.Reset();
            }
           else if(KeyReleased(Keys.Space))
            {
                Console.WriteLine("Jumping up");

                player.Jump(stopwatch.ElapsedMilliseconds, 0);
                stopwatch.Reset();
            }


            if (KeyDown(Keys.A))
            {
                if (player.State == PlayerState.Idle || player.State == PlayerState.Walking)
                {
                    player.Walk("left", gameTime);
                }
            }
            if (KeyDown(Keys.D))
            {
                if (player.State == PlayerState.Idle || player.State == PlayerState.Walking)
                {
                    player.Walk("right", gameTime);
                }
            }
            if (KeyReleased(Keys.A) && KeyReleased(Keys.D))
            {
                if (player.State == PlayerState.Walking)
                {
                    player.Idle(); 
                }
            }

        }
    }
}
