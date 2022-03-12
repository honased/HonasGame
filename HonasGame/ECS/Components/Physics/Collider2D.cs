using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using HonasGame.Rendering;

namespace HonasGame.ECS.Components.Physics
{
    public class Collider2D : Component
    {
        public CollisionShape Shape { get; set; }

        public Transform2D Transform { get; set; }

        public uint Tag { get; set; } = 0;

        public Collider2D(Entity parent) : base(parent)
        {

        }

        public bool CollidesWith<T>() where T : Entity
        {
            if (Shape == null) return false;

            foreach(Entity e in Scene.GetEntities())
            {
                if(e != Parent && e is T && e.Enabled)
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

        /// <summary>
        /// Check for a collision with another collider with the given tag
        /// </summary>
        /// <param name="tag">The tag to check for collisions with</param>
        /// <returns>If there was a collision or not</returns>
        public bool CollidesWith(uint tag)
        {
            return CollidesWith(tag, Vector2.Zero, out var e);
        }

        /// <summary>
        /// Checks for collisions with any entity
        /// </summary>
        /// <returns>The other collider's tag</returns>
        public Entity CollidesWithAnything(out uint tag)
        {
            CollidesWith(uint.MaxValue, Vector2.Zero, out var e);
            if (e == null) tag = 0;
            else
            {
                if(e.GetComponent<Collider2D>(out Collider2D collider))
                {
                    tag = collider.Tag;
                }
                else tag = 0;
            }
            return e;
        }

        public bool CollidesWith(uint tag, out Entity e)
        {
            return CollidesWith(tag, Vector2.Zero, out e);
        }

        public bool CollidesWithGetTag(uint tag, Vector2 offset, out uint outTag)
        {
            outTag = 0;
            if(CollidesWith(tag, offset, out Entity e))
            {
                if(e.GetComponent<Collider2D>(out var collider))
                {
                    outTag = collider.Tag;
                    return true;
                }
            }
            return false;
        }

        public bool CollidesWith(uint tag, Vector2 offset, out Entity entity)
        {
            entity = null;
            if (Shape == null) return false;
            if (tag == 0) return false;

            foreach (Entity e in Scene.GetEntities())
            {
                if (e != Parent && e.Enabled)
                {
                    foreach (Component c in e.GetComponents())
                    {
                        if (c is Collider2D collider && (collider.Tag & tag) > 0)
                        {
                            if (Shape.CollidesWith(collider.Shape, offset))
                            {
                                entity = e;
                                return true;
                            }
                        }
                    }
                }
            }

            return false;
        }

        public override void Update(GameTime gameTime)
        {
            if(Transform != null && Shape != null)
            {
                Shape.Position = Transform.Position;
            }
        }

#if DEBUG
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (!Debugger.Debug) return;

            if( Shape is BoundingRectangle rect )
            {
                spriteBatch.DrawRectangle(new Rectangle((int)rect.Left, (int)rect.Top, (int)rect.Size.X, (int)rect.Size.Y), Color.Red, 1);
            }
            else if(Shape is BoundingCircle circle)
            {
                spriteBatch.DrawCircle(circle.Center, circle.Radius, Color.Red);
            }
        }
#endif
    }
}
