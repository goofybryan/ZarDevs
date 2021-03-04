using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

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
        IEnumerable Resolve();
    }

    internal class DependencyResolutions : IDependencyResolutions
    {
        private readonly Type _requestType;
        private readonly IList<IDependencyResolution> _resolutions;

        public DependencyResolutions(Type requestType, IList<IDependencyResolution> resolutions)
        {
            _requestType = requestType ?? throw new ArgumentNullException(nameof(requestType));
            _resolutions = resolutions ?? throw new ArgumentNullException(nameof(resolutions));
        }

        public IEnumerable<T> Resolve<T>()
        {
            return YieldResolve<T>().ToArray();
        }

        public IEnumerable Resolve()
        {
            return YieldResolve();
        }

        private IEnumerable<T> YieldResolve<T>()
        {
            foreach (var resolution in _resolutions)
            {
                yield return (T)resolution.Resolve();
            }
        }

        private IEnumerable YieldResolve()
        {
            Array resolved = Array.CreateInstance(_requestType, _resolutions.Count);

            for (int i = 0; i < _resolutions.Count; i++)
            {
                IDependencyResolution resolution = _resolutions[i];
                resolved.SetValue(resolution.Resolve(), i);
            }

            return resolved;
        }
    }
}