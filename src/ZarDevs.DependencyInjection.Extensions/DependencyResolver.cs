using System;

namespace ZarDevs.DependencyInjection
{
    public interface IDependencyResolver : IIocContainer<IDependencyInstanceResolution>
    {
    }

    public class DependencyResolver : IDependencyResolver
    {
        #region Fields

        private bool _isDisposed;

        #endregion Fields

        #region Constructors

        public DependencyResolver(IDependencyInstanceResolution instanceResolution)
        {
            Kernel = instanceResolution ?? throw new ArgumentNullException(nameof(instanceResolution));
        }

        #endregion Constructors

        #region Properties

        public IDependencyInstanceResolution Kernel { get; }

        #endregion Properties

        #region Methods

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        public T Resolve<T>(params object[] parameters) where T : class
        {
            return (T)Kernel.GetResolution(typeof(T)).Resolve(parameters);
        }

        public T Resolve<T>(params (string, object)[] parameters) where T : class
        {
            return (T)Kernel.GetResolution(typeof(T)).Resolve(parameters);
        }

        public T Resolve<T>() where T : class
        {
            return (T)Kernel.GetResolution(typeof(T)).Resolve();
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
            return (T)Kernel.GetResolution(name, typeof(T)).Resolve();
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
            return (T)Kernel.GetResolution(enumValue, typeof(T)).Resolve();
        }

        public T ResolveWithKey<T>(object key) where T : class
        {
            return (T)Kernel.GetResolution(key, typeof(T)).Resolve();
        }

        public object TryResolve(Type requestType)
        {
            return Kernel.TryGetResolution(requestType)?.Resolve();
        }

        public T TryResolve<T>(params object[] parameters) where T : class
        {
            return (T)Kernel.TryGetResolution(typeof(T))?.Resolve(parameters);
        }

        public T TryResolve<T>(params (string, object)[] parameters) where T : class
        {
            return (T)Kernel.TryGetResolution(typeof(T))?.Resolve(parameters);
        }

        public T TryResolve<T>() where T : class
        {
            return (T)Kernel.TryGetResolution(typeof(T))?.Resolve();
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
            return (T)Kernel.TryGetResolution(name, typeof(T))?.Resolve();
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
            return (T)Kernel.TryGetResolution(enumValue, typeof(T))?.Resolve();
        }

        public T TryResolveWithKey<T>(object key) where T : class
        {
            return (T)Kernel.TryGetResolution(key, typeof(T))?.Resolve();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_isDisposed) return;

            if (disposing)
            {
                Kernel.Dispose();
            }

            _isDisposed = true;
        }

        private object Resolve(object key, Type requestType, params object[] parameters)
        {
            return Kernel.GetResolution(key, requestType).Resolve(parameters);
        }

        private object Resolve(object key, Type requestType, params (string, object)[] parameters)
        {
            return Kernel.GetResolution(key, requestType).Resolve(parameters);
        }

        private object TryResolve(object key, Type requestType, params object[] parameters)
        {
            return Kernel.TryGetResolution(key, requestType)?.Resolve(parameters);
        }

        private object TryResolve(object key, Type requestType, params (string, object)[] parameters)
        {
            return Kernel.TryGetResolution(key, requestType)?.Resolve(parameters);
        }

        #endregion Methods
    }
}