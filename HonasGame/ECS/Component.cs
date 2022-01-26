using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HonasGame.ECS
{
    public abstract class Component
    {
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
