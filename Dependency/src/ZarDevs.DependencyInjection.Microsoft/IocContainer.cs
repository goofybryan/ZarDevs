using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;

namespace ZarDevs.DependencyInjection
{
    internal class IocContainer : IIocContainer<IServiceProvider>
    {
        #region Fields

        private IDependencyResolver _dependencyResolver;
        private bool _isDisposed;

        #endregion Fields

        #region Constructors

        public IocContainer(IDependencyResolver dependencyResolver, IServiceProvider serviceProvider)
        {
            _dependencyResolver = dependencyResolver ?? throw new ArgumentNullException(nameof(dependencyResolver));
            Kernel = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }

        #endregion Constructors

        #region Properties

        public IServiceProvider Kernel { get; set; }

        #endregion Properties

        #region Methods

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        public T Resolve<T>(params (string, object)[] parameters) where T : class
        {
            return _dependencyResolver.Resolve<T>(parameters);
        }

        public T Resolve<T>() where T : class
        {
            return (T)Kernel.GetRequiredService(typeof(T));
        }

        public T Resolve<T>(params object[] parameters) where T : class
        {
            return _dependencyResolver.Resolve<T>(parameters);
        }

        public IEnumerable<T> ResolveAll<T>() where T : class
        {
            return Kernel.GetServices<T>();
        }

        public IEnumerable ResolveAll(Type requestType)
        {
            return Kernel.GetServices(requestType);
        }

        public T ResolveNamed<T>(string name, params (string, object)[] parameters) where T : class
        {
            return _dependencyResolver.ResolveNamed<T>(name, parameters);
        }

        public T ResolveNamed<T>(string name) where T : class
        {
            return _dependencyResolver.ResolveNamed<T>(name);
        }

        public T ResolveNamed<T>(string name, params object[] parameters) where T : class
        {
            return _dependencyResolver.ResolveNamed<T>(name, parameters);
        }

        public T ResolveWithKey<T>(Enum key, params (string, object)[] parameters) where T : class
        {
            return ResolveWithKey<T>(key, parameters);
        }

        public T ResolveWithKey<T>(object key, params (string, object)[] parameters) where T : class
        {
            return ResolveWithKey<T>(key, parameters);
        }

        public T ResolveWithKey<T>(Enum key) where T : class
        {
            return _dependencyResolver.ResolveWithKey<T>(key);
        }

        public T ResolveWithKey<T>(object key) where T : class
        {
            return _dependencyResolver.ResolveWithKey<T>(key);
        }

        public T ResolveWithKey<T>(Enum key, params object[] parameters) where T : class
        {
            return _dependencyResolver.ResolveWithKey<T>(key, parameters);
        }

        public T ResolveWithKey<T>(object key, params object[] parameters) where T : class
        {
            return _dependencyResolver.ResolveWithKey<T>(key, parameters);
        }

        public T TryResolve<T>(params (string, object)[] parameters) where T : class
        {
            return _dependencyResolver.TryResolve<T>(parameters);
        }

        public T TryResolve<T>() where T : class
        {
            return (T)TryResolve(typeof(T));
        }

        public T TryResolve<T>(params object[] parameters) where T : class
        {
            return _dependencyResolver.TryResolve<T>(parameters);
        }

        public object TryResolve(Type requestType)
        {
            return Kernel.GetService(requestType);
        }

        public T TryResolveNamed<T>(string name, params (string, object)[] parameters) where T : class
        {
            return _dependencyResolver.TryResolveNamed<T>(name, parameters);
        }

        public T TryResolveNamed<T>(string name) where T : class
        {
            return _dependencyResolver.TryResolveNamed<T>(name);
        }

        public T TryResolveNamed<T>(string name, params object[] parameters) where T : class
        {
            return _dependencyResolver.TryResolveNamed<T>(name, parameters);
        }

        public T TryResolveWithKey<T>(Enum key, params (string, object)[] parameters) where T : class
        {
            return _dependencyResolver.TryResolveWithKey<T>(key, parameters);
        }

        public T TryResolveWithKey<T>(object key, params (string, object)[] parameters) where T : class
        {
            return _dependencyResolver.TryResolveWithKey<T>(key, parameters);
        }

        public T TryResolveWithKey<T>(Enum key) where T : class
        {
            return _dependencyResolver.TryResolveWithKey<T>(key);
        }

        public T TryResolveWithKey<T>(object key) where T : class
        {
            return _dependencyResolver.TryResolveWithKey<T>(key);
        }

        public T TryResolveWithKey<T>(Enum key, params object[] parameters) where T : class
        {
            return _dependencyResolver.TryResolveWithKey<T>(key, parameters);
        }

        public T TryResolveWithKey<T>(object key, params object[] parameters) where T : class
        {
            return _dependencyResolver.TryResolveWithKey<T>(key, parameters);
        }

        private void Dispose(bool disposing)
        {
            if (!_isDisposed)
            {
                if (disposing)
                {
                    _dependencyResolver.Dispose();
                    _dependencyResolver = null;

                    Kernel = null;
                }

                _isDisposed = true;
            }
        }

        #endregion Methods
    }
}