using System;

namespace ZarDevs.DependencyInjection
{
    public interface IDependencyResolver : IIocContainer
    {
    }

    public class DependencyResolver : IDependencyResolver
    {
        #region Fields

        private readonly IDependencyInstanceResolution _instanceResolution;
        private bool _isDisposed;

        #endregion Fields

        #region Constructors

        public DependencyResolver(IDependencyInstanceResolution instanceResolution)
        {
            _instanceResolution = instanceResolution ?? throw new ArgumentNullException(nameof(instanceResolution));
        }

        #endregion Constructors

        #region Methods

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        public T Resolve<T>(params object[] parameters) where T : class
        {
            return (T)_instanceResolution.GetResolution(typeof(T)).Resolve(parameters);
        }

        public T Resolve<T>(params (string, object)[] parameters) where T : class
        {
            return (T)_instanceResolution.GetResolution(typeof(T)).Resolve(parameters);
        }

        public T Resolve<T>() where T : class
        {
            return (T)_instanceResolution.GetResolution(typeof(T)).Resolve();
        }

        public T ResolveNamed<T>(string key, params object[] parameters) where T : class
        {
            return (T)Resolve(key, typeof(T), parameters);
        }

        public T ResolveNamed<T>(string key, params (string, object)[] parameters) where T : class
        {
            return (T)Resolve(key, typeof(T), parameters);
        }

        public T ResolveNamed<T>(string name) where T : class
        {
            return (T)_instanceResolution.GetResolution(name, typeof(T)).Resolve();
        }

        public T ResolveWithKey<T>(Enum key, params object[] parameters) where T : class
        {
            return (T)Resolve(key, typeof(T), parameters);
        }

        public T ResolveWithKey<T>(object key, params object[] parameters) where T : class
        {
            return (T)Resolve(key, typeof(T), parameters);
        }

        public T ResolveWithKey<T>(Enum key, params (string, object)[] parameters) where T : class
        {
            return (T)Resolve(key, typeof(T), parameters);
        }

        public T ResolveWithKey<T>(object key, params (string, object)[] parameters) where T : class
        {
            return (T)Resolve(key, typeof(T), parameters);
        }

        public T ResolveWithKey<T>(Enum enumValue) where T : class
        {
            return (T)_instanceResolution.GetResolution(enumValue, typeof(T)).Resolve();
        }

        public T ResolveWithKey<T>(object key) where T : class
        {
            return (T)_instanceResolution.GetResolution(key, typeof(T)).Resolve();
        }

        public object TryResolve(Type requestType)
        {
            return _instanceResolution.TryGetResolution(requestType)?.Resolve();
        }

        public T TryResolve<T>(params object[] parameters) where T : class
        {
            return (T)_instanceResolution.TryGetResolution(typeof(T))?.Resolve(parameters);
        }

        public T TryResolve<T>(params (string, object)[] parameters) where T : class
        {
            return (T)_instanceResolution.TryGetResolution(typeof(T))?.Resolve(parameters);
        }

        public T TryResolve<T>() where T : class
        {
            return (T)_instanceResolution.TryGetResolution(typeof(T))?.Resolve();
        }

        public T TryResolveNamed<T>(string key, params object[] parameters) where T : class
        {
            return (T)TryResolve(key, typeof(T), parameters);
        }

        public T TryResolveNamed<T>(string key, params (string, object)[] parameters) where T : class
        {
            return (T)TryResolve(key, typeof(T), parameters);
        }

        public T TryResolveNamed<T>(string name) where T : class
        {
            return (T)_instanceResolution.TryGetResolution(name, typeof(T))?.Resolve();
        }

        public T TryResolveWithKey<T>(Enum key, params object[] parameters) where T : class
        {
            return (T)TryResolve(key, typeof(T), parameters);
        }

        public T TryResolveWithKey<T>(object key, params object[] parameters) where T : class
        {
            return (T)TryResolve(key, typeof(T), parameters);
        }

        public T TryResolveWithKey<T>(Enum key, params (string, object)[] parameters) where T : class
        {
            return (T)TryResolve(key, typeof(T), parameters);
        }

        public T TryResolveWithKey<T>(object key, params (string, object)[] parameters) where T : class
        {
            return (T)TryResolve(key, typeof(T), parameters);
        }

        public T TryResolveWithKey<T>(Enum enumValue) where T : class
        {
            return (T)_instanceResolution.TryGetResolution(enumValue, typeof(T))?.Resolve();
        }

        public T TryResolveWithKey<T>(object key) where T : class
        {
            return (T)_instanceResolution.TryGetResolution(key, typeof(T))?.Resolve();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_isDisposed) return;

            if (disposing)
            {
                _instanceResolution.Dispose();
            }

            _isDisposed = true;
        }

        private object Resolve(object key, Type requestType, params object[] parameters)
        {
            return _instanceResolution.GetResolution(key, requestType).Resolve(parameters);
        }

        private object Resolve(object key, Type requestType, params (string, object)[] parameters)
        {
            return _instanceResolution.GetResolution(key, requestType).Resolve(parameters);
        }

        private object TryResolve(object key, Type requestType, params object[] parameters)
        {
            return _instanceResolution.TryGetResolution(key, requestType)?.Resolve(parameters);
        }

        private object TryResolve(object key, Type requestType, params (string, object)[] parameters)
        {
            return _instanceResolution.TryGetResolution(key, requestType)?.Resolve(parameters);
        }

        #endregion Methods
    }
}