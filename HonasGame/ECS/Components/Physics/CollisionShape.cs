using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace HonasGame.ECS.Components.Physics
{
    public abstract class CollisionShape
    {
        public Vector2 Position { get; set; }
        public Vector2 Origin { get; set; } = Vector2.Zero;
        protected abstract bool CheckCollision(CollisionShape other, Vector2 offset);
        public bool CollidesWith(CollisionShape other)
        {
            return CheckCollision(other, -Origin);
        }

        public bool CollidesWith(CollisionShape other, Vector2 offset)
        {
            return CheckCollision(other, -Origin + offset);
        }

        protected static class CollisionResolver
        {
            public static bool Collides(BoundingRectangle a, BoundingRectangle b)
            {
                return a.Left < b.Right && a.Right > b.Left && a.Top < b.Bottom && a.Bottom > b.Top;
            }
        }
    }

    public sealed class BoundingRectangle : CollisionShape
    {
        public Vector2 Size { get; set; }

        public float Left => Position.X;
        public float Right => Position.X + Size.X;
        public float Top => Position.Y;
        public float Bottom => Position.Y + Size.Y;


        public BoundingRectangle(float x, float y, float width, float height)
        {
            Position = new Vector2(x, y);
            Size = new Vector2(width, height);
        }

        protected override bool CheckCollision(CollisionShape other, Vector2 offset)
        {
            if (other == null) return false;

            Position += offset;
            bool collision = false;
            
            if(other is BoundingRectangle br)
            {
                br.Position -= br.Origin;
                collision = CollisionResolver.Collides(this, br);
                br.Position += br.Origin;
            }

            Position -= offset;

            return collision;
        }
    }
}
