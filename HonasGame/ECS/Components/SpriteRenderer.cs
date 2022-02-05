using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace HonasGame.ECS.Components
{
    public class SpriteRenderer : Component
    {
        public Vector2 Position { get; set; }
        public Vector2 Scale { get; set; }
        public Vector2 Origin { get; set; }

        public SpriteRenderer(Entity parent) : base(parent)
        {

        }

        public override void Update(GameTime gameTime)
        {
            
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            
        }
    }
}
