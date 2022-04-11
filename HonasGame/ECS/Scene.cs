using System;
using System.Collections.Generic;
using HonasGame.Particles;
using HonasGame.Tiled;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;
using HonasGame.Helper;

namespace HonasGame.ECS
{
    public static class Scene
    {
        private static List<Entity> _entities = new List<Entity>();
        private static Dictionary<string, int> _layers = new Dictionary<string, int>();
        private static List<Entity> _addEntities = new List<Entity>();
        private static List<ParticleSystem> _particleSystems = new List<ParticleSystem>();

        public static void AddLayer(string layer, int depth)
        {
            if(_layers.ContainsKey(layer))
            {
                throw new System.Exception($"Layer '{layer}' already exists!");
            }

            _layers.Add(layer, depth);
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

        public static T GetParticleSystem<T>() where T : ParticleSystem
        {
            foreach(ParticleSystem ps in _particleSystems)
            {
                if (ps is T) return ps as T;
            }
            return default(T);
        }

        public static void AddEntity(Entity e, string layer = null)
        {
            if(layer != null)
            {
                if (!_layers.ContainsKey(layer)) throw new Exception($"Layer '{layer}' does not exist!");
                e.Depth = _layers[layer];
            }
            _addEntities.Add(e);
        }

        public static bool GetEntity<T>(out T entity) where T : Entity
        {
            entity = null;
            foreach (Entity e in _entities)
            {
                if (e is T en)
                {
                    entity = en;
                    return true;
                }
            }

            return false;
        }

        public static IEnumerable<Entity> GetEntities()
        {
            foreach (Entity e in _entities)
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
            Input.Update();
#if DEBUG
            Debugger.Update(gameTime);
#endif
            Camera.Update(gameTime);
            TiledManager.Update(gameTime);

            while (_addEntities.Count > 0)
            {
                if (!_addEntities[0].Destroyed)
                {
                    _entities.Add(_addEntities[0]);
                }
                _addEntities.RemoveAt(0);
            }

            // Insertion sort b/c the list will generally be sorted or almost sorted resulting in O(n).
            // Insertion is also inplace (preferred) and stable (necessary).
            Sorting.InsertionSort(_entities, x => x.Depth);

            for (int i = 0; i < _entities.Count; i++)
            {
                Entity e = _entities[i];
                if (e.Destroyed)
                {
                    _entities.RemoveAt(i);
                    i--;
                }
                else if (e.Enabled) e.Update(gameTime);
            }

            foreach (ParticleSystem ps in _particleSystems)
            {
                ps.Update(gameTime);
            }
        }

        public static void Clear(Entity exclude = null)
        {
            foreach(Entity e in _addEntities)
            {
                if(e != exclude && !e.Persistent) e.Destroy();
            }

            foreach (Entity e in _entities)
            {
                if (e != exclude && !e.Persistent) e.Destroy();
            }
        }

        public static void Draw(GameTime gameTime, SpriteBatch spriteBatch, Vector2 windowSize)
        {
            var mat = Camera.GetMatrix(windowSize);
            
            //spriteBatch.Begin(samplerState: SamplerState.PointClamp, transformMatrix: mat);
            foreach (Entity e in _entities)
            {
                e.Draw(gameTime, spriteBatch);
            }
            //spriteBatch.End();

            foreach (ParticleSystem ps in _particleSystems)
            {
                ps.Draw(spriteBatch, gameTime, mat);
            }
        }
    }
}
