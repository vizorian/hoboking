using HoboKing.Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace HoboKing.Control
{
    static class InputController
    {
        public static MouseState MouseState { get; set; }
        public static MouseState PreviousMouseState { get; set; }
        public static KeyboardState KeyState { get; set; }
        public static KeyboardState PreviousKeyState { get; set; }

        public static void Update()
        {
            PreviousKeyState = KeyState;
            KeyState = Keyboard.GetState();
        }

        public static bool KeyPressed(Keys key)
        {
            if (KeyState.IsKeyDown(key) && PreviousKeyState.IsKeyUp(key))
                return true;
            return false;
        }

        public static bool KeyPressedDown(Keys key)
        {
            if (KeyState.IsKeyDown(key))
                return true;
            return false;
        }

        public static bool KeyReleased(Keys key)
        {
            if (KeyState.IsKeyUp(key) && PreviousKeyState.IsKeyDown(key))
                return true;
            return false;
        }
    }
}
