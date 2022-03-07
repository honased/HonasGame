using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace HonasGame
{
    internal static class Debugger
    {
        internal static bool Debug { get; private set; } = false;

        internal static void Update(GameTime gameTime)
        {
#if DEBUG
            if (Input.IsKeyPressed(Microsoft.Xna.Framework.Input.Keys.F6)) Debug = !Debug;
#endif
        }
    }
}
