using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace ZarDevs.DependencyInjection
{
    /// <summary>
    /// Type resolution interface that will be used to wrap the generated implementations.
    /// </summary>
    public interface ITypeResolution
    {
        /// <summary>
        /// Get the key for the resolution, null key is an empty key.
        /// </summary>
        object Key { get; }

        /// <summary>
        /// Resolve it without any parameters
        /// </summary>
        /// <returns>The resolution.</returns>
        object Resolve();

        /// <summary>
        /// Resolve it with parameters
        /// </summary>
        /// <returns>The resolution.</returns>
        object Resolve(params object[] parameters);

        /// <summary>
        /// Resolve it with parameters
        /// </summary>
        /// <returns>The resolution.</returns>
        object Resolve(params (string, object)[] parameters);
    }
}
