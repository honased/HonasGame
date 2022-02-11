using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HonasGame.ECS
{
    public static class Scene
    {
        private static List<Entity> _entities = new List<Entity>();
        private static List<Entity> _destroyEntities = new List<Entity>();
        private static List<Entity> _addEntities = new List<Entity>();
        private static bool _clearEntities = false;
        private static Entity _saveEntity = null;

        public static void AddEntity(Entity e)
        {
            _addEntities.Add(e);
        }

        public static void RemoveEntity(Entity e)
        {
            _destroyEntities.Add(e);
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
                _entities.Add(_addEntities[0]);
                _addEntities.RemoveAt(0);
            }

            foreach (Entity e in _entities)
            {
                if(e.Enabled) e.Update(gameTime);
            }

            if(_clearEntities)
            {
                for (int i = 0; i < _addEntities.Count; i++)
                {
                    if (_addEntities[i] == _saveEntity) continue;
                    _addEntities.RemoveAt(i);
                    i--;
                }

                for (int i = 0; i < _entities.Count; i++)
                {
                    if (_entities[i] == _saveEntity) continue;
                    _entities.RemoveAt(i);
                    i--;
                }

                _saveEntity = null;
                _clearEntities = false;
            }

            while(_destroyEntities.Count > 0)
            {
                _entities.Remove(_destroyEntities[0]);
                _addEntities.Remove(_destroyEntities[0]);
                _destroyEntities.RemoveAt(0);
            }
        }

        public static void Clear(Entity exclude = null)
        {
            _clearEntities = true;
            _saveEntity = exclude;
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
