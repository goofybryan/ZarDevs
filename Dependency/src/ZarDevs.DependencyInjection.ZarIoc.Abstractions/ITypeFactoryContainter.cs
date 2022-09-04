using System;
using System.Collections.Generic;

namespace ZarDevs.DependencyInjection.ZarIoc
{
    /// <summary>
    /// Type factory container. Used to manage the list of <see cref="IDependencyResolution"/> resolutions
    /// for a type and key.
    /// </summary>
    public interface ITypeFactoryContainter : IEnumerable<Type>
    {
        #region Methods

        /// <summary>
        /// Add the <paramref name="resolution"/> for the type without a key.
        /// </summary>
        /// <param name="type">The type to add.</param>
        /// <param name="resolution">The resolution it is for.</param>
        public void Add(Type type, IDependencyResolution resolution);

        /// <summary>
        /// Find all resolutions for the <paramref name="type"/>. This is essentially a <see cref="TryGet(Type, object, out IDependencyResolutions)"/>
        /// </summary>
        /// <param name="type">Specify the type to get a resolution for.</param>
        /// <param name="key">Specify the key for the resolution.</param>
        /// <returns>
        /// A <see cref="IDependencyResolutions"/> object contain all the <see cref="IDependencyResolution"/> resolutions or null if not found.
        /// </returns>
        public IResolution Find(Type type, object key);

        /// <summary>
        /// Get all resolutions for the <paramref name="type"/>
        /// </summary>
        /// <param name="type">Specify the type to get a resolution for.</param>
        /// <returns>
        /// A <see cref="IDependencyResolutions"/> object contain all the <see cref="IDependencyResolution"/> resolutions.
        /// </returns>
        /// <exception cref="TypeNotFoundException">
        /// Thrown when the <paramref name="type"/> is not found.
        /// </exception>
        public IResolution Get(Type type);

        /// <summary>
        /// Get all resolutions for the <paramref name="type"/> and <paramref name="key"/>
        /// </summary>
        /// <param name="type">Specify the type to get a resolution for.</param>
        /// <param name="key">Specify the key for the resolution.</param>
        /// <returns>
        /// A <see cref="IDependencyResolutions"/> object contain all the <see cref="IDependencyResolution"/> resolutions.
        /// </returns>
        /// <exception cref="TypeForKeyNotFoundException">
        /// Thrown when the <paramref name="type"/> and <paramref name="key"/> is not found.
        /// </exception>
        public IResolution Get(Type type, object key);

        /// <summary>
        /// Get all resolutions for the <paramref name="type"/>
        /// </summary>
        /// <param name="type">Specify the type to get a resolution for.</param>
        /// <param name="resolution">
        /// A <see cref="IResolution"/> object contain all the <see cref="IDependencyResolution"/> resolutions.
        /// </param>
        /// <returns>Returns a true when the resolutions are found.</returns>
        public bool TryGet(Type type, out IResolution resolution);

        /// <summary>
        /// Get all resolutions for the <paramref name="type"/> and <paramref name="key"/>
        /// </summary>
        /// <param name="type">Specify the type to get a resolution for.</param>
        /// <param name="key">Specify the key for the resolution.</param>
        /// <param name="resolution">
        /// A <see cref="IResolution"/> object contain all the <see cref="IDependencyResolution"/> resolutions.
        /// </param>
        /// <returns>Returns a true when the resolutions are found.</returns>
        public bool TryGet(Type type, object key, out IResolution resolution);

        #endregion Methods
    }
}