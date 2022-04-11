using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;

namespace HonasGame.ECS.Components
{
    public class Transform3D : Component
    {
        public Vector3 Position { get; set; } = Vector3.Zero;

        public Transform3D(Entity parent) : base(parent)
        {

        }
    }
}
