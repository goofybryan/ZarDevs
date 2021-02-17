using System;
using System.Collections.Generic;

namespace ZarDevs.DependencyInjection
{
    public interface INamedServiceConfiguration
    {
        #region Methods

        void Configure<TService, TInstance>(object name) where TInstance : TService;

        void Configure(Type requestType, Type resolvedType, object name);

        Type ResolveInstanceType<TService>(object name);

        #endregion Methods
    }

    internal class NamedServiceConfiguration : INamedServiceConfiguration
    {
        #region Fields

        private readonly IDictionary<Type, IDictionary<object, Type>> _bindings;

        #endregion Fields

        #region Constructors

        public NamedServiceConfiguration()
        {
            _bindings = new Dictionary<Type, IDictionary<object, Type>>();
        }

        #endregion Constructors

        #region Methods

        public void Configure<TService, TInstance>(object name) where TInstance : TService
        {
            Configure(typeof(TService), typeof(TInstance), name);
        }

        public void Configure(Type requestType, Type resolvedType, object name)
        {
            if (!_bindings.ContainsKey(requestType))
            {
                _bindings[requestType] = new Dictionary<object, Type>();
            }

            _bindings[requestType].Add(name, resolvedType);
        }

        public Type ResolveInstanceType<TService>(object name)
        {
            Type keyType = typeof(TService);

            return _bindings.TryGetValue(keyType, out IDictionary<object, Type> enumBindings) && enumBindings.TryGetValue(name, out Type instanceType) ? instanceType : null;
        }

        #endregion Methods
    }
}