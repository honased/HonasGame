using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HonasGame.ECS
{
    public class Scene
    {
        private List<Entity> _entities;

        public Scene()
        {
            _entities = new List<Entity>();
        }

        public void AddEntity(Entity e)
        {
            _entities.Add(e);
        }

        public void RemoveEntity(Entity e)
        {
            _entities.Remove(e);
        }

        public T GetEntity<T>() where T : Entity
        {
            foreach(Entity e in _entities)
            {
                if(e is T entity)
                {
                    return entity;
                }
            }

            return null;
        }

        public void Update(GameTime gameTime)
        {
            foreach(Entity e in _entities)
            {
                e.Update(gameTime);
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            foreach(Entity e in _entities)
            {
                e.Draw(gameTime, spriteBatch);
            }
        }
    }
}
