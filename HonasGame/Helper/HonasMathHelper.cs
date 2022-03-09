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

        public static Vector2 LerpDelta(Vector2 value1, Vector2 value2, float amount, GameTime gameTime)
        {
            return new Vector2(
                MathHelper.Lerp(value1.X, value2.X, 1 - (float)Math.Pow(1 - amount, gameTime.ElapsedGameTime.TotalSeconds * 60.0)),
                MathHelper.Lerp(value1.Y, value2.Y, 1 - (float)Math.Pow(1 - amount, gameTime.ElapsedGameTime.TotalSeconds * 60.0))
                );
        }

        public static Vector2 LerpDelta(Vector2 value1, Vector2 value2, Vector2 amount, GameTime gameTime)
        {
            return new Vector2(
                MathHelper.Lerp(value1.X, value2.X, 1 - (float)Math.Pow(1 - amount.X, gameTime.ElapsedGameTime.TotalSeconds * 60.0)),
                MathHelper.Lerp(value1.Y, value2.Y, 1 - (float)Math.Pow(1 - amount.Y, gameTime.ElapsedGameTime.TotalSeconds * 60.0))
                );
        }
    }
}
