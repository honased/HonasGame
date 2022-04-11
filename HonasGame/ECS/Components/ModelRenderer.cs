using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace HonasGame.ECS.Components
{
    public class ModelRenderer : Component
    {
        public Model Model { get; set; }

        public ModelRenderer(Entity parent) : base(parent)
        {

        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            var mat = Matrix.CreateRotationY(MathHelper.PiOver4 / 2.0f);
            mat *= Matrix.CreateTranslation(0, -1, 5f);
            Model.Draw(mat, Matrix.CreateLookAt(new Vector3(0, 0, 0), new Vector3(0, 0, 1), Vector3.UnitY), Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(90.0f), 1920.0f / 1080.0f, 0.01f, 100.0f));
        }
    }
}
