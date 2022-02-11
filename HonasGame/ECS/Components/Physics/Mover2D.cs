using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace HonasGame.ECS.Components.Physics
{
    public class Mover2D : Component
    {
        private Collider2D _collider;
        private Transform2D _transform;
        public Mover2D(Entity parent) : base(parent)
        {
            _collider = Parent.GetComponent<Collider2D>();
            _transform = Parent.GetComponent<Transform2D>();
        }

        public bool MoveX(float velocity, int tag)
        {
            int xAmount = (int)Math.Round(velocity);

            int sign = Math.Sign(xAmount);
            while(!_collider.CollidesWith(tag, Vector2.UnitX * sign, out var e) && xAmount != 0) 
            { 
                xAmount -= sign;
                _transform.Position += Vector2.UnitX * sign;
                _collider.Shape.Position = _transform.Position;
            }

            return xAmount != 0;
        }

        public bool MoveY(float velocity, int tag)
        {
            int yAmount = (int)Math.Round(velocity);

            int sign = Math.Sign(yAmount);
            while (!_collider.CollidesWith(tag, Vector2.UnitY * sign, out var e) && yAmount != 0)
            {
                yAmount -= sign;
                _transform.Position += Vector2.UnitY * sign;
                _collider.Shape.Position = _transform.Position;
            }

            return yAmount != 0;
        }

        public override void Update(GameTime gameTime)
        {
            _collider.Shape.Position = _transform.Position;
        }
    }
}
