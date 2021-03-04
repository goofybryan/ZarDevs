using System;
using System.Collections.Generic;
using System.Linq;

namespace ZarDevs.DependencyInjection
{
    /// <summary>
    /// Dependency instance resolution
    /// </summary>
    public interface IDependencyInstanceResolution : IDisposable
    {
        #region Methods

        /// <summary>
        /// Get all the resolution for the request type.
        /// </summary>
        /// <param name="requestType">The request type that will need to be resolved.</param>
        /// <returns>
        /// The resolved instance, if not found an empty <see
        /// cref="Enumerable.Empty{IDependencyResolution}"/> will be returned.
        /// </returns>
        IDependencyResolutions GetAllResolutions(Type requestType);

        /// <summary>
        /// Get all the resolution for the request type.
        /// </summary>
        /// <param name="requestType">The request type that will need to be resolved.</param>
        /// <param name="key">The key for this resolution. A null value is also considered a key.</param>
        /// <returns>
        /// The resolved instance, if not found an empty <see
        /// cref="Enumerable.Empty{IDependencyResolution}"/> will be returned.
        /// </returns>
        IDependencyResolutions GetAllResolutions(Type requestType, object key);

        /// <summary>
        /// Get the resolution for the request type.
        /// </summary>
        /// <param name="requestType">The request type that will need to be resolved.</param>
        /// <returns>
        /// The resolved instance, if not found a <see cref="NotFoundDependencyResolution"/> that
        /// will throw an <see cref="DependencyResolutionNotConfiguredException"/>.
        /// </returns>
        IDependencyResolution GetResolution(Type requestType);

        /// <summary>
        /// Get the resolution for the request type.
        /// </summary>
        /// <param name="requestType">The request type that will need to be resolved.</param>
        /// <param name="key">The key for this resolution.</param>
        /// <returns>
        /// The resolved instance, if not found a <see cref="NotFoundDependencyResolution"/> that
        /// will throw an <see cref="DependencyResolutionNotConfiguredException"/>.
        /// </returns>
        IDependencyResolution GetResolution(object key, Type requestType);

        /// <summary>
        /// Try and get the resolution for the request type.
        /// </summary>
        /// <param name="requestType">The request type that will need to be resolved.</param>
        /// <returns>
        /// The resolved instance, if not found a <see cref="NotFoundDependencyResolution"/> that
        /// will return null when resolved.
        /// </returns>
        IDependencyResolution TryGetResolution(Type requestType);

        /// <summary>
        /// Get the resolution for the request type.
        /// </summary>
        /// <param name="requestType">The request type that will need to be resolved.</param>
        /// <param name="key">The key for this resolution.</param>
        /// <returns>
        /// The resolved instance, if not found a <see cref="NotFoundDependencyResolution"/> that
        /// will return null when resolved.
        /// </returns>
        IDependencyResolution TryGetResolution(object key, Type requestType);

        #endregion Methods
    }

    /// <summary>
    /// Dependency resolution configuration is used to configure any dependency resolutions and get
    /// them them at runtime.
    /// </summary>
    public class DependencyInstanceResolution : IDependencyInstanceResolution
    {
        #region Fields

        private readonly IDependencyResolutionConfiguration _configuration;
        private bool _disposed;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Create a new instance of the Dependency resolution configuration.
        /// </summary>
        public DependencyInstanceResolution(IDependencyResolutionConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Dispose of the underlying resources. If any <see cref="IDependencyResolution"/>
        /// implements <see cref="IDisposable"/> that will be called.
        /// </summary>
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Get all the resolution for the request type.
        /// </summary>
        /// <param name="requestType">The request type that will need to be resolved.</param>
        /// <returns>
        /// The resolved instance, if not found an empty <see
        /// cref="Enumerable.Empty{IDependencyResolution}"/> will be returned.
        /// </returns>
        public IDependencyResolutions GetAllResolutions(Type requestType)
        {
            return new DependencyResolutions(requestType, _configuration.GetResolutionsByType(requestType));
        }

        /// <summary>
        /// Get all the resolution for the request type.
        /// </summary>
        /// <param name="requestType">The request type that will need to be resolved.</param>
        /// <param name="key">The key for this resolution. A null value is also considered a key and will return only resolutions that have no key configured.</param>
        /// <returns>
        /// The resolved instance, if not found an empty <see
        /// cref="Enumerable.Empty{IDependencyResolution}"/> will be returned.
        /// </returns>
        public IDependencyResolutions GetAllResolutions(Type requestType, object key)
        {
            return new DependencyResolutions(requestType, _configuration.GetResolutionsByKey(requestType, key));
        }

        /// <summary>
        /// Get the resolution for the request type.
        /// </summary>
        /// <param name="requestType">The request type that will need to be resolved.</param>
        /// <param name="key">The key for this resolution.</param>
        /// <returns>
        /// The resolved instance, if not found a <see cref="NotFoundDependencyResolution"/> that
        /// will throw an <see cref="DependencyResolutionNotConfiguredException"/>.
        /// </returns>
        public IDependencyResolution GetResolution(object key, Type requestType)
        {
            return TryGetResolution(requestType, key, true);
        }

        /// <summary>
        /// Get the resolution for the request type.
        /// </summary>
        /// <param name="requestType">The request type that will need to be resolved.</param>
        /// <returns>
        /// The resolved instance, if not found a <see cref="NotFoundDependencyResolution"/> that
        /// will throw an <see cref="DependencyResolutionNotConfiguredException"/>.
        /// </returns>
        public IDependencyResolution GetResolution(Type requestType)
        {
            return TryGetResolution(requestType, true);
        }

        /// <summary>
        /// Get the resolution for the request type.
        /// </summary>
        /// <param name="requestType">The request type that will need to be resolved.</param>
        /// <param name="key">The key for this resolution.</param>
        /// <returns>The resolved instance, if not found a <see cref="NotFoundDependencyResolution"/>.</returns>
        public IDependencyResolution TryGetResolution(object key, Type requestType)
        {
            return TryGetResolution(requestType, key, false);
        }

        /// <summary>
        /// Get the resolution for the request type.
        /// </summary>
        /// <param name="requestType">The request type that will need to be resolved.</param>
        /// <returns>The resolved instance, if not found a <see cref="NotFoundDependencyResolution"/>.</returns>
        public IDependencyResolution TryGetResolution(Type requestType)
        {
            return TryGetResolution(requestType, false);
        }

        /// <summary>
        /// Dispose of the underlying resources. If any <see cref="IDependencyResolution"/>
        /// implements <see cref="IDisposable"/> that will be called.
        /// </summary>
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed) return;

            if (disposing)
            {
                _configuration.Dispose();
            }

            _disposed = true;
        }

        private IDependencyResolution TryGetResolution(Type requestType, bool throwOnNotFound)
        {
            return _configuration.GetResolutionsByType(requestType).FirstOrDefault() ?? new NotFoundDependencyResolution(requestType, null, throwOnNotFound);
        }

        private IDependencyResolution TryGetResolution(Type requestType, object key, bool throwOnNotFound)
        {
            return _configuration.GetResolutionsByKey(requestType, key).FirstOrDefault() ?? new NotFoundDependencyResolution(requestType, key, throwOnNotFound);
        }

        #endregion Methods
    }
}