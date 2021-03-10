using System;
using System.Collections;
using System.Collections.Generic;

namespace ZarDevs.DependencyInjection
{
    /// <summary>
    /// Interface for resolving multiple resolutions.
    /// </summary>
    public interface IDependencyResolutions
    {
        #region Methods

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

        #endregion Methods
    }

    internal class DependencyResolutions : IDependencyResolutions
    {
        #region Fields

        private readonly Type _requestType;
        private readonly IList<IDependencyResolution> _resolutions;

        #endregion Fields

        #region Constructors

        public DependencyResolutions(Type requestType, IList<IDependencyResolution> resolutions)
        {
            _requestType = requestType ?? throw new ArgumentNullException(nameof(requestType));
            _resolutions = resolutions ?? throw new ArgumentNullException(nameof(resolutions));
        }

        #endregion Constructors

        #region Methods

        public IEnumerable<T> Resolve<T>()
        {
            return (IEnumerable<T>)YieldResolve();
        }

        public IEnumerable Resolve()
        {
            return YieldResolve();
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

        #endregion Methods
    }
}