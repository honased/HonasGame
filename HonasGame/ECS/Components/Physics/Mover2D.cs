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
            Parent.GetComponent<Collider2D>(out _collider);
            Parent.GetComponent<Transform2D>(out _transform);
        }

        public Entity MoveX(float velocity, uint tag)
        {
            _collider.Shape.Position = _transform.Position;
            int xAmount = (int)Math.Round(velocity);

            int sign = Math.Sign(xAmount);
            Entity e = null;
            while(!_collider.CollidesWith(tag, Vector2.UnitX * sign, out e) && xAmount != 0) 
            { 
                xAmount -= sign;
                _transform.Position += Vector2.UnitX * sign;
                _collider.Shape.Position = _transform.Position;
            }

            return e;
        }

        public Entity MoveY(float velocity, uint tag, uint oneWayTag = 0)
        {
            _collider.Shape.Position = _transform.Position;
            int yAmount = (int)Math.Round(velocity);

            int sign = Math.Sign(yAmount);
            tag |= oneWayTag;
            Entity e = null;
            while (yAmount != 0)
            {
                if(_collider.CollidesWith(tag, Vector2.UnitY * sign, out e))
                {
                    if (e.GetComponent<Collider2D>(out var eCollider) && (eCollider.Tag & oneWayTag) > 0)
                    {
                        if (_collider.Shape.Bottom <= eCollider.Shape.Top) break;
                    }
                    else break;
                }
                yAmount -= sign;
                _transform.Position += Vector2.UnitY * sign;
                _collider.Shape.Position = _transform.Position;
            }

            return e;
        }

        public override void Update(GameTime gameTime)
        {
            _collider.Shape.Position = _transform.Position;
        }
    }
}
