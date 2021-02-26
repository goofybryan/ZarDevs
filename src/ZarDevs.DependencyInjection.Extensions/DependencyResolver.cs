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

        public T Resolve<T>(params object[] args)
        {
            return (T)_instanceResolution.GetResolution(typeof(T)).Resolve(this, args);
        }

        public T Resolve<T>(params (string, object)[] args)
        {
            return (T)_instanceResolution.GetResolution(typeof(T)).Resolve(this, args);
        }

        public T Resolve<T>()
        {
            return (T)_instanceResolution.GetResolution(typeof(T)).Resolve(this);
        }

        public T ResolveNamed<T>(string key, params object[] args)
        {
            return (T)Resolve(key, typeof(T), args);
        }

        public T ResolveNamed<T>(string key, params (string, object)[] args)
        {
            return (T)Resolve(key, typeof(T), args);
        }

        public T ResolveNamed<T>(string name)
        {
            return (T)_instanceResolution.GetResolution(name, typeof(T)).Resolve(this);
        }

        public T ResolveWithKey<T>(Enum key, params object[] args)
        {
            return (T)Resolve(key, typeof(T), args);
        }

        public T ResolveWithKey<T>(object key, params object[] args)
        {
            return (T)Resolve(key, typeof(T), args);
        }

        public T ResolveWithKey<T>(Enum key, params (string, object)[] args)
        {
            return (T)Resolve(key, typeof(T), args);
        }

        public T ResolveWithKey<T>(object key, params (string, object)[] args)
        {
            return (T)Resolve(key, typeof(T), args);
        }

        public T ResolveWithKey<T>(Enum enumValue)
        {
            return (T)_instanceResolution.GetResolution(enumValue, typeof(T)).Resolve(this);
        }

        public T ResolveWithKey<T>(object key)
        {
            return (T)_instanceResolution.GetResolution(key, typeof(T)).Resolve(this);
        }

        public object TryResolve(Type requestType)
        {
            return _instanceResolution.TryGetResolution(requestType)?.Resolve(this);
        }

        public T TryResolve<T>(params object[] args)
        {
            return (T)_instanceResolution.TryGetResolution(typeof(T))?.Resolve(this, args);
        }

        public T TryResolve<T>(params (string, object)[] args)
        {
            return (T)_instanceResolution.TryGetResolution(typeof(T))?.Resolve(this, args);
        }

        public T TryResolve<T>()
        {
            return (T)_instanceResolution.TryGetResolution(typeof(T))?.Resolve(this);
        }

        public T TryResolveNamed<T>(string key, params object[] args)
        {
            return (T)TryResolve(key, typeof(T), args);
        }

        public T TryResolveNamed<T>(string key, params (string, object)[] args)
        {
            return (T)TryResolve(key, typeof(T), args);
        }

        public T TryResolveNamed<T>(string name)
        {
            return (T)_instanceResolution.TryGetResolution(name, typeof(T))?.Resolve(this);
        }

        public T TryResolveWithKey<T>(Enum key, params object[] args)
        {
            return (T)TryResolve(key, typeof(T), args);
        }

        public T TryResolveWithKey<T>(object key, params object[] args)
        {
            return (T)TryResolve(key, typeof(T), args);
        }

        public T TryResolveWithKey<T>(Enum key, params (string, object)[] args)
        {
            return (T)TryResolve(key, typeof(T), args);
        }

        public T TryResolveWithKey<T>(object key, params (string, object)[] args)
        {
            return (T)TryResolve(key, typeof(T), args);
        }

        public T TryResolveWithKey<T>(Enum enumValue)
        {
            return (T)_instanceResolution.TryGetResolution(enumValue, typeof(T))?.Resolve(this);
        }

        public T TryResolveWithKey<T>(object key)
        {
            return (T)_instanceResolution.TryGetResolution(key, typeof(T))?.Resolve(this);
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

        private object Resolve(object key, Type requestType, params object[] args)
        {
            return _instanceResolution.GetResolution(key, requestType).Resolve(this, args);
        }

        private object Resolve(object key, Type requestType, params (string, object)[] args)
        {
            return _instanceResolution.GetResolution(key, requestType).Resolve(this, args);
        }

        private object TryResolve(object key, Type requestType, params object[] args)
        {
            return _instanceResolution.TryGetResolution(key, requestType)?.Resolve(this, args);
        }

        private object TryResolve(object key, Type requestType, params (string, object)[] args)
        {
            return _instanceResolution.TryGetResolution(key, requestType)?.Resolve(this, args);
        }

        #endregion Methods
    }
}