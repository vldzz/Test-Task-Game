using System;
using System.Collections.Generic;
using System.Linq;
using EIS.Runtime.Exceptions;
using UnityEngine;

namespace EIS.Runtime.Misc
{
    /// <summary>
    /// Provides a way to efficiently retrieve components by index, using caching for performance.
    /// </summary>
    public class CachedComponentProvider : MonoBehaviour
    {
        /// <summary>
        /// List of cached components, each containing an index, GameObject reference, and cached component references.
        /// </summary>
        [Tooltip(
            "List of cached components, each containing an index, GameObject reference, and cached component references")]
        [SerializeField]
        private List<CachedComponent> cachedComponents = new List<CachedComponent>();

        /// <summary>
        /// Structure to store cached component information for a specific index.
        /// </summary>
        [Serializable]
        public class CachedComponent
        {
            /// <summary>
            /// String identifier for the cached component.
            /// </summary>
            [Tooltip("String identifier for the cached component")]
            public string index;

            /// <summary>
            /// GameObject that the cached components are attached to.
            /// </summary>
            [Tooltip("GameObject that the cached components are attached to")]
            public GameObject go;

            /// <summary>
            /// Dictionary to store cached components, keyed by their component types.
            /// </summary>
            [Tooltip("Dictionary to store cached components, keyed by their component types")]
            public Dictionary<Type, Component> cachedComponents = new Dictionary<Type, Component>();
        }

        /// <summary>
        /// Retrieves a component of the specified type by its index, using caching for performance.
        /// </summary>
        /// <typeparam name="T">Type of the component to retrieve.</typeparam>
        /// <param name="index">String index of the component to retrieve.</param>
        /// <returns>The retrieved component of type T.</returns>
        /// <exception cref="ComponentNotFoundException">Thrown if a component with the specified index is not found.</exception>
        public T GetComponentByIndex<T>(string index) where T : Component
        {
            CachedComponent cachedComponent = cachedComponents.FirstOrDefault(ic => ic.index.Equals(index));

            if (cachedComponent == null)
            {
                throw new ComponentNotFoundException(index);
            }

            if (cachedComponent.cachedComponents.ContainsKey(typeof(T)))
            {
                return (T)cachedComponent.cachedComponents[typeof(T)];
            }

            Component component = cachedComponent.go.GetComponent<T>();
            if (component == null)
            {
                throw new ComponentNotFoundException(index);
            }

            cachedComponent.cachedComponents[typeof(T)] = component;

            return (T)component;
        }
    }
}