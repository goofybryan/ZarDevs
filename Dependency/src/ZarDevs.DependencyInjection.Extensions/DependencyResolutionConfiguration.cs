using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace ZarDevs.DependencyInjection
{
    /// <summary>
    /// Dependency instance configuration
    /// </summary>
    public interface IDependencyResolutionConfiguration : IDisposable
    {
        #region Methods

        /// <summary>
        /// Configure the request type <paramref name="type"/> to the resolution <paramref name="resolution"/>
        /// </summary>
        /// <param name="type">The request type that will need to be resolved.</param>
        /// <param name="resolution">The resolution that will be implemented.</param>
        void Add(Type type, IDependencyResolution resolution);

        /// <summary>
        /// Add an instance to the configuration for the Type <typeparamref name="T"/>
        /// </summary>
        /// <typeparam name="T">The type of the instance.</typeparam>
        /// <param name="instance">The instance that will always be resolved.</param>
        void AddInstance<T>(T instance);

        /// <summary>
        /// Get a list of configured resolutions by key. If any generic resolution is requested, it
        /// will be generated for all and then the keyed values will be returned.
        /// </summary>
        /// <param name="type">The request type that will need to be resolved.</param>
        /// <param name="key">The key that is requested, can be null.</param>
        /// <returns>A list of resolutions, if none found, will return <see cref="Enumerable.Empty{IDependencyResolution}"/></returns>
        IList<IDependencyResolution> GetResolutionsByKey(Type type, object key);

        /// <summary>
        /// Get a list of configured resolutions. If any generic resolution is requested, it will be
        /// generated for all.
        /// </summary>
        /// <param name="type">The request type that will need to be resolved.</param>
        /// <returns>A list of resolutions, if none found, will return <see cref="Enumerable.Empty{IDependencyResolution}"/></returns>
        IList<IDependencyResolution> GetResolutionsByType(Type type);

        #endregion Methods
    }

    /// <summary>
    /// Dependency instance configuration
    /// </summary>
    public class DependencyResolutionConfiguration : IDependencyResolutionConfiguration
    {
        #region Fields

        private readonly ConcurrentDictionary<Type, ISet<IDependencyResolution>> _typeMap;
        private bool _disposed;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Create a new instance of the dependency resolution configuration.
        /// </summary>
        public DependencyResolutionConfiguration()
        {
            _typeMap = new ConcurrentDictionary<Type, ISet<IDependencyResolution>>();
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Configure the request type <paramref name="type"/> to the resolution <paramref name="resolution"/>
        /// </summary>
        /// <param name="type">The request type that will need to be resolved.</param>
        /// <param name="resolution">The resolution that will be implemented.</param>
        public void Add(Type type, IDependencyResolution resolution)
        {
            AddToTypeMap(type, resolution);
        }

        /// <summary>
        /// Add an instance to the configuration for the Type <typeparamref name="T"/>
        /// </summary>
        /// <typeparam name="T">The type of the instance.</typeparam>
        /// <param name="instance">The instance that will always be resolved.</param>
        public void AddInstance<T>(T instance)
        {
            Add(typeof(T), new DependencySingletonInstance(new DependencyInstanceInfo(typeof(T), instance)));
        }

        /// <summary>
        /// Dispose of the underlying resources. If any <see cref="IDependencyResolution"/> implement <see cref="IDisposable"/>, they will also be called.
        /// </summary>
        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Get a list of configured resolutions by key. If any generic resolution is requested, it
        /// will be generated for all and then the keyed values will be returned.
        /// </summary>
        /// <param name="type">The request type that will need to be resolved.</param>
        /// <param name="key">The key that is requested, can be null. Be warned, if the key is null, it will only return configured values that have a null key.</param>
        /// <returns>A list of resolutions, if none found, will return <see cref="Enumerable.Empty{IDependencyResolution}"/></returns>
        public IList<IDependencyResolution> GetResolutionsByKey(Type type, object key)
        {
            return GetResolutionsByType(type).Where(r => Equals(r.Key, key)).ToArray();
        }

        /// <summary>
        /// Get a list of configured resolutions. If any generic resolution is requested, it will be
        /// generated for all.
        /// </summary>
        /// <param name="type">The request type that will need to be resolved.</param>
        /// <returns>A list of resolutions, if none found, will return <see cref="Enumerable.Empty{IDependencyResolution}"/></returns>
        public IList<IDependencyResolution> GetResolutionsByType(Type type)
        {
            if (_typeMap.TryGetValue(type, out var resolutions))
                return resolutions.ToArray();

            if (!type.IsConstructedGenericType)
                return new IDependencyResolution[0];

            var genericType = type.GetGenericTypeDefinition();

            if (!_typeMap.TryGetValue(genericType, out resolutions))
                return new IDependencyResolution[0];

            return _typeMap.GetOrAdd(type, t => CreateConcreteResolutions(type, resolutions)).ToArray();
        }

        /// <summary>
        /// Dispose of the underlying resources. If any <see cref="IDependencyResolution"/> implement <see cref="IDisposable"/>, they will also be called.
        /// </summary>
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                _disposed = true;

                if (disposing)
                {
                    foreach (var resolutions in _typeMap.Values)
                    {
                        foreach (var disposable in resolutions.OfType<IDisposable>())
                        {
                            disposable.Dispose();
                        }

                        resolutions.Clear();
                    }

                    _typeMap.Clear();
                }
            }
        }

        private bool AddToTypeMap(Type type, IDependencyResolution resolution)
        {
            var resolutions = _typeMap.GetOrAdd(type, t => new HashSet<IDependencyResolution>());

            return resolutions.Add(resolution);
        }

        private IDependencyResolution CreateConcreteResolution(Type requestType, IDependencyResolution resolution)
        {
            var concreteResolution = resolution.MakeConcrete(requestType);
            Add(requestType, concreteResolution);

            return concreteResolution;
        }

        private ISet<IDependencyResolution> CreateConcreteResolutions(Type toType, ISet<IDependencyResolution> generics)
        {
            var concreteSet = new HashSet<IDependencyResolution>();
            foreach (var generic in generics)
            {
                concreteSet.Add(CreateConcreteResolution(toType, generic));
            }
            return concreteSet;
        }

        #endregion Methods
    }
}