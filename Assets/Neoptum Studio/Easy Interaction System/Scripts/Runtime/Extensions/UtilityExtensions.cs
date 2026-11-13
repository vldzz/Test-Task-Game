using System;
using UnityEngine;

namespace EIS.Runtime.Extensions
{
    /// <summary>
    /// This static class provides utility extension methods for common operations.
    /// </summary>
    public static class UtilityExtensions
    {
        /// <summary>
        /// Checks if a LayerMask contains a specific layer.
        /// </summary>
        /// <param name="layermask">The LayerMask to check.</param>
        /// <param name="layer">The layer ID to check for.</param>
        /// <returns>True if the LayerMask contains the specified layer, false otherwise.</returns>
        public static bool ContainsLayer(this LayerMask layermask, int layer)
        {
            return layermask == (layermask | (1 << layer));
        }

        /// <summary>
        /// Checks if a string is null or empty.
        /// </summary>
        /// <param name="toCheck">The string to check.</param>
        /// <returns>True if the string is null or empty, false otherwise.</returns>
        public static bool IsNullOrEmpty(this string toCheck)
        {
            return String.IsNullOrEmpty(toCheck);
        }
    }
}