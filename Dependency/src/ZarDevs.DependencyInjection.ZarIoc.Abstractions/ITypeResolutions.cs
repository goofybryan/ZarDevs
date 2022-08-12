using System.Collections.Generic;

namespace ZarDevs.DependencyInjection
{
    /// <summary>
    /// Type resolution interface that will be used to wrap the generated implementations.
    /// </summary>
    public interface ITypeResolutions : IEnumerable<ITypeResolution>
    {
        /// <summary>
        /// Get the key for the resolution, null key is an empty key.
        /// </summary>
        ITypeResolutions Filter(object key);

        /// <summary>
        /// Resolve it without any parameters
        /// </summary>
        /// <returns>The resolution.</returns>
        IEnumerable<object> Resolve();

        /// <summary>
        /// Resolve it with parameters
        /// </summary>
        /// <returns>The resolution.</returns>
        IEnumerable<object> Resolve(params object[] parameters);

        /// <summary>
        /// Resolve it with parameters
        /// </summary>
        /// <returns>The resolution.</returns>
        IEnumerable<object> Resolve(params (string key, object value)[] parameters);
    }
}
