using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ZarDevs.DependencyInjection.ZarIoc
{
    /// <summary>
    /// Ioc container for type factory resolution.
    /// </summary>
    public class IocContainer : IIocContainer
    {
        #region Fields

        private readonly ITypeFactoryContainter _typeFactoryContainer;

        private bool _disposedValue;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Create a new instance of the <see cref="IocContainer"/>
        /// </summary>
        /// <param name="typeFactoryContainer">The type factory that contains all the <see cref="ITypeResolution"/></param>
        public IocContainer(ITypeFactoryContainter typeFactoryContainer)
        {
            _typeFactoryContainer = typeFactoryContainer ?? throw new ArgumentNullException(nameof(typeFactoryContainer));

            Type currentType = typeof(IIocContainer);
            _typeFactoryContainer.Add(currentType, new InstanceResolution(new DependencyInstanceInfo(currentType, this)));
        }

        #endregion Constructors

        #region Methods

        /// <inheritdoc/>
        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        /// <inheritdoc/>
        public T Resolve<T>(params object[] parameters) where T : class
        {
            var resolution = GetResolution(typeof(T)).FirstOrDefault();
            return (T)resolution?.Resolve(parameters);
        }

        /// <inheritdoc/>
        public T Resolve<T>(params (string, object)[] parameters) where T : class
        {
            var resolution = GetResolution(typeof(T)).FirstOrDefault();
            return (T)resolution?.Resolve(parameters);
        }

        /// <inheritdoc/>
        public T Resolve<T>() where T : class
        {
            var resolution = GetResolution(typeof(T)).FirstOrDefault();
            return (T)resolution?.Resolve();
        }

        /// <inheritdoc/>
        public IEnumerable ResolveAll(Type requestType)
        {
            var resolved = TryGetResolution(requestType).Resolve().ToList();

            Array resolvedArray = Array.CreateInstance(requestType, resolved.Count);
            for (int i = 0; i < resolved.Count; i++)
            {
                var value = resolved[i];
                resolvedArray.SetValue(value, i);
            }

            return resolvedArray;
        }

        /// <inheritdoc/>
        public IEnumerable<T> ResolveAll<T>() where T : class
        {
            var resolutions = TryGetResolution(typeof(T));
            return resolutions.Resolve().Cast<T>().ToArray();
        }

        /// <inheritdoc/>
        public T ResolveNamed<T>(string key, params object[] parameters) where T : class
        {
            var resolution = GetResolution(typeof(T), key).FirstOrDefault();
            return (T)resolution?.Resolve(parameters);
        }

        /// <inheritdoc/>
        public T ResolveNamed<T>(string key, params (string, object)[] parameters) where T : class
        {
            var resolution = GetResolution(typeof(T), key).FirstOrDefault();
            return (T)resolution?.Resolve(parameters);
        }

        /// <inheritdoc/>
        public T ResolveNamed<T>(string key) where T : class
        {
            var resolution = GetResolution(typeof(T), key).FirstOrDefault();
            return (T)resolution?.Resolve();
        }

        /// <inheritdoc/>
        public T ResolveWithKey<T>(Enum key, params object[] parameters) where T : class
        {
            var resolution = GetResolution(typeof(T), key).FirstOrDefault();
            return (T)resolution?.Resolve(parameters);
        }

        /// <inheritdoc/>
        public T ResolveWithKey<T>(Enum key, params (string, object)[] parameters) where T : class
        {
            var resolution = GetResolution(typeof(T), key).FirstOrDefault();
            return (T)resolution?.Resolve(parameters);
        }

        /// <inheritdoc/>
        public T ResolveWithKey<T>(object key, params object[] parameters) where T : class
        {
            var resolution = GetResolution(typeof(T), key).FirstOrDefault();
            return (T)resolution?.Resolve(parameters);
        }

        /// <inheritdoc/>
        public T ResolveWithKey<T>(object key, params (string, object)[] parameters) where T : class
        {
            var resolution = GetResolution(typeof(T), key).FirstOrDefault();
            return (T)resolution?.Resolve(parameters);
        }

        /// <inheritdoc/>
        public T ResolveWithKey<T>(Enum key) where T : class
        {
            var resolution = GetResolution(typeof(T), key).FirstOrDefault();
            return (T)resolution?.Resolve();
        }

        /// <inheritdoc/>
        public T ResolveWithKey<T>(object key) where T : class
        {
            var resolution = GetResolution(typeof(T), key).FirstOrDefault();
            return (T)resolution?.Resolve();
        }

        /// <inheritdoc/>
        public object TryResolve(Type requestType)
        {
            var resolution = TryGetResolution(requestType, null).FirstOrDefault();
            return resolution?.Resolve();
        }

        /// <inheritdoc/>
        public T TryResolve<T>(params object[] parameters) where T : class
        {
            var resolution = TryGetResolution(typeof(T), null).FirstOrDefault();
            return (T)resolution?.Resolve(parameters);
        }

        /// <inheritdoc/>
        public T TryResolve<T>(params (string, object)[] parameters) where T : class
        {
            var resolution = TryGetResolution(typeof(T), null).FirstOrDefault();
            return (T)resolution?.Resolve(parameters);
        }

        /// <inheritdoc/>
        public T TryResolve<T>() where T : class
        {
            var resolution = TryGetResolution(typeof(T), null).FirstOrDefault();
            return (T)resolution?.Resolve();
        }

        /// <inheritdoc/>
        public object TryResolveNamed(Type requestType, string key, params object[] parameters)
        {
            var resolution = TryGetResolution(requestType, key).FirstOrDefault();
            return resolution?.Resolve(parameters);
        }

        /// <inheritdoc/>
        public T TryResolveNamed<T>(string key, params object[] parameters) where T : class
        {
            var resolution = TryGetResolution(typeof(T), key).FirstOrDefault();
            return (T)resolution?.Resolve(parameters);
        }

        /// <inheritdoc/>
        public T TryResolveNamed<T>(string key, params (string, object)[] parameters) where T : class
        {
            var resolution = TryGetResolution(typeof(T), key).FirstOrDefault();
            return (T)resolution?.Resolve(parameters);
        }

        /// <inheritdoc/>
        public T TryResolveNamed<T>(string key) where T : class
        {
            var resolution = TryGetResolution(typeof(T), key).FirstOrDefault();
            return (T)resolution?.Resolve();
        }

        /// <inheritdoc/>
        public object TryResolveWithKey(Type requestType, Enum key, params object[] parameters)
        {
            var resolution = TryGetResolution(requestType, key).FirstOrDefault();
            return resolution?.Resolve(parameters);
        }

        /// <inheritdoc/>
        public object TryResolveWithKey(Type requestType, object key, params object[] parameters)
        {
            var resolution = TryGetResolution(requestType, key).FirstOrDefault();
            return resolution?.Resolve(parameters);
        }

        /// <inheritdoc/>
        public T TryResolveWithKey<T>(Enum key, params object[] parameters) where T : class
        {
            var resolution = TryGetResolution(typeof(T), key).FirstOrDefault();
            return (T)resolution?.Resolve(parameters);
        }

        /// <inheritdoc/>
        public T TryResolveWithKey<T>(Enum key, params (string, object)[] parameters) where T : class
        {
            var resolution = TryGetResolution(typeof(T), key).FirstOrDefault();
            return (T)resolution?.Resolve(parameters);
        }

        /// <inheritdoc/>
        public T TryResolveWithKey<T>(object key, params object[] parameters) where T : class
        {
            var resolution = TryGetResolution(typeof(T), key).FirstOrDefault();
            return (T)resolution?.Resolve(parameters);
        }

        /// <inheritdoc/>
        public T TryResolveWithKey<T>(object key, params (string, object)[] parameters) where T : class
        {
            var resolution = TryGetResolution(typeof(T), key).FirstOrDefault();
            return (T)resolution?.Resolve(parameters);
        }

        /// <inheritdoc/>
        public T TryResolveWithKey<T>(Enum key) where T : class
        {
            var resolution = TryGetResolution(typeof(T), key).FirstOrDefault();
            return (T)resolution?.Resolve();
        }

        /// <inheritdoc/>
        public T TryResolveWithKey<T>(object key) where T : class
        {
            var resolution = TryGetResolution(typeof(T), key).FirstOrDefault();
            return (T)resolution?.Resolve();
        }

        /// <inheritdoc/>
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                }

                _disposedValue = true;
            }
        }

        private ITypeResolutions GetResolution(Type type)
        {
            return _typeFactoryContainer.Get(type);
        }

        private ITypeResolutions GetResolution(Type type, object key)
        {
            return _typeFactoryContainer.Get(type, key);
        }

        private ITypeResolutions TryGetResolution(Type type)
        {
            return _typeFactoryContainer.TryGet(type, out var resolutions) ? resolutions : new TypeResolutions();
        }

        private ITypeResolutions TryGetResolution(Type type, object key)
        {
            return _typeFactoryContainer.TryGet(type, key, out var resolutions) ? resolutions : new TypeResolutions();
        }

        #endregion Methods
    }
}