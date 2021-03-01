using System;
using System.Collections.Generic;
using System.Linq;

namespace ZarDevs.DependencyInjection
{
    public interface IDependencyInstanceConfiguration
    {
        #region Methods

        void AddInstanceResolution<T>(T instance, object key);

        void Configure(Type requestType, IDependencyResolution info);

        #endregion Methods
    }

    public interface IDependencyInstanceResolution : IDisposable
    {
        #region Methods

        IDependencyResolution GetResolution(Type requestType);

        IDependencyResolution GetResolution(object key, Type requestType);

        IDependencyResolution TryGetResolution(Type requestType);

        IDependencyResolution TryGetResolution(object key, Type requestType);

        #endregion Methods
    }

    public class DependencyResolutionConfiguration : IDependencyInstanceConfiguration, IDependencyInstanceResolution
    {
        #region Fields

        private readonly IDictionary<Type, IDictionary<object, IDependencyResolution>> _bindings;
        private bool _disposed;

        #endregion Fields

        #region Constructors

        public DependencyResolutionConfiguration()
        {
            _bindings = new Dictionary<Type, IDictionary<object, IDependencyResolution>>();
        }

        #endregion Constructors

        #region Methods

        public void AddInstanceResolution<T>(T instance, object key)
        {
            Type requestType = typeof(T);
            Configure(requestType, new DependencySingletonInstance(new DependencyInstanceInfo(instance, new DependencyInfo { RequestType = requestType, Key = key })));
        }

        public void Configure(Type requestType, IDependencyResolution info)
        {
            if (!_bindings.ContainsKey(requestType))
            {
                _bindings[requestType] = new Dictionary<object, IDependencyResolution>();
            }

            _bindings[requestType].Add(info.Key ?? string.Empty, info);
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        public IDependencyResolution GetResolution(object key, Type requestType)
        {
            return TryGetResolution(key, requestType) ??
                throw new InvalidOperationException($"Cannot get resolution for {requestType} for key {key}.");
        }

        public IDependencyResolution GetResolution(Type requestType)
        {
            return GetResolution(null, requestType);
        }

        public IDependencyResolution TryGetResolution(object key, Type requestType)
        {
            if (!_bindings.TryGetValue(requestType, out IDictionary<object, IDependencyResolution> binding)
                || !binding.TryGetValue(key ?? string.Empty, out IDependencyResolution instanceType))
                return null;

            return instanceType;
        }

        public IDependencyResolution TryGetResolution(Type requestType)
        {
            return TryGetResolution(null, requestType);
        }

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

        #endregion Methods
    }
}