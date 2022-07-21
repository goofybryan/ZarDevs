using System;
using System.Runtime.Serialization;

namespace ZarDevs.DependencyInjection
{
    /// <summary>
    /// Exception thrown when the <see cref="Type"/> is not found.
    /// </summary>
    [Serializable]
    public class TypeNotFoundException : Exception
    {
        #region Constructors

        /// <summary>
        /// Create a new instance of the <see cref="TypeForKeyNotFoundException"/>
        /// </summary>
        /// <param name="type">The type that cannot be found.</param>
        public TypeNotFoundException(Type type) : base($"Type '{type.Name}' could not be found.")
        {
            Type = type ?? throw new ArgumentNullException(nameof(type));
        }

        /// <inheritdoc/>
        protected TypeNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// The type that cannot be found.
        /// </summary>
        public Type Type { get; }

        #endregion Properties
    }
}