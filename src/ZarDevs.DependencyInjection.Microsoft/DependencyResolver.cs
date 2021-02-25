using System;

namespace ZarDevs.DependencyInjection
{
    public interface IDependencyResolver
    {
        #region Methods

        T Resolve<T>(params object[] args);

        T Resolve<T>(params (string, object)[] args);

        T ResolveNamed<T>(string key, params object[] args);

        T ResolveNamed<T>(string key, params (string, object)[] args);

        T ResolveWithKey<T>(Enum key, params object[] args);

        T ResolveWithKey<T>(object key, params object[] args);

        T ResolveWithKey<T>(Enum key, params (string, object)[] args);

        T ResolveWithKey<T>(object key, params (string, object)[] args);

        T TryResolve<T>(params object[] args);

        T TryResolve<T>(params (string, object)[] args);

        T TryResolveNamed<T>(string key, params object[] args);

        T TryResolveNamed<T>(string key, params (string, object)[] args);

        T TryResolveWithKey<T>(Enum key, params object[] args);

        T TryResolveWithKey<T>(object key, params object[] args);

        T TryResolveWithKey<T>(Enum key, params (string, object)[] args);

        T TryResolveWithKey<T>(object key, params (string, object)[] args);

        #endregion Methods
    }

    internal class DependencyResolver : IDependencyResolver
    {
        #region Fields

        private readonly IDependencyInstanceResolution _instanceResolution;
        private readonly IIocContainer _ioc;

        #endregion Fields

        #region Constructors

        public DependencyResolver(IIocContainer ioc, IDependencyInstanceResolution instanceResolution)
        {
            _ioc = ioc ?? throw new ArgumentNullException(nameof(ioc));
            _instanceResolution = instanceResolution ?? throw new ArgumentNullException(nameof(instanceResolution));
        }

        #endregion Constructors

        #region Methods

        public T Resolve<T>(params object[] args)
        {
            return (T)_instanceResolution.GetResolution(typeof(T)).Resolve(_ioc, args);
        }

        public T Resolve<T>(params (string, object)[] args)
        {
            return (T)_instanceResolution.GetResolution(typeof(T)).Resolve(_ioc, args);
        }

        public T ResolveNamed<T>(string key, params object[] args)
        {
            return (T)Resolve(key, typeof(T), args);
        }

        public T ResolveNamed<T>(string key, params (string, object)[] args)
        {
            return (T)Resolve(key, typeof(T), args);
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

        public T TryResolve<T>(params object[] args)
        {
            return (T)_instanceResolution.TryGetResolution(typeof(T))?.Resolve(_ioc, args);
        }

        public T TryResolve<T>(params (string, object)[] args)
        {
            return (T)_instanceResolution.TryGetResolution(typeof(T))?.Resolve(_ioc, args);
        }

        public T TryResolveNamed<T>(string key, params object[] args)
        {
            return (T)TryResolve(key, typeof(T), args);
        }

        public T TryResolveNamed<T>(string key, params (string, object)[] args)
        {
            return (T)TryResolve(key, typeof(T), args);
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

        private object Resolve(object key, Type requestType, params object[] args)
        {
            return _instanceResolution.GetResolution(key, requestType).Resolve(_ioc, args);
        }

        private object Resolve(object key, Type requestType, params (string, object)[] args)
        {
            return _instanceResolution.GetResolution(key, requestType).Resolve(_ioc, args);
        }

        private object TryResolve(object key, Type requestType, params object[] args)
        {
            return _instanceResolution.TryGetResolution(key, requestType)?.Resolve(_ioc, args);
        }

        private object TryResolve(object key, Type requestType, params (string, object)[] args)
        {
            return _instanceResolution.TryGetResolution(key, requestType)?.Resolve(_ioc, args);
        }

        #endregion Methods
    }
}