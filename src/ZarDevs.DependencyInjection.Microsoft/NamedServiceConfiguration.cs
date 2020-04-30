using System;
using System.Collections.Generic;

namespace ZarDevs.DependencyInjection
{
    public interface INamedServiceConfiguration
    {
        #region Methods

        void Configure<TService, TInstance>(object name) where TInstance : TService;

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
            Type keyType = typeof(TService);

            if (!_bindings.ContainsKey(keyType))
            {
                _bindings[keyType] = new Dictionary<object, Type>();
            }

            _bindings[keyType].Add(name, typeof(TInstance));
        }

        public Type ResolveInstanceType<TService>(object name)
        {
            Type keyType = typeof(TService);

            return _bindings.TryGetValue(keyType, out IDictionary<object, Type> enumBindings) && enumBindings.TryGetValue(name, out Type instanceType) ? instanceType : null;
        }

        #endregion Methods
    }
}