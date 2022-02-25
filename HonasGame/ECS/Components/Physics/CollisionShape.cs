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

            public static bool Collides(BoundingRectangle a, BoundingCircle b)
            {
                float nearestX = MathHelper.Clamp(b.Center.X, a.Left, a.Right);
                float nearestY = MathHelper.Clamp(b.Center.Y, a.Top, a.Bottom);
                return Math.Pow(b.Radius, 2) >= Math.Pow(b.Center.X - nearestX, 2) + Math.Pow(b.Center.Y - nearestY, 2);
            }

            public static bool Collides(BoundingCircle a, BoundingCircle b)
            {
                return Math.Pow(a.Radius + b.Radius, 2) >= Math.Pow(a.Center.X - b.Center.X, 2) + Math.Pow(a.Center.Y - b.Center.Y, 2);
            }
        }
    }

    public sealed class BoundingRectangle : CollisionShape
    {
        public Vector2 Size { get; set; }

        public float Left => Position.X - Origin.X;
        public float Right => Position.X + Size.X - Origin.X;
        public float Top => Position.Y - Origin.Y;
        public float Bottom => Position.Y + Size.Y - Origin.Y;


        public BoundingRectangle(float x, float y, float width, float height)
        {
            Position = new Vector2(x, y);
            Size = new Vector2(width, height);
        }

        protected override bool CheckCollision(CollisionShape other, Vector2 offset)
        {
            if (other == null) return false;

            bool collision = false;
            
            if(other is BoundingRectangle br)
            {
                collision = CollisionResolver.Collides(this, br);
            }
            else if(other is BoundingCircle bc)
            {
                collision = CollisionResolver.Collides(this, bc);
            }

            return collision;
        }
    }

    public class BoundingCircle : CollisionShape
    {
        public float Radius { get; set; }

        public Vector2 Center => Position - Origin;

        public BoundingCircle(float x, float y, float radius)
        {
            Position = new Vector2(x, y);
            Radius = radius;
        }

        protected override bool CheckCollision(CollisionShape other, Vector2 offset)
        {
            if (other == null) return false;
            bool collision = false;

            if (other is BoundingRectangle br)
            {
                collision = CollisionResolver.Collides(br, this);
            }
            else if (other is BoundingCircle bc)
            {
                collision = CollisionResolver.Collides(this, bc);
            }

            return collision;
        }
    }
}
