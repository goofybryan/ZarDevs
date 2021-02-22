using System;
using System.Collections.Generic;

namespace ZarDevs.DependencyInjection
{
    public interface IDependencyInstanceConfiguration
    {
        #region Methods

        void Configure(Type requestType, IDependencyResolution info);

        #endregion Methods
    }

    public interface IDependencyInstanceResolution
    {
        #region Methods

        IDependencyResolution GetResolution(Type requestType);

        IDependencyResolution GetResolution(object key, Type requestType);

        IDependencyResolution TryGetResolution(Type requestType);

        IDependencyResolution TryGetResolution(object key, Type requestType);

        #endregion Methods
    }

    internal class DependencyResolutionConfiguration : IDependencyInstanceConfiguration, IDependencyInstanceResolution
    {
        #region Fields

        private readonly IDictionary<Type, IDictionary<object, IDependencyResolution>> _bindings;

        #endregion Fields

        #region Constructors

        public DependencyResolutionConfiguration()
        {
            _bindings = new Dictionary<Type, IDictionary<object, IDependencyResolution>>();
        }

        #endregion Constructors

        #region Methods

        public void Configure(Type requestType, IDependencyResolution info)
        {
            if (!_bindings.ContainsKey(requestType))
            {
                _bindings[requestType] = new Dictionary<object, IDependencyResolution>();
            }

            _bindings[requestType].Add(info.Key, info);
        }

        public IDependencyResolution GetResolution(object key, Type requestType)
        {
            return TryGetResolution(key, requestType) ??
                throw new InvalidOperationException("Cannot resolve typed instance for method binding, must be typeof(IDependencyTypeInfo).");
        }

        public IDependencyResolution GetResolution(Type requestType)
        {
            return GetResolution(null, requestType);
        }

        public IDependencyResolution TryGetResolution(object key, Type requestType)
        {
            if (!_bindings.TryGetValue(requestType, out IDictionary<object, IDependencyResolution> binding) || !binding.TryGetValue(key ?? string.Empty, out IDependencyResolution instanceType))
                return null;

            return instanceType;
        }

        public IDependencyResolution TryGetResolution(Type requestType)
        {
            return TryGetResolution(null, requestType);
        }

        #endregion Methods
    }
}