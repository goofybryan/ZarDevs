using System;
using System.Collections;
using System.Collections.Generic;

namespace ZarDevs.DependencyInjection
{
    /// <summary>
    /// IOC containter used to resolved a single instance
    /// </summary>
    public interface IIocContainer : IDisposable
    {
        #region Methods

        /// <summary>
        /// Resolve the requested type with the list of paramaters specified.
        /// </summary>
        /// <typeparam name="T">The request type</typeparam>
        /// <param name="parameters">
        /// A list of ordered parameters, these must match a constructor order.
        /// </param>
        /// <returns>The resolved type.</returns>
        T Resolve<T>(params object[] parameters) where T : class;

        /// <summary>
        /// Resolve the requested type with the list of paramaters specified.
        /// </summary>
        /// <typeparam name="T">The request type</typeparam>
        /// <param name="parameters">
        /// A list of named parameters, these must match a constructor parameter name and associated types.
        /// </param>
        /// <returns>The resolved type.</returns>
        T Resolve<T>(params (string, object)[] parameters) where T : class;

        /// <summary>
        /// Resolve the requested type.
        /// </summary>
        /// <typeparam name="T">The request type</typeparam>
        /// <returns>The resolved type.</returns>
        T Resolve<T>() where T : class;

        /// <summary>
        /// Resolve all instance of the requested type <paramref name="requestType"/>.
        /// </summary>
        /// <param name="requestType">The request type to resolve,</param>
        /// <returns>The resolved IEnumberable as an object (can safely be cast to IEnumerable).</returns>
        IEnumerable ResolveAll(Type requestType);

        /// <summary>
        /// Resolve all instance of the requested type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The request type</typeparam>
        /// <returns>The resolved type.</returns>
        IEnumerable<T> ResolveAll<T>() where T : class;

        /// <summary>
        /// Resolve the requested type with the list of paramaters specified.
        /// </summary>
        /// <param name="key">The name of the resolution request.</param>
        /// <typeparam name="T">The request type</typeparam>
        /// <param name="parameters">
        /// A list of ordered parameters, these must match a constructor order.
        /// </param>
        /// <returns>The resolved type.</returns>
        T ResolveNamed<T>(string key, params object[] parameters) where T : class;

        /// <summary>
        /// Resolve the requested type with the list of paramaters specified.
        /// </summary>
        /// <param name="name">The name of the resolution request.</param>
        /// <typeparam name="T">The request type</typeparam>
        /// <param name="parameters">
        /// A list of named parameters, these must match a constructor parameter name and associated types.
        /// </param>
        /// <returns>The resolved type.</returns>
        T ResolveNamed<T>(string name, params (string, object)[] parameters) where T : class;

        /// <summary>
        /// Resolve the requested type.
        /// </summary>
        /// <param name="key">The name of the resolution request.</param>
        /// <typeparam name="T">The request type</typeparam>
        /// <returns>The resolved type.</returns>
        T ResolveNamed<T>(string key) where T : class;

        /// <summary>
        /// Resolve the requested type with the list of paramaters specified.
        /// </summary>
        /// <param name="key">The key of the resolution request.</param>
        /// <typeparam name="T">The request type</typeparam>
        /// <param name="parameters">
        /// A list of ordered parameters, these must match a constructor order.
        /// </param>
        /// <returns>The resolved type.</returns>
        T ResolveWithKey<T>(Enum key, params object[] parameters) where T : class;

        /// <summary>
        /// Resolve the requested type with the list of paramaters specified.
        /// </summary>
        /// <param name="key">The key of the resolution request.</param>
        /// <typeparam name="T">The request type</typeparam>
        /// <param name="parameters">
        /// A list of named parameters, these must match a constructor parameter name and associated types.
        /// </param>
        /// <returns>The resolved type.</returns>
        T ResolveWithKey<T>(Enum key, params (string, object)[] parameters) where T : class;

        /// <summary>
        /// Resolve the requested type with the list of paramaters specified.
        /// </summary>
        /// <param name="key">The key of the resolution request.</param>
        /// <typeparam name="T">The request type</typeparam>
        /// <param name="parameters">
        /// A list of ordered parameters, these must match a constructor order.
        /// </param>
        /// <returns>The resolved type.</returns>
        T ResolveWithKey<T>(object key, params object[] parameters) where T : class;

        /// <summary>
        /// Resolve the requested type with the list of paramaters specified.
        /// </summary>
        /// <param name="key">The key of the resolution request.</param>
        /// <typeparam name="T">The request type</typeparam>
        /// <param name="parameters">
        /// A list of named parameters, these must match a constructor parameter name and associated types.
        /// </param>
        /// <returns>The resolved type.</returns>
        T ResolveWithKey<T>(object key, params (string, object)[] parameters) where T : class;

        /// <summary>
        /// Resolve the requested type with the list of paramaters specified.
        /// </summary>
        /// <param name="key">The key of the resolution request.</param>
        /// <typeparam name="T">The request type</typeparam>
        /// <returns>The resolved type.</returns>
        T ResolveWithKey<T>(Enum key) where T : class;

        /// <summary>
        /// Resolve the requested type with the list of paramaters specified.
        /// </summary>
        /// <param name="key">The key of the resolution request.</param>
        /// <typeparam name="T">The request type</typeparam>
        /// <returns>The resolved type.</returns>
        T ResolveWithKey<T>(object key) where T : class;

        /// <summary>
        /// Try resolved the requested type with the list of paramaters specified.
        /// </summary>
        /// <param name="requestType">The request type</param>
        /// <returns>The resolved type.</returns>
        object TryResolve(Type requestType);

        /// <summary>
        /// Try resolved the requested type with the list of paramaters specified.
        /// </summary>
        /// <typeparam name="T">The request type</typeparam>
        /// <param name="parameters">
        /// A list of ordered parameters, these must match a constructor order.
        /// </param>
        /// <returns>The resolved type.</returns>
        T TryResolve<T>(params object[] parameters) where T : class;

        /// <summary>
        /// Try resolved the requested type with the list of paramaters specified.
        /// </summary>
        /// <typeparam name="T">The request type</typeparam>
        /// <param name="parameters">
        /// A list of named parameters, these must match a constructor parameter name and associated types.
        /// </param>
        /// <returns>The resolved type.</returns>
        T TryResolve<T>(params (string, object)[] parameters) where T : class;

        /// <summary>
        /// Try resolved the requested type with the list of paramaters specified.
        /// </summary>
        /// <typeparam name="T">The request type</typeparam>
        /// <returns>The resolved type.</returns>
        T TryResolve<T>() where T : class;

        /// <summary>
        /// Try resolved the requested type with the list of paramaters specified.
        /// </summary>
        /// <param name="requestType">The request type</param>
        /// <param name="key">The name of the resolution request.</param>
        /// <param name="parameters">
        /// A list of named parameters, these must match a constructor order.
        /// </param>
        /// <returns>The resolved type.</returns>
        object TryResolveNamed(Type requestType, string key, params object[] parameters);

        /// <summary>
        /// Try resolved the requested type with the list of paramaters specified.
        /// </summary>
        /// <typeparam name="T">The request type</typeparam>
        /// <param name="key">The name of the resolution request.</param>
        /// <param name="parameters">
        /// A list of named parameters, these must match a constructor order.
        /// </param>
        /// <returns>The resolved type.</returns>
        T TryResolveNamed<T>(string key, params object[] parameters) where T : class;

        /// <summary>
        /// Try resolved the requested type with the list of paramaters specified.
        /// </summary>
        /// <typeparam name="T">The request type</typeparam>
        /// <param name="key">The name of the resolution request.</param>
        /// <param name="parameters">
        /// A list of named parameters, these must match a constructor parameter name and associated types.
        /// </param>
        /// <returns>The resolved type.</returns>
        T TryResolveNamed<T>(string key, params (string, object)[] parameters) where T : class;

        /// <summary>
        /// Try resolved the requested type with the list of paramaters specified.
        /// </summary>
        /// <typeparam name="T">The request type</typeparam>
        /// <param name="key">The name of the resolution request.</param>
        /// <returns>The resolved type.</returns>
        T TryResolveNamed<T>(string key) where T : class;

        /// <summary>
        /// Try resolved the requested type with the list of paramaters specified.
        /// </summary>
        /// <param name="requestType">The request type</param>
        /// <param name="key">The key of the resolution request.</param>
        /// <param name="parameters">
        /// A list of named parameters, these must match a constructor order.
        /// </param>
        /// <returns>The resolved type.</returns>
        object TryResolveWithKey(Type requestType, Enum key, params object[] parameters);

        /// <summary>
        /// Try resolved the requested type with the list of paramaters specified.
        /// </summary>
        /// <param name="requestType">The request type</param>
        /// <param name="key">The key of the resolution request.</param>
        /// <param name="parameters">
        /// A list of named parameters, these must match a constructor order.
        /// </param>
        /// <returns>The resolved type.</returns>
        object TryResolveWithKey(Type requestType, object key, params object[] parameters);

        /// <summary>
        /// Try resolved the requested type with the list of paramaters specified.
        /// </summary>
        /// <typeparam name="T">The request type</typeparam>
        /// <param name="key">The key of the resolution request.</param>
        /// <param name="parameters">
        /// A list of named parameters, these must match a constructor order.
        /// </param>
        /// <returns>The resolved type.</returns>
        T TryResolveWithKey<T>(Enum key, params object[] parameters) where T : class;

        /// <summary>
        /// Try resolved the requested type with the list of paramaters specified.
        /// </summary>
        /// <typeparam name="T">The request type</typeparam>
        /// <param name="key">The key of the resolution request.</param>
        /// <param name="parameters">
        /// A list of named parameters, these must match a constructor parameter name and associated types.
        /// </param>
        /// <returns>The resolved type.</returns>
        T TryResolveWithKey<T>(Enum key, params (string, object)[] parameters) where T : class;

        /// <summary>
        /// Try resolved the requested type with the list of paramaters specified.
        /// </summary>
        /// <typeparam name="T">The request type</typeparam>
        /// <param name="key">The key of the resolution request.</param>
        /// <param name="parameters">
        /// A list of named parameters, these must match a constructor order.
        /// </param>
        /// <returns>The resolved type.</returns>
        T TryResolveWithKey<T>(object key, params object[] parameters) where T : class;

        /// <summary>
        /// Try resolved the requested type with the list of paramaters specified.
        /// </summary>
        /// <typeparam name="T">The request type</typeparam>
        /// <param name="key">The key of the resolution request.</param>
        /// <param name="parameters">
        /// A list of named parameters, these must match a constructor parameter name and associated types.
        /// </param>
        /// <returns>The resolved type.</returns>
        T TryResolveWithKey<T>(object key, params (string, object)[] parameters) where T : class;

        /// <summary>
        /// Try resolved the requested type with the list of paramaters specified.
        /// </summary>
        /// <typeparam name="T">The request type</typeparam>
        /// <param name="key">The key of the resolution request.</param>
        /// <returns>The resolved type.</returns>
        T TryResolveWithKey<T>(Enum key) where T : class;

        /// <summary>
        /// Try resolved the requested type with the list of paramaters specified.
        /// </summary>
        /// <typeparam name="T">The request type</typeparam>
        /// <param name="key">The key of the resolution request.</param>
        /// <returns>The resolved type.</returns>
        T TryResolveWithKey<T>(object key) where T : class;

        #endregion Methods
    }

    /// <summary>
    /// Ioc Containter with the Kernel exposed.
    /// </summary>
    /// <typeparam name="TKernel">The underlying resolution technology.</typeparam>
    public interface IIocContainer<TKernel> : IIocContainer
    {
        #region Properties

        /// <summary>
        /// Get the IOC container kernel that is the underlying resolution technology.
        /// </summary>
        public TKernel Kernel { get; }

        #endregion Properties
    }

    internal class PartialIocContainer : IIocContainer
    {
        public PartialIocContainer(IIocKernelBuilder builder)
        {
            Builder = builder ?? throw new ArgumentNullException(nameof(builder));
        }

        public IIocKernelBuilder Builder { get; }

        public void Dispose()
        {
        }

        public T Resolve<T>(params object[] parameters) where T : class
        {
            throw new NotSupportedException("Ioc has only been partially initialized.");
        }

        public T Resolve<T>(params (string, object)[] parameters) where T : class
        {
            throw new NotSupportedException("Ioc has only been partially initialized.");
        }

        public T Resolve<T>() where T : class
        {
            throw new NotSupportedException("Ioc has only been partially initialized.");
        }

        public IEnumerable ResolveAll(Type requestType)
        {
            throw new NotSupportedException("Ioc has only been partially initialized.");
        }

        public IEnumerable<T> ResolveAll<T>() where T : class
        {
            throw new NotSupportedException("Ioc has only been partially initialized.");
        }

        public T ResolveNamed<T>(string key, params object[] parameters) where T : class
        {
            throw new NotSupportedException("Ioc has only been partially initialized.");
        }

        public T ResolveNamed<T>(string name, params (string, object)[] parameters) where T : class
        {
            throw new NotSupportedException("Ioc has only been partially initialized.");
        }

        public T ResolveNamed<T>(string key) where T : class
        {
            throw new NotSupportedException("Ioc has only been partially initialized.");
        }

        public T ResolveWithKey<T>(Enum key, params object[] parameters) where T : class
        {
            throw new NotSupportedException("Ioc has only been partially initialized.");
        }

        public T ResolveWithKey<T>(Enum key, params (string, object)[] parameters) where T : class
        {
            throw new NotSupportedException("Ioc has only been partially initialized.");
        }

        public T ResolveWithKey<T>(object key, params object[] parameters) where T : class
        {
            throw new NotSupportedException("Ioc has only been partially initialized.");
        }

        public T ResolveWithKey<T>(object key, params (string, object)[] parameters) where T : class
        {
            throw new NotSupportedException("Ioc has only been partially initialized.");
        }

        public T ResolveWithKey<T>(Enum key) where T : class
        {
            throw new NotSupportedException("Ioc has only been partially initialized.");
        }

        public T ResolveWithKey<T>(object key) where T : class
        {
            throw new NotSupportedException("Ioc has only been partially initialized.");
        }

        public object TryResolve(Type requestType)
        {
            throw new NotSupportedException("Ioc has only been partially initialized.");
        }

        public T TryResolve<T>(params object[] parameters) where T : class
        {
            throw new NotSupportedException("Ioc has only been partially initialized.");
        }

        public T TryResolve<T>(params (string, object)[] parameters) where T : class
        {
            throw new NotSupportedException("Ioc has only been partially initialized.");
        }

        public T TryResolve<T>() where T : class
        {
            throw new NotSupportedException("Ioc has only been partially initialized.");
        }

        public object TryResolveNamed(Type requestType, string key, params object[] parameters)
        {
            throw new NotSupportedException("Ioc has only been partially initialized.");
        }

        public T TryResolveNamed<T>(string key, params object[] parameters) where T : class
        {
            throw new NotSupportedException("Ioc has only been partially initialized.");
        }

        public T TryResolveNamed<T>(string key, params (string, object)[] parameters) where T : class
        {
            throw new NotSupportedException("Ioc has only been partially initialized.");
        }

        public T TryResolveNamed<T>(string key) where T : class
        {
            throw new NotSupportedException("Ioc has only been partially initialized.");
        }

        public object TryResolveWithKey(Type requestType, Enum key, params object[] parameters)
        {
            throw new NotSupportedException("Ioc has only been partially initialized.");
        }

        public object TryResolveWithKey(Type requestType, object key, params object[] parameters)
        {
            throw new NotSupportedException("Ioc has only been partially initialized.");
        }

        public T TryResolveWithKey<T>(Enum key, params object[] parameters) where T : class
        {
            throw new NotSupportedException("Ioc has only been partially initialized.");
        }

        public T TryResolveWithKey<T>(Enum key, params (string, object)[] parameters) where T : class
        {
            throw new NotSupportedException("Ioc has only been partially initialized.");
        }

        public T TryResolveWithKey<T>(object key, params object[] parameters) where T : class
        {
            throw new NotSupportedException("Ioc has only been partially initialized.");
        }

        public T TryResolveWithKey<T>(object key, params (string, object)[] parameters) where T : class
        {
            throw new NotSupportedException("Ioc has only been partially initialized.");
        }

        public T TryResolveWithKey<T>(Enum key) where T : class
        {
            throw new NotSupportedException("Ioc has only been partially initialized.");
        }

        public T TryResolveWithKey<T>(object key) where T : class
        {
            throw new NotSupportedException("Ioc has only been partially initialized.");
        }
    }
}