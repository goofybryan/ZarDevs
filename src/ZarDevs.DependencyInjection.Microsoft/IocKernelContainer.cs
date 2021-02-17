using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using ZarDevs.Runtime;

namespace ZarDevs.DependencyInjection
{
    public interface IIocKernelServiceProvider
    {

        #region Methods

        void ConfigureServiceProvider(IServiceProvider serviceProvider);

        #endregion Methods

    }

    internal sealed class IocKernelContainer : IIocKernelContainer, IIocKernelServiceProvider
    {

        #region Fields

        private readonly IDependencyContainer _container;
        private INamedResolver _namedResolver;
        private IServiceProvider _serviceProvider;

        #endregion Fields

        #region Constructors

        public IocKernelContainer(IDependencyContainer container)
        {
            _container = container ?? throw new ArgumentNullException(nameof(container));
        }

        #endregion Constructors

        #region Methods

        public void ConfigureServiceProvider(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            _namedResolver = serviceProvider.GetRequiredService<INamedResolver>();
        }

        public IDependencyContainer CreateDependencyContainer()
        {
            return _container;
        }

        public void Dispose()
        {
            _serviceProvider = null;
        }

        public T Resolve<T>(params (string, object)[] parameters)
        {
            return Resolve<T>(GetOrderedParameterValuesFromMap(typeof(T), parameters));
        }

        public T Resolve<T>(string name, params (string, object)[] parameters)
        {
            return Resolve<T>(name, GetOrderedParameterValuesFromMap(typeof(T), parameters));
        }

        public T Resolve<T>(Enum enumValue, params (string, object)[] parameters)
        {
            return Resolve<T>(enumValue, GetOrderedParameterValuesFromMap(typeof(T), parameters));
        }

        public T Resolve<T>(object key, params (string, object)[] parameters)
        {
            return Resolve<T>(key, GetOrderedParameterValuesFromMap(typeof(T), parameters));
        }

        public T Resolve<T>()
        {
            return (T)_serviceProvider.GetRequiredService(typeof(T));
        }

        public T Resolve<T>(string name)
        {
            return _namedResolver.Resolve<T>(name);
        }

        public T Resolve<T>(Enum enumValue)
        {
            return _namedResolver.Resolve<T>(enumValue);
        }

        public T Resolve<T>(object key)
        {
            return _namedResolver.Resolve<T>(key);
        }

        public T Resolve<T>(params object[] parameters)
        {
            return ActivatorUtilities.CreateInstance<T>(_serviceProvider, parameters);
        }

        public T Resolve<T>(string name, params object[] parameters)
        {
            return _namedResolver.Resolve<T>(name, parameters);
        }

        public T Resolve<T>(Enum enumValue, params object[] parameters)
        {
            return _namedResolver.Resolve<T>(enumValue, parameters);
        }

        public T Resolve<T>(object key, params object[] parameters)
        {
            return _namedResolver.Resolve<T>(key, parameters);
        }

        public T TryResolve<T>(params (string, object)[] parameters)
        {
            return TryResolve<T>(GetOrderedParameterValuesFromMap(typeof(T), parameters));
        }

        public T TryResolve<T>(string name, params (string, object)[] parameters)
        {
            return TryResolve<T>(name, GetOrderedParameterValuesFromMap(typeof(T), parameters));
        }

        public T TryResolve<T>(Enum enumValue, params (string, object)[] parameters)
        {
            return TryResolve<T>(enumValue, GetOrderedParameterValuesFromMap(typeof(T), parameters));
        }

        public T TryResolve<T>(object key, params (string, object)[] parameters)
        {
            return TryResolve<T>(key, GetOrderedParameterValuesFromMap(typeof(T), parameters));
        }

        public T TryResolve<T>()
        {
            return (T)_serviceProvider.GetService(typeof(T));
        }

        public T TryResolve<T>(string name)
        {
            return _namedResolver.TryResolve<T>(name);
        }

        public T TryResolve<T>(Enum enumValue)
        {
            return _namedResolver.TryResolve<T>(enumValue);
        }

        public T TryResolve<T>(object key)
        {
            return _namedResolver.TryResolve<T>(key);
        }

        public T TryResolve<T>(params object[] parameters)
        {
            return ActivatorUtilities.CreateInstance<T>(_serviceProvider, parameters);
        }

        public T TryResolve<T>(string name, params object[] parameters)
        {
            return _namedResolver.TryResolve<T>(name, parameters);
        }

        public T TryResolve<T>(Enum enumValue, params object[] parameters)
        {
            return _namedResolver.TryResolve<T>(enumValue, parameters);
        }

        public T TryResolve<T>(object key, params object[] parameters)
        {
            return _namedResolver.TryResolve<T>(key, parameters);
        }

        private object[] GetOrderedParameterValuesFromMap(Type type, IList<(string, object)> map)
        {
            return Inspect.Instance.OrderConstructorParameters(type, map.ToDictionary(key => key.Item1, value => value.Item2)).ToArray();
        }

        #endregion Methods

    }
}