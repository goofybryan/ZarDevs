using System;

namespace ZarDevs.DependencyInjection
{
    // TODO BM: Add methods for getting all services e.g. IEnumerable<T> ResolveAll<T>()where T :
    // class; TODO BM: Add methods for partial parameters and resolve rest (constructor matching to
    // nearest parameter size)
    /// <summary>
    /// IOC containter used to resolved a single instance
    /// </summary>
    public interface IIocContainer : IDisposable
    {
        #region Methods

        /// <summary>
        /// Resolved the requested type with the list of paramaters specified.
        /// </summary>
        /// <typeparam name="T">The request type</typeparam>
        /// <param name="parameters">
        /// A list of ordered parameters, these must match a constructor order.
        /// </param>
        /// <returns>The resolved type.</returns>
        T Resolve<T>(params object[] parameters) where T : class;

        /// <summary>
        /// Resolved the requested type with the list of paramaters specified.
        /// </summary>
        /// <typeparam name="T">The request type</typeparam>
        /// <param name="parameters">
        /// A list of named parameters, these must match a constructor parameter name and associated types.
        /// </param>
        /// <returns>The resolved type.</returns>
        T Resolve<T>(params (string, object)[] parameters) where T : class;

        /// <summary>
        /// Resolved the requested type.
        /// </summary>
        /// <typeparam name="T">The request type</typeparam>
        /// <returns>The resolved type.</returns>
        T Resolve<T>() where T : class;

        /// <summary>
        /// Resolved the requested type with the list of paramaters specified.
        /// </summary>
        /// <param name="name">The name of the resolution request.</param>
        /// <typeparam name="T">The request type</typeparam>
        /// <param name="parameters">
        /// A list of ordered parameters, these must match a constructor order.
        /// </param>
        /// <returns>The resolved type.</returns>
        T ResolveNamed<T>(string name, params object[] parameters) where T : class;

        /// <summary>
        /// Resolved the requested type with the list of paramaters specified.
        /// </summary>
        /// <param name="name">The name of the resolution request.</param>
        /// <typeparam name="T">The request type</typeparam>
        /// <param name="parameters">
        /// A list of named parameters, these must match a constructor parameter name and associated types.
        /// </param>
        /// <returns>The resolved type.</returns>
        T ResolveNamed<T>(string name, params (string, object)[] parameters) where T : class;

        /// <summary>
        /// Resolved the requested type.
        /// </summary>
        /// <param name="name">The name of the resolution request.</param>
        /// <typeparam name="T">The request type</typeparam>
        /// <returns>The resolved type.</returns>
        T ResolveNamed<T>(string name) where T : class;

        /// <summary>
        /// Resolved the requested type with the list of paramaters specified.
        /// </summary>
        /// <param name="key">The key of the resolution request.</param>
        /// <typeparam name="T">The request type</typeparam>
        /// <param name="parameters">
        /// A list of ordered parameters, these must match a constructor order.
        /// </param>
        /// <returns>The resolved type.</returns>
        T ResolveWithKey<T>(Enum key, params object[] parameters) where T : class;

        /// <summary>
        /// Resolved the requested type with the list of paramaters specified.
        /// </summary>
        /// <param name="key">The key of the resolution request.</param>
        /// <typeparam name="T">The request type</typeparam>
        /// <param name="parameters">
        /// A list of named parameters, these must match a constructor parameter name and associated types.
        /// </param>
        /// <returns>The resolved type.</returns>
        T ResolveWithKey<T>(Enum key, params (string, object)[] parameters) where T : class;

        /// <summary>
        /// Resolved the requested type with the list of paramaters specified.
        /// </summary>
        /// <param name="key">The key of the resolution request.</param>
        /// <typeparam name="T">The request type</typeparam>
        /// <param name="parameters">
        /// A list of ordered parameters, these must match a constructor order.
        /// </param>
        /// <returns>The resolved type.</returns>
        T ResolveWithKey<T>(object key, params object[] parameters) where T : class;

        /// <summary>
        /// Resolved the requested type with the list of paramaters specified.
        /// </summary>
        /// <param name="key">The key of the resolution request.</param>
        /// <typeparam name="T">The request type</typeparam>
        /// <param name="parameters">
        /// A list of named parameters, these must match a constructor parameter name and associated types.
        /// </param>
        /// <returns>The resolved type.</returns>
        T ResolveWithKey<T>(object key, params (string, object)[] parameters) where T : class;

        /// <summary>
        /// Resolved the requested type with the list of paramaters specified.
        /// </summary>
        /// <param name="key">The key of the resolution request.</param>
        /// <typeparam name="T">The request type</typeparam>
        /// <returns>The resolved type.</returns>
        T ResolveWithKey<T>(Enum key) where T : class;

        /// <summary>
        /// Resolved the requested type with the list of paramaters specified.
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
        /// <typeparam name="T">The request type</typeparam>
        /// <param name="name">The name of the resolution request.</param>
        /// <param name="parameters">
        /// A list of named parameters, these must match a constructor order.
        /// </param>
        /// <returns>The resolved type.</returns>
        T TryResolveNamed<T>(string name, params object[] parameters) where T : class;

        /// <summary>
        /// Try resolved the requested type with the list of paramaters specified.
        /// </summary>
        /// <typeparam name="T">The request type</typeparam>
        /// <param name="name">The name of the resolution request.</param>
        /// <param name="parameters">
        /// A list of named parameters, these must match a constructor parameter name and associated types.
        /// </param>
        /// <returns>The resolved type.</returns>
        T TryResolveNamed<T>(string name, params (string, object)[] parameters) where T : class;

        /// <summary>
        /// Try resolved the requested type with the list of paramaters specified.
        /// </summary>
        /// <typeparam name="T">The request type</typeparam>
        /// <param name="name">The name of the resolution request.</param>
        /// <returns>The resolved type.</returns>
        T TryResolveNamed<T>(string name) where T : class;

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
}