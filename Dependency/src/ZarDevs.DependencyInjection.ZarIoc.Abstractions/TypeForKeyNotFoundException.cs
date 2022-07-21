using System;
using System.Runtime.Serialization;

namespace ZarDevs.DependencyInjection
{
    /// <summary>
    /// Exception thrown when the <see cref="Type"/> and <see cref="Key"/> is not found.
    /// </summary>
    [Serializable]
    public class TypeForKeyNotFoundException : Exception
    {
        #region Constructors

        /// <summary>
        /// Create a new instance of the <see cref="TypeForKeyNotFoundException"/>
        /// </summary>
        /// <param name="type">The type that cannot be found.</param>
        /// <param name="key">The key that cannot be found.</param>
        public TypeForKeyNotFoundException(Type type, object key) : base($"Type '{type.Name}' for key '{key}' could not be found.")
        {
            Type = type ?? throw new ArgumentNullException(nameof(type));
            Key = key ?? throw new ArgumentNullException(nameof(key));
        }

        /// <inheritdoc/>
        protected TypeForKeyNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// The key that cannot be found.
        /// </summary>
        public object Key { get; }

        /// <summary>
        /// The type that cannot be found.
        /// </summary>
        public Type Type { get; }

        #endregion Properties
    }
}