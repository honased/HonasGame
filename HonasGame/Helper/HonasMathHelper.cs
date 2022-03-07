using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace HonasGame.Helper
{
    public static class HonasMathHelper
    {
        public static float LerpDelta(float value1, float value2, float amount, GameTime gameTime)
        {
            return MathHelper.Lerp(value1, value2, 1 - (float)Math.Pow(1 - amount, gameTime.ElapsedGameTime.TotalSeconds * 60.0));
        }
    }
}
