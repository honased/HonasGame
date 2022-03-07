using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace HonasGame
{
    public static class Input
    {
        private static KeyboardState _keyState;
        private static KeyboardState _keyStateOld;

        internal static void Update()
        {
            _keyStateOld = _keyState;
            _keyState = Keyboard.GetState();
        }

        public static bool IsKeyDown(Keys key)
        {
            return _keyState.IsKeyDown(key);
        }

        public static bool IsKeyPressed(Keys key)
        {
            return _keyState.IsKeyDown(key) && !_keyStateOld.IsKeyDown(key);
        }
    }
}
