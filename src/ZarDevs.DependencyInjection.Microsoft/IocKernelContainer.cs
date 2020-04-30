using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using ZarDevs.DependencyInjection;

namespace ZarDevs.DependencyInjection
{
    public interface IIocKernelServiceProvider
    {
        void ConfigureServiceProvider(IServiceProvider serviceProvider);
    }

    internal sealed class IocKernelContainer : IIocKernelContainer, IIocKernelServiceProvider
    {
        private readonly IDependencyContainer _container;
        private IServiceProvider _serviceProvider;
        private INamedResolver _namedResolver;

        #region Constructors

        public IocKernelContainer(IDependencyContainer container)
        {
            _container = container ?? throw new ArgumentNullException(nameof(container));
        }

        public void ConfigureServiceProvider(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            _namedResolver = serviceProvider.GetRequiredService<INamedResolver>();
        }

        #endregion Constructors

        #region Methods

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
            throw new NotImplementedException();
        }

        public T Resolve<T>(string name)
        {
            throw new NotImplementedException();
        }

        public T Resolve<T>(Enum enumValue)
        {
            throw new NotImplementedException();
        }

        public T Resolve<T>(object key)
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }

        public T TryResolve<T>(string name)
        {
            throw new NotImplementedException();
        }

        public T TryResolve<T>(Enum enumValue)
        {
            return TryResolve<T>();
        }

        public T TryResolve<T>(object key)
        {
            return TryResolve<T>();
        }

        #endregion Methods
    }
}