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

    public sealed class IocKernelContainer : IIocKernelContainer, IIocKernelServiceProvider
    {
        #region Fields

        private readonly IDependencyContainer _container;
        private IDependencyResolver _dependencyResolver;
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
            _dependencyResolver = serviceProvider.GetRequiredService<IDependencyResolver>();
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
            return _dependencyResolver.Resolve<T>(parameters);
        }

        public T Resolve<T>(string name, params (string, object)[] parameters)
        {
            return Resolve<T>(name, parameters);
        }

        public T Resolve<T>(Enum enumValue, params (string, object)[] parameters)
        {
            return Resolve<T>(enumValue, parameters);
        }

        public T Resolve<T>(object key, params (string, object)[] parameters)
        {
            return Resolve<T>(key, parameters);
        }

        public T Resolve<T>()
        {
            return (T)_serviceProvider.GetRequiredService(typeof(T));
        }

        public T Resolve<T>(string name)
        {
            return _dependencyResolver.Resolve<T>(name, (object[])null);
        }

        public T Resolve<T>(Enum enumValue)
        {
            return _dependencyResolver.Resolve<T>(enumValue, (object[])null);
        }

        public T Resolve<T>(object key)
        {
            return _dependencyResolver.Resolve<T>(key, (object[])null);
        }

        public T Resolve<T>(params object[] parameters)
        {
            return _dependencyResolver.Resolve<T>(parameters);
        }

        public T Resolve<T>(string name, params object[] parameters)
        {
            return _dependencyResolver.Resolve<T>(name, parameters);
        }

        public T Resolve<T>(Enum enumValue, params object[] parameters)
        {
            return _dependencyResolver.Resolve<T>(enumValue, parameters);
        }

        public T Resolve<T>(object key, params object[] parameters)
        {
            return _dependencyResolver.Resolve<T>(key, parameters);
        }

        public T TryResolve<T>(params (string, object)[] parameters)
        {
            return TryResolve<T>(parameters);
        }

        public T TryResolve<T>(string name, params (string, object)[] parameters)
        {
            return TryResolve<T>(name, parameters);
        }

        public T TryResolve<T>(Enum enumValue, params (string, object)[] parameters)
        {
            return TryResolve<T>((object)enumValue, parameters);
        }

        public T TryResolve<T>(object key, params (string, object)[] parameters)
        {
            return _dependencyResolver.TryResolve<T>(key, parameters);
        }

        public T TryResolve<T>()
        {
            return (T)_serviceProvider.GetService(typeof(T));
        }

        public T TryResolve<T>(string name)
        {
            return _dependencyResolver.TryResolve<T>(name, (object[])null);
        }

        public T TryResolve<T>(Enum enumValue)
        {
            return _dependencyResolver.TryResolve<T>(enumValue, (object[])null);
        }

        public T TryResolve<T>(object key)
        {
            return _dependencyResolver.TryResolve<T>(key, (object[])null);
        }

        public T TryResolve<T>(params object[] parameters)
        {
            return ActivatorUtilities.CreateInstance<T>(_serviceProvider, parameters);
        }

        public T TryResolve<T>(string name, params object[] parameters)
        {
            return _dependencyResolver.TryResolve<T>(name, parameters);
        }

        public T TryResolve<T>(Enum enumValue, params object[] parameters)
        {
            return _dependencyResolver.TryResolve<T>(enumValue, parameters);
        }

        public T TryResolve<T>(object key, params object[] parameters)
        {
            return _dependencyResolver.TryResolve<T>(key, parameters);
        }

        #endregion Methods
    }
}