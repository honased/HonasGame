using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HonasGame.ECS
{
    public static class Scene
    {
        private static List<Entity> _entities = new List<Entity>();

        public static void AddEntity(Entity e)
        {
            _entities.Add(e);
        }

        public static void RemoveEntity(Entity e)
        {
            _entities.Remove(e);
        }

        public static T GetEntity<T>() where T : Entity
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

        public static IEnumerable<Entity> GetEntities()
        {
            foreach(Entity e in _entities)
            {
                yield return e;
            }
        }

        public static void Update(GameTime gameTime)
        {
            foreach(Entity e in _entities)
            {
                if(e.Enabled) e.Update(gameTime);
            }
        }

        public static void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            foreach(Entity e in _entities)
            {
                e.Draw(gameTime, spriteBatch);
            }
        }
    }
}
