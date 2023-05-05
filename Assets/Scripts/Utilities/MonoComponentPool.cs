using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

namespace Utilities
{
    public class MonoComponentPool<T> where T : Component
    {
        private List<T> _unusedComponents;
        private GameObject _goAnchor;

        public MonoComponentPool(GameObject goAnchor)
        {
            _goAnchor = goAnchor;
            _unusedComponents = _goAnchor.GetComponents<T>().ToList();
        }

        public T Borrow()
        {
            if (_unusedComponents.Count == 0)
            {
                var newComponent = _goAnchor.AddComponent<T>();
                return newComponent;
            }

            var component = _unusedComponents.Last();
            _unusedComponents.Remove(component);
            return component;
        }

        public void Return(T component)
        {
            if (component.gameObject != _goAnchor)
            {
                throw new ArgumentException(nameof(component));
            }

            _unusedComponents.Add(component);
        }
    }
}
