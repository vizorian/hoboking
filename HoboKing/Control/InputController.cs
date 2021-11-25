using Microsoft.Xna.Framework.Input;

namespace HoboKing.Control
{
    internal static class InputController
    {
        public static MouseState MouseState { get; set; }
        public static MouseState PreviousMouseState { get; set; }
        public static KeyboardState KeyboardState { get; set; }
        public static KeyboardState PreviousKeyboardState { get; set; }

        public static void Update()
        {
            PreviousKeyboardState = KeyboardState;
            KeyboardState = Keyboard.GetState();

            PreviousMouseState = MouseState;
            MouseState = Mouse.GetState();
        }

        public static bool KeyPressed(Keys key)
        {
            if (KeyboardState.IsKeyDown(key) && PreviousKeyboardState.IsKeyUp(key))
                return true;
            return false;
        }

        public static bool KeyPressedDown(Keys key)
        {
            if (KeyboardState.IsKeyDown(key))
                return true;
            return false;
        }

        public static bool KeyReleased(Keys key)
        {
            if (KeyboardState.IsKeyUp(key) && PreviousKeyboardState.IsKeyDown(key))
                return true;
            return false;
        }
    }
}