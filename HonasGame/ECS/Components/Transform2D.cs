using Microsoft.Xna.Framework;

namespace HonasGame.ECS.Components
{
    public class Transform2D : Component
    {
        public Vector2 Position { get; set; } = Vector2.Zero;

        public Transform2D(Entity parent) : base(parent)
        {

        }
    }
}
