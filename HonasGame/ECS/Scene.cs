using System;
using System.Collections.Generic;
using HonasGame.Particles;
using HonasGame.Tiled;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace HonasGame.ECS
{
    public static class Scene
    {
        private static List<List<Entity>> _entities = new List<List<Entity>>();
        private static List<string> _layers = new List<string>();
        private static List<Tuple<string, Entity>> _addEntities = new List<Tuple<string, Entity>>();
        private static List<ParticleSystem> _particleSystems = new List<ParticleSystem>();

        public static void AddLayer(string layer)
        {
            if(_layers.Contains(layer))
            {
                throw new System.Exception($"Layer '{layer}' already exists!");
            }

            _layers.Add(layer);
            _entities.Add(new List<Entity>());
        }

        public static void ClearLayers()
        {
            _layers.Clear();
        }

        public static void AddParticleSystem(ParticleSystem system)
        {
            _particleSystems.Add(system);
            _particleSystems.Sort((x, y) =>
            {
                return x.DrawOrder.CompareTo(y.DrawOrder);
            });
        }

        public static void RemoveParticleSystem(ParticleSystem system)
        {
            _particleSystems.Remove(system);
        }

        public static void AddEntity(Entity e, string layer = null)
        {
            if (layer == null) layer = _layers[0];
            _addEntities.Add(new Tuple<string, Entity>(layer, e));
        }

        public static T GetEntity<T>() where T : Entity
        {
            foreach(List<Entity> le in _entities)
            {
                foreach (Entity e in le)
                {
                    if (e is T entity)
                    {
                        return entity;
                    }
                }
            }

            return null;
        }

        public static IEnumerable<Entity> GetEntities()
        {
            foreach (List<Entity> le in _entities)
            {
                foreach (Entity e in le)
                {
                    yield return e;
                }
            }

            foreach (Tuple<string, Entity> tuple in _addEntities)
            {
                yield return tuple.Item2;
            }
        }

        public static void Update(GameTime gameTime)
        {
            Input.Update();
#if DEBUG
            Debugger.Update(gameTime);
#endif
            Camera.Update(gameTime);
            TiledManager.Update(gameTime);

            while (_addEntities.Count > 0)
            {
                if (!_addEntities[0].Item2.Destroyed)
                {
                    string layer = _addEntities[0].Item1;
                    int index = _layers.IndexOf(layer);
                    if (index == -1) throw new Exception($"Layer '{layer}' does not exist!");
                    _entities[index].Add(_addEntities[0].Item2);
                }
                _addEntities.RemoveAt(0);
            }

            for (int j = 0; j < _entities.Count; j++)
            {
                List<Entity> entitiesList = _entities[j];
                for (int i = 0; i < entitiesList.Count; i++)
                {
                    Entity e = entitiesList[i];
                    if (e.Destroyed)
                    {
                        entitiesList.RemoveAt(i);
                        i--;
                    }
                    else if (e.Enabled) e.Update(gameTime);
                }
            }

            foreach(ParticleSystem ps in _particleSystems)
            {
                ps.Update(gameTime);
            }
        }

        public static void Clear(Entity exclude = null)
        {
            foreach(var tuple in _addEntities)
            {
                if(tuple.Item2 != exclude && !tuple.Item2.Persistent) tuple.Item2.Destroy();
            }

            for (int i = 0; i < _layers.Count; i++)
            {
                foreach (Entity e in _entities[i])
                {
                    if (e != exclude && !e.Persistent) e.Destroy();
                }
            }
        }

        public static void Draw(GameTime gameTime, SpriteBatch spriteBatch, Vector2 windowSize)
        {
            var mat = Camera.GetMatrix(windowSize);
            
            spriteBatch.Begin(samplerState: SamplerState.PointClamp, transformMatrix: mat);
            for (int i = 0; i < _layers.Count; i++)
            {
                foreach (Entity e in _entities[i])
                {
                    e.Draw(gameTime, spriteBatch);
                }
            }
            spriteBatch.End();

            foreach (ParticleSystem ps in _particleSystems)
            {
                ps.Draw(spriteBatch, gameTime, mat);
            }
        }
    }
}
