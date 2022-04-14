using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HonasGame.ECS
{
    public abstract class Component
    {
        protected Entity Parent { get; private set; }

        public bool Enabled { get; set; } = true;

        public bool Visible { get; set; } = true;

        public Component(Entity parent)
        {
            parent.RegisterComponent(this);
            Parent = parent;
        }

        public virtual void Update(GameTime gameTime)
        {
            // Do nothing
        }

        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            // Do nothing
        }

        public virtual void Draw3D(GameTime gameTime, SpriteBatch spriteBatch)
        {
            // Do nothing
        }
    }
}
