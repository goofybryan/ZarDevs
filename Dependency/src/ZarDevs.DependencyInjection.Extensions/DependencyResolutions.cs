using System;
using System.Collections.Generic;

namespace ZarDevs.DependencyInjection
{
    /// <summary>
    /// Interface for resolving multiple resolutions.
    /// </summary>
    public interface IDependencyResolutions
    {
        /// <summary>
        /// Resolve and resolutions and return as <typeparamref name="T"/>
        /// </summary>
        /// <typeparam name="T">The return type to resolve to.</typeparam>
        /// <returns>An IEnumerable of the resolved values cast to <typeparamref name="T"/></returns>
        IEnumerable<T> Resolve<T>();

        /// <summary>
        /// Resolve and resolutions and return
        /// </summary>
        /// <returns>An IEnumerable of the resolved values</returns>
        IEnumerable<object> Resolve();
    }

    internal class DependencyResolutions : IDependencyResolutions
    {
        private readonly IEnumerable<IDependencyResolution> _resolutions;

        public DependencyResolutions(IEnumerable<IDependencyResolution> resolutions)
        {
            _resolutions = resolutions ?? throw new ArgumentNullException(nameof(resolutions));
        }

        public IEnumerable<T> Resolve<T>()
        {
            foreach(var resolution in _resolutions)
            {
                yield return (T)resolution.Resolve();
            }
        }

        public IEnumerable<object> Resolve()
        {
            foreach (var resolution in _resolutions)
            {
                yield return resolution.Resolve();
            }
        }
    }
}