using System;

namespace EIS.Runtime.Exceptions
{
    /// <summary>
    /// This exception is thrown when a component cannot be found based on an index.
    /// </summary>
    public class ComponentNotFoundException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the ComponentNotFoundException class with a specific error message.
        /// </summary>
        /// <param name="componentIndex">The index of the component that could not be found.</param>
        public ComponentNotFoundException(string componentIndex) :
            base($"Cannot found component with index {componentIndex}")
        {
        }
    }
}