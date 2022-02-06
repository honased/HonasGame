using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace HonasGame.ECS.Components.Physics
{
    public class Collider2D : Component
    {
        public CollisionShape Shape { get; set; }

        public int Tag { get; set; } = 0;

        public Collider2D(Entity parent) : base(parent)
        {

        }

        public bool CollidesWith<T>() where T : Entity
        {
            if (Shape == null) return false;

            foreach(Entity e in Scene.GetEntities())
            {
                if(e != Parent && e is T)
                {
                    foreach(Component c in e.GetComponents())
                    {
                        if(c is Collider2D collider)
                        {
                            if (Shape.CollidesWith(collider.Shape)) return true;
                        }
                    }
                }
            }

            return false;
        }

        public bool CollidesWith(int tag)
        {
            return CollidesWith(tag, Vector2.Zero);
        }

        public bool CollidesWith(int tag, Vector2 offset)
        {
            if (Shape == null) return false;

            foreach (Entity e in Scene.GetEntities())
            {
                if (e != Parent)
                {
                    foreach (Component c in e.GetComponents())
                    {
                        if (c is Collider2D collider && (collider.Tag & tag) > 0)
                        {
                            if (Shape.CollidesWith(collider.Shape, offset)) return true;
                        }
                    }
                }
            }

            return false;
        }

        static Texture2D _pointTexture;
        public static void DrawRectangle(SpriteBatch spriteBatch, Rectangle rectangle, Color color, int lineWidth)
        {
            if (_pointTexture == null)
            {
                _pointTexture = new Texture2D(spriteBatch.GraphicsDevice, 1, 1);
                _pointTexture.SetData<Color>(new Color[] { Color.White });
            }

            spriteBatch.Draw(_pointTexture, new Rectangle(rectangle.X, rectangle.Y, lineWidth, rectangle.Height + lineWidth), color);
            spriteBatch.Draw(_pointTexture, new Rectangle(rectangle.X, rectangle.Y, rectangle.Width + lineWidth, lineWidth), color);
            spriteBatch.Draw(_pointTexture, new Rectangle(rectangle.X + rectangle.Width, rectangle.Y, lineWidth, rectangle.Height + lineWidth), color);
            spriteBatch.Draw(_pointTexture, new Rectangle(rectangle.X, rectangle.Y + rectangle.Height, rectangle.Width + lineWidth, lineWidth), color);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if( Shape is BoundingRectangle rect )
            {
                //DrawRectangle(spriteBatch, new Rectangle((int)rect.Left - (int)rect.Origin.X, (int)rect.Top - (int)rect.Origin.Y, (int)rect.Size.X, (int)rect.Size.Y), Color.Red, 1);
            }
        }
    }
}
