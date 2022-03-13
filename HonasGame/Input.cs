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
        private static GamePadState _padState;
        private static GamePadState _padStateOld;

        private static float DEAD_ZONE = 0.1f;

        internal static void Update()
        {
            _keyStateOld = _keyState;
            _keyState = Keyboard.GetState();

            _padStateOld = _padState;
            _padState = GamePad.GetState(0);
        }

        public static bool IsKeyDown(Keys key)
        {
            return _keyState.IsKeyDown(key);
        }

        public static bool IsKeyPressed(Keys key)
        {
            return _keyState.IsKeyDown(key) && !_keyStateOld.IsKeyDown(key);
        }

        public static bool CheckAnalogDirection(bool leftStick, bool xAxis, int direction)
        {
            direction = Math.Sign(direction);
            if(leftStick)
            {
                if (xAxis) return _padState.ThumbSticks.Left.X * direction >= DEAD_ZONE;
                else return _padState.ThumbSticks.Left.Y * -direction >= DEAD_ZONE;
            }
            else
            {
                if (xAxis) return _padState.ThumbSticks.Right.X * direction >= DEAD_ZONE;
                else return _padState.ThumbSticks.Right.Y * -direction >= DEAD_ZONE;
            }
        }

        public static bool CheckAnalogPressed(bool leftStick, bool xAxis, int direction)
        {
            direction = Math.Sign(direction);
            if (leftStick)
            {
                if (xAxis) return (_padState.ThumbSticks.Left.X * direction >= DEAD_ZONE) && !(_padStateOld.ThumbSticks.Left.X * direction >= DEAD_ZONE);
                else return (_padState.ThumbSticks.Left.Y * -direction >= DEAD_ZONE) && !(_padStateOld.ThumbSticks.Left.Y * -direction >= DEAD_ZONE);
            }
            else
            {
                if (xAxis) return (_padState.ThumbSticks.Right.X * direction >= DEAD_ZONE) && !(_padStateOld.ThumbSticks.Right.X * direction >= DEAD_ZONE);
                else return (_padState.ThumbSticks.Right.Y * -direction >= DEAD_ZONE) && !(_padStateOld.ThumbSticks.Right.Y * -direction >= DEAD_ZONE);
            }
        }

        public static bool IsButtonDown(Buttons button)
        {
            return _padState.IsButtonDown(button);
        }

        public static bool IsButtonPressed(Buttons button)
        {
            return _padState.IsButtonDown(button) && !_padStateOld.IsButtonDown(button);
        }
    }
}
