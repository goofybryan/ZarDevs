using Microsoft.Extensions.DependencyInjection;
using System;

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

        public T Resolve<T>()
        {
            return (T)_serviceProvider.GetRequiredService(typeof(T));
        }

        public T Resolve<T>(params object[] parameters)
        {
            return _dependencyResolver.Resolve<T>(parameters);
        }

        public T ResolveNamed<T>(string name, params (string, object)[] parameters)
        {
            return _dependencyResolver.ResolveNamed<T>(name, parameters);
        }

        public T ResolveNamed<T>(string name)
        {
            return _dependencyResolver.ResolveNamed<T>(name, (object[])null);
        }

        public T ResolveNamed<T>(string name, params object[] parameters)
        {
            return _dependencyResolver.ResolveNamed<T>(name, parameters);
        }

        public T ResolveWithKey<T>(Enum key, params (string, object)[] parameters)
        {
            return ResolveWithKey<T>(key, parameters);
        }

        public T ResolveWithKey<T>(object key, params (string, object)[] parameters)
        {
            return ResolveWithKey<T>(key, parameters);
        }

        public T ResolveWithKey<T>(Enum key)
        {
            return _dependencyResolver.ResolveWithKey<T>(key, (object[])null);
        }

        public T ResolveWithKey<T>(object key)
        {
            return _dependencyResolver.ResolveWithKey<T>(key, (object[])null);
        }

        public T ResolveWithKey<T>(Enum key, params object[] parameters)
        {
            return _dependencyResolver.ResolveWithKey<T>(key, parameters);
        }

        public T ResolveWithKey<T>(object key, params object[] parameters)
        {
            return _dependencyResolver.ResolveWithKey<T>(key, parameters);
        }

        public T TryResolve<T>(params (string, object)[] parameters)
        {
            return _dependencyResolver.TryResolve<T>(parameters);
        }

        public T TryResolve<T>()
        {
            return (T)_serviceProvider.GetService(typeof(T));
        }

        public T TryResolve<T>(params object[] parameters)
        {
            return _dependencyResolver.TryResolve<T>(parameters);
        }

        public T TryResolveNamed<T>(string name, params (string, object)[] parameters)
        {
            return _dependencyResolver.TryResolveNamed<T>(name, parameters);
        }

        public T TryResolveNamed<T>(string name)
        {
            return _dependencyResolver.TryResolveNamed<T>(name, (object[])null);
        }

        public T TryResolveNamed<T>(string name, params object[] parameters)
        {
            return _dependencyResolver.TryResolveNamed<T>(name, parameters);
        }

        public T TryResolveWithKey<T>(Enum key, params (string, object)[] parameters)
        {
            return _dependencyResolver.TryResolveWithKey<T>(key, parameters);
        }

        public T TryResolveWithKey<T>(object key, params (string, object)[] parameters)
        {
            return _dependencyResolver.TryResolveWithKey<T>(key, parameters);
        }

        public T TryResolveWithKey<T>(Enum key)
        {
            return _dependencyResolver.TryResolveWithKey<T>(key, (object[])null);
        }

        public T TryResolveWithKey<T>(object key)
        {
            return _dependencyResolver.TryResolveWithKey<T>(key, (object[])null);
        }

        public T TryResolveWithKey<T>(Enum key, params object[] parameters)
        {
            return _dependencyResolver.TryResolveWithKey<T>(key, parameters);
        }

        public T TryResolveWithKey<T>(object key, params object[] parameters)
        {
            return _dependencyResolver.TryResolveWithKey<T>(key, parameters);
        }

        #endregion Methods
    }
}