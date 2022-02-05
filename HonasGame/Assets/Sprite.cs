using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace HonasGame.Assets
{
    public struct SpriteAnimation
    {
        public Rectangle[] Frames;
        public double FrameTime;

        public static SpriteAnimation FromSpritesheet(int frameCount, double frameTime, int x, int y, int width, int height)
        {
            SpriteAnimation animation = new SpriteAnimation();
            animation.Frames = new Rectangle[frameCount];
            animation.FrameTime = frameTime;

            for(int i = 0; i < frameCount; i++)
            {
                animation.Frames[i] = new Rectangle(x + i * width, y, width, height);
            }

            return animation;
        }
    }

    public class Sprite
    {
        public Texture2D Texture { get; private set; }

        public Dictionary<string, SpriteAnimation> Animations { get; private set; }

        public Sprite(Texture2D texture)
        {
            Texture = texture;
            Animations = new Dictionary<string, SpriteAnimation>();
        }
    }
}
