using System;
using System.Collections.Generic;
using System.Text;

namespace HonasGame.ECS
{
    public abstract class Entity
    {
        private List<Component> _components;

        public Entity()
        {
            _components = new List<Component>();
        }

        public void RegisterComponent(Component component)
        {
            _components.Add(component);
        }

        public void RemoveComponent(Component component)
        {
            _components.Remove(component);
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
    }
}
