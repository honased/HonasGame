using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HonasGame.ECS
{
    public abstract class Component
    {
        protected Entity _parentEntity;

        public Component(Entity parent)
        {
            parent.RegisterComponent(this);
            _parentEntity = parent;
        }

        public virtual void Update(GameTime gameTime)
        {
            // Do nothing
        }

        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            // Do nothing
        }
    }
}
