using Microsoft.Xna.Framework;
using System;

namespace HonasGame.ECS.Components
{
    public struct Velocity2D
    {
        private Vector2 _velocity;
        private Vector2 _subVelocity;

        public float X
        {
            get => _velocity.X;
            set
            {
                _velocity.X = value;
            }
        }

        public float Y
        {
            get => _velocity.Y;
            set
            {
                _velocity.Y = value;
            }
        }
        
        public Vector2 CalculateVelocity(GameTime gameTime)
        {
            float t = (float)gameTime.ElapsedGameTime.TotalSeconds;
            _subVelocity += new Vector2(X * t, Y * t);
            Vector2 newVel = new Vector2((float)Math.Round(_subVelocity.X), (float)Math.Round(_subVelocity.Y));
            _subVelocity -= newVel;

            return newVel;
        }

        public void Set(Vector2 v)
        {
            _velocity = v;
        }

        public static Velocity2D operator +(Velocity2D v1, Velocity2D v2)
        {
            v1._velocity += v2._velocity;
            return v1;
        }

        public static Velocity2D operator *(Velocity2D v1, Velocity2D v2)
        {
            v1._velocity *= v2._velocity;
            return v1;
        }
    }
}
