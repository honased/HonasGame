using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace HonasGame.ECS.Components.Physics
{
    public abstract class CollisionShape
    {
        public Vector2 Position { get; set; }
        public Vector2 Offset { get; set; } = Vector2.Zero;
        public abstract float Left { get; }
        public abstract float Right { get; }
        public abstract float Top { get; }
        public abstract float Bottom { get; }

        protected abstract bool CheckCollision(CollisionShape other, Vector2 offset);
        public bool CollidesWith(CollisionShape other)
        {
            return CheckCollision(other, Vector2.Zero);
        }

        public bool CollidesWith(CollisionShape other, Vector2 offset)
        {
            return CheckCollision(other, offset);
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

        public override float Left => Position.X + Offset.X;
        public override float Right => Position.X + Size.X + Offset.X;
        public override float Top => Position.Y + Offset.Y;
        public override float Bottom => Position.Y + Size.Y + Offset.Y;


        public BoundingRectangle(float width, float height)
        {
            Size = new Vector2(width, height);
        }

        protected override bool CheckCollision(CollisionShape other, Vector2 offset)
        {
            if (other == null) return false;
            bool collision = false;
            Vector2 origPos = Position;

            Position += offset;

            if (other is BoundingRectangle br)
            {
                collision = CollisionResolver.Collides(this, br);
            }
            else if(other is BoundingCircle bc)
            {
                collision = CollisionResolver.Collides(this, bc);
            }

            Position = origPos;

            return collision;
        }
    }

    public class BoundingCircle : CollisionShape
    {
        public float Radius { get; set; }

        public Vector2 Center => Position + Offset;

        public override float Left => Position.X + Offset.X - Radius;
        public override float Right => Position.X + Offset.X + Radius;
        public override float Top => Position.Y + Offset.Y - Radius;
        public override float Bottom => Position.Y + Offset.Y + Radius;

        public BoundingCircle(float radius)
        {
            Radius = radius;
        }

        protected override bool CheckCollision(CollisionShape other, Vector2 offset)
        {
            if (other == null) return false;
            bool collision = false;
            Vector2 origPos = Position;

            Position += offset;

            if (other is BoundingRectangle br)
            {
                collision = CollisionResolver.Collides(br, this);
            }
            else if (other is BoundingCircle bc)
            {
                collision = CollisionResolver.Collides(this, bc);
            }

            Position = origPos;

            return collision;
        }
    }
}
