using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HonasGame.ECS
{
    public static class Scene
    {
        private static List<Entity> _entities = new List<Entity>();
        private static List<Entity> _addEntities = new List<Entity>();

        public static void AddEntity(Entity e)
        {
            _addEntities.Add(e);
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

            foreach (Entity e in _addEntities)
            {
                yield return e;
            }
        }

        public static void Update(GameTime gameTime)
        {
            Camera.Update(gameTime);

            while (_addEntities.Count > 0)
            {
                if(!_addEntities[0].Destroyed) _entities.Add(_addEntities[0]);
                _addEntities.RemoveAt(0);
            }

            for(int i = 0; i < _entities.Count; i++)
            {
                Entity e = _entities[i];
                if(e.Destroyed)
                {
                    _entities.RemoveAt(i);
                    i--;
                }
                else if(e.Enabled) e.Update(gameTime);
            }
        }

        public static void Clear(Entity exclude = null)
        {
            foreach(Entity e in _addEntities)
            {
                if(e != exclude) e.Destroy();
            }

            foreach(Entity e in _entities)
            {
                if (e != exclude) e.Destroy();
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
