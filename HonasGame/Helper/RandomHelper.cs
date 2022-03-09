using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace HonasGame.Helper
{
    public static class RandomHelper
    {
        private static Random random = new Random();

        /// <summary>
        /// Returns a random value from 0 to the upper bound not inclusive.
        /// </summary>
        /// <param name="max">The max upper bound not inclusive</param>
        /// <returns></returns>
        public static int NextInt(int max)
        {
            return random.Next(max);
        }

        /// <summary>
        /// Returns a random value from min inclusive to max exclusive
        /// </summary>
        /// <param name="max">The min lower bound inclusive</param>
        /// <param name="max">The max upper bound not inclusive</param>
        /// <returns></returns>
        public static int NextInt(int min, int max)
        {
            return random.Next(min, max);
        }

        public static float NextFloat()
        {
            return (float)random.NextDouble();
        }

        public static float NextFloat(float max)
        {
            return NextFloat() * max;
        }

        public static float NextFloat(float min, float max)
        {
            return min + NextFloat() * (max - min);
        }

        public static Vector2 RandomPosition(Rectangle bounds)
        {
            return new Vector2(NextFloat(bounds.Left, bounds.Right), NextFloat(bounds.Top, bounds.Bottom));
        }

        public static Vector2 NextDir()
        {
            float angle = NextFloat(0, MathHelper.TwoPi);
            return new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
        }
    }
}
