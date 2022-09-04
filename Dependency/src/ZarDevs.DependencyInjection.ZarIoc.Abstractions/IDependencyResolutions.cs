using System.Collections.Generic;

namespace ZarDevs.DependencyInjection.ZarIoc
{
    /// <summary>
    /// Type resolution interface that will be used to wrap the generated implementations.
    /// </summary>
    public interface IDependencyResolutions : IResolution, IEnumerable<IDependencyResolution>
    {
        #region Methods

        /// <summary>
        /// Check if the resolution is empty.
        /// </summary>
        /// <returns></returns>
        bool IsEmpty { get; }

        /// <summary>
        /// Get the key for the resolution, null key is an empty key.
        /// </summary>
        IDependencyResolutions Filter(object key);

        /// <summary>
        /// Resolve it without any parameters
        /// </summary>
        /// <returns>The resolution.</returns>
        IEnumerable<object> ResolveAll();

        /// <summary>
        /// Resolve it with parameters
        /// </summary>
        /// <returns>The resolution.</returns>
        IEnumerable<object> ResolveAll(params object[] parameters);

        /// <summary>
        /// Resolve it with parameters
        /// </summary>
        /// <returns>The resolution.</returns>
        IEnumerable<object> ResolveAll(params (string key, object value)[] parameters);

        #endregion Methods
    }
}