using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

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

        public T Resolve<T>(params KeyValuePair<string, object>[] parameters)
        {
            throw new NotSupportedException("Runtime parameters declaration is not supported for Microsoft dependency injection. Recommended to make use of factory pattern instead.");
        }

        public T Resolve<T>(string name, params KeyValuePair<string, object>[] parameters)
        {
            throw new NotSupportedException("Runtime parameters declaration is not supported for Microsoft dependency injection. Recommended to make use of factory pattern instead.");
        }

        public T Resolve<T>(Enum enumValue, params KeyValuePair<string, object>[] parameters)
        {
            throw new NotSupportedException("Runtime parameters declaration is not supported for Microsoft dependency injection. Recommended to make use of factory pattern instead.");
        }

        public T Resolve<T>(object key, params KeyValuePair<string, object>[] parameters)
        {
            throw new NotSupportedException("Runtime parameters declaration is not supported for Microsoft dependency injection. Recommended to make use of factory pattern instead.");
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

        public T TryResolve<T>(params KeyValuePair<string, object>[] parameters)
        {
            throw new NotSupportedException("Runtime parameters declaration is not supported for Microsoft dependency injection. Recommended to make use of factory pattern instead.");
        }

        public T TryResolve<T>(string name, params KeyValuePair<string, object>[] parameters)
        {
            throw new NotSupportedException("Runtime parameters declaration is not supported for Microsoft dependency injection. Recommended to make use of factory pattern instead.");
        }

        public T TryResolve<T>(Enum enumValue, params KeyValuePair<string, object>[] parameters)
        {
            throw new NotSupportedException("Runtime parameters declaration is not supported for Microsoft dependency injection. Recommended to make use of factory pattern instead.");
        }

        public T TryResolve<T>(object key, params KeyValuePair<string, object>[] parameters)
        {
            throw new NotSupportedException("Runtime parameters declaration is not supported for Microsoft dependency injection. Recommended to make use of factory pattern instead.");
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

        #endregion Methods
    }
}