using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace HonasGame.Rendering
{
    public static class SpritebatchHelper
    {
        private static Texture2D _1Pixel;

        // Based on CraftworkGames response at https://community.monogame.net/t/line-drawing/6962/4
        public static void DrawLine(this SpriteBatch spriteBatch, Vector2 p1, Vector2 p2, Color color, float width)
        {
            spriteBatch.DrawLine(p1, Vector2.Distance(p1, p2), (float)Math.Atan2(p2.Y - p1.Y, p2.X - p1.X), color, width);
        }

        // Based on CraftworkGames response at https://community.monogame.net/t/line-drawing/6962/4
        public static void DrawLine(this SpriteBatch spriteBatch, Vector2 p, float length, float angle, Color color, float width)
        {
            if (_1Pixel == null)
            {
                _1Pixel = new Texture2D(spriteBatch.GraphicsDevice, 1, 1);
                _1Pixel.SetData(new Color[] { Color.White });
            }

            spriteBatch.Draw(_1Pixel, p, null, color, angle, new Vector2(0f, 0.5f), new Vector2(length, width), SpriteEffects.None, 0.0f);
        }

        public static void DrawRectangle(this SpriteBatch spriteBatch, Rectangle rect, Color color, float width)
        {
            Vector2 tl = new Vector2(rect.Left, rect.Top);
            Vector2 tr = new Vector2(rect.Right, rect.Top);
            Vector2 bl = new Vector2(rect.Left, rect.Bottom);
            Vector2 br = new Vector2(rect.Right, rect.Bottom);
            Vector2 halfX = Vector2.UnitX * width / 2.0f;
            Vector2 halfY = Vector2.UnitY * width / 2.0f;

            spriteBatch.DrawLine(tl + halfY, tr + halfY, color, width);
            spriteBatch.DrawLine(tr - halfX, br - halfX, color, width);
            spriteBatch.DrawLine(br - halfY, bl - halfY, color, width);
            spriteBatch.DrawLine(bl + halfX, tl + halfX, color, width);
        }
    }
}
