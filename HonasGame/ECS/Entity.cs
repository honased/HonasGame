using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace HonasGame.ECS
{
    public abstract class Entity
    {
        private List<Component> _components;
        private List<Component> _removeComponents;

        public bool Enabled { get; set; } = true;

        public bool Destroyed { get; private set; } = false;

        public bool Persistent { get; protected set; } = false;

        public Entity()
        {
            _components = new List<Component>();
            _removeComponents = new List<Component>();
        }

        public virtual void Update(GameTime gameTime)
        {
            foreach(Component c in _components)
            {
                if(c.Enabled) c.Update(gameTime);
            }

            while(_removeComponents.Count > 0)
            {
                _components.Remove(_removeComponents[0]);
                _removeComponents.RemoveAt(0);
            }
        }

        public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            foreach (Component c in _components)
            {
                c.Draw(gameTime, spriteBatch);
            }
        }

        public void RegisterComponent(Component component)
        {
            _components.Add(component);
        }

        public void RemoveComponent(Component component)
        {
            _removeComponents.Add(component);
        }

        public IEnumerable<Component> GetComponents()
        {
            foreach(Component c in _components)
            {
                yield return c;
            }
        }

        public T GetComponent<T>() where T : Component
        {
            foreach(Component c in _components)
            {
                if(c is T actualComponent)
                {
                    return actualComponent;
                }
            }

            return null;
        }

        public void Destroy()
        {
            if (Destroyed) return;

            Enabled = false;
            Cleanup();
            foreach(Component c in GetComponents())
            {
                RemoveComponent(c);
            }
            Destroyed = true;
        }

        protected abstract void Cleanup();
    }
}
