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
        /// Get the resolution for the request type.
        /// </summary>
        /// <param name="requestType">The request type that will need to be resolved.</param>
        /// <returns>The resolved instance, if not found a <see cref="NotFoundDependencyResolution"/> that will throw an <see cref="DependencyResolutionNotConfiguredException"/>.</returns>
        IDependencyResolution GetResolution(Type requestType);

        /// <summary>
        /// Get the resolution for the request type.
        /// </summary>
        /// <param name="requestType">The request type that will need to be resolved.</param>
        /// <param name="key">The key for this resolution.</param>
        /// <returns>The resolved instance, if not found a <see cref="NotFoundDependencyResolution"/> that will throw an <see cref="DependencyResolutionNotConfiguredException"/>.</returns>
        IDependencyResolution GetResolution(object key, Type requestType);

        /// <summary>
        /// Try and get the resolution for the request type.
        /// </summary>
        /// <param name="requestType">The request type that will need to be resolved.</param>
        /// <returns>The resolved instance, if not found a <see cref="NotFoundDependencyResolution"/> that will return null when resolved.</returns>
        IDependencyResolution TryGetResolution(Type requestType);

        /// <summary>
        /// Get the resolution for the request type.
        /// </summary>
        /// <param name="requestType">The request type that will need to be resolved.</param>
        /// <param name="key">The key for this resolution.</param>
        /// <returns>The resolved instance, if not found a <see cref="NotFoundDependencyResolution"/> that will return null when resolved.</returns>
        IDependencyResolution TryGetResolution(object key, Type requestType);

        #endregion Methods
    }

    /// <summary>
    /// Dependency resolution configuration is used to configure any dependency resolutions and get them them at runtime.
    /// </summary>
    public class DependencyInstanceResolution : IDependencyInstanceConfiguration, IDependencyInstanceResolution
    {
        #region Fields

        private readonly IDictionary<Type, IDictionary<object, IDependencyResolution>> _bindings;
        private bool _disposed;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Create a new instance of the Dependency resolution configuration.
        /// </summary>
        public DependencyInstanceResolution()
        {
            _bindings = new Dictionary<Type, IDictionary<object, IDependencyResolution>>();
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Add an instance to the configuration for the Type <typeparamref name="T"/>
        /// </summary>
        /// <typeparam name="T">The type of the instance.</typeparam>
        /// <param name="instance">The instance that will always be resolved.</param>
        /// <param name="key">An optional key</param>
        public void AddInstanceResolution<T>(T instance, object key)
        {
            Type requestType = typeof(T);
            Configure(requestType, new DependencySingletonInstance(new DependencyInstanceInfo(instance, new DependencyInfo { RequestType = requestType, Key = key })));
        }

        /// <summary>
        /// Configure the request type <paramref name="requestType"/> to the resolution <paramref name="info"/>
        /// </summary>
        /// <param name="requestType">The request type that will need to be resolved.</param>
        /// <param name="info">The resolution that will be implemented.</param>
        public void Configure(Type requestType, IDependencyResolution info)
        {
            if (!_bindings.ContainsKey(requestType))
            {
                _bindings[requestType] = new Dictionary<object, IDependencyResolution>();
            }

            _bindings[requestType].Add(info.Key ?? string.Empty, info);
        }

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
        /// Get the resolution for the request type.
        /// </summary>
        /// <param name="requestType">The request type that will need to be resolved.</param>
        /// <param name="key">The key for this resolution.</param>
        /// <returns>The resolved instance, if not found a <see cref="NotFoundDependencyResolution"/> that will throw an <see cref="DependencyResolutionNotConfiguredException"/>.</returns>
        public IDependencyResolution GetResolution(object key, Type requestType)
        {
            return TryGetResolution(key, requestType, true);
        }

        /// <summary>
        /// Get the resolution for the request type.
        /// </summary>
        /// <param name="requestType">The request type that will need to be resolved.</param>
        /// <returns>The resolved instance, if not found a <see cref="NotFoundDependencyResolution"/> that will throw an <see cref="DependencyResolutionNotConfiguredException"/>.</returns>
        public IDependencyResolution GetResolution(Type requestType)
        {
            return GetResolution(null, requestType);
        }

        /// <summary>
        /// Get the resolution for the request type.
        /// </summary>
        /// <param name="requestType">The request type that will need to be resolved.</param>
        /// <param name="key">The key for this resolution.</param>
        /// <returns>The resolved instance, if not found a <see cref="NotFoundDependencyResolution"/>.</returns>
        public IDependencyResolution TryGetResolution(object key, Type requestType)
        {
            return TryGetResolution(key, requestType, false);
        }

        /// <summary>
        /// Get the resolution for the request type.
        /// </summary>
        /// <param name="requestType">The request type that will need to be resolved.</param>
        /// <returns>The resolved instance, if not found a <see cref="NotFoundDependencyResolution"/>.</returns>
        public IDependencyResolution TryGetResolution(Type requestType)
        {
            return TryGetResolution(null, requestType);
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
                foreach (var resolutions in _bindings.Values)
                {
                    foreach (var disposable in resolutions.OfType<IDisposable>())
                    {
                        disposable.Dispose();
                    }
                }

                _bindings.Clear();
            }

            _disposed = true;
        }

        private IDependencyResolution TryGetResolution(object key, Type requestType, bool throwOnNotFound)
        {
            if (TryGetResolution(key, requestType, out IDependencyResolution resolution))
                return resolution;

            if (!requestType.IsConstructedGenericType) 
                return new NotFoundDependencyResolution(requestType, key, throwOnNotFound);

            var genericType = requestType.GetGenericTypeDefinition();

            if(!TryGetResolution(key, genericType, out resolution))
                return new NotFoundDependencyResolution(requestType, key, throwOnNotFound);

            Configure(requestType, resolution.MakeConcrete(requestType));

            return _bindings[requestType][key ?? string.Empty];
        }

        private bool TryGetResolution(object key, Type requestType, out IDependencyResolution resolution)
        { 
            resolution = null;

            return _bindings.TryGetValue(requestType, out IDictionary<object, IDependencyResolution> binding)
                            && binding.TryGetValue(key ?? string.Empty, out resolution);
        }


        #endregion Methods
    }
}