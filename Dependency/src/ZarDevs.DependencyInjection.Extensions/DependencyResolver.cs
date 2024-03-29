﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ZarDevs.DependencyInjection
{
    /// <summary>
    /// Dependency resolver used when a custom resololution is required.
    /// </summary>
    public interface IDependencyResolver : IIocContainer<IDependencyInstanceResolution>
    {
        #region Methods

        /// <summary>
        /// Try and get all the resolutions for the type <typeparamref name="T"/>
        /// </summary>
        /// <typeparam name="T">The type to resolve</typeparam>
        /// <returns></returns>
        IDependencyResolutions TryGetAllResolutions<T>();

        /// <summary>
        /// Try and get all the resolutions for the type <paramref name="typeToResolve"/>
        /// </summary>
        /// <param name="typeToResolve">The type to resolve</param>
        /// <returns></returns>
        IDependencyResolutions TryGetAllResolutions(Type typeToResolve);

        /// <summary>
        /// Try and get the resolution for the type <typeparamref name="T"/>
        /// </summary>
        /// <typeparam name="T">The type to resolve</typeparam>
        /// <param name="key">An optional key, to only resolve those.</param>
        /// <returns></returns>
        IDependencyResolution TryGetResolution<T>(object key = null);

        /// <summary>
        /// Try and get the resolution for the type <paramref name="typeToResolve"/>
        /// </summary>
        /// <param name="typeToResolve">The type to resolve</param>
        /// <param name="key">An optional key, to only resolve those.</param>
        /// <returns></returns>
        IDependencyResolution TryGetResolution(Type typeToResolve, object key = null);

        #endregion Methods
    }

    /// <summary>
    /// Dependency resolver used when a custom resololution is required.
    /// </summary>
    public class DependencyResolver : IDependencyResolver
    {
        #region Fields

        private bool _isDisposed;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Create a new instance of the dependency resolution class
        /// </summary>
        /// <param name="instanceResolution">The dependecy resolution container.</param>
        public DependencyResolver(IDependencyInstanceResolution instanceResolution)
        {
            Kernel = instanceResolution ?? throw new ArgumentNullException(nameof(instanceResolution));
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Get the IOC container kernel that is the underlying resolution technology.
        /// </summary>
        public IDependencyInstanceResolution Kernel { get; }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Dispose of the resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// ThreadTracker the requested type with the list of paramaters specified.
        /// </summary>
        /// <typeparam name="T">The request type</typeparam>
        /// <param name="parameters">
        /// A list of ordered parameters, these must match a constructor order.
        /// </param>
        /// <returns>The resolved type.</returns>
        public T Resolve<T>(params object[] parameters) where T : class
        {
            return (T)Kernel.GetResolution(typeof(T)).Resolve(parameters);
        }

        /// <summary>
        /// ThreadTracker the requested type with the list of paramaters specified.
        /// </summary>
        /// <typeparam name="T">The request type</typeparam>
        /// <param name="parameters">
        /// A list of named parameters, these must match a constructor parameter name and associated types.
        /// </param>
        /// <returns>The resolved type.</returns>
        public T Resolve<T>(params (string, object)[] parameters) where T : class
        {
            return (T)Kernel.GetResolution(typeof(T)).Resolve(parameters);
        }

        /// <summary>
        /// ThreadTracker the requested type.
        /// </summary>
        /// <typeparam name="T">The request type</typeparam>
        /// <returns>The resolved type.</returns>
        public T Resolve<T>() where T : class
        {
            return (T)Kernel.GetResolution(typeof(T)).Resolve();
        }

        /// <summary>
        /// Resolve all instance of the requested type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The request type</typeparam>
        /// <returns>The resolved type.</returns>
        public IEnumerable<T> ResolveAll<T>() where T : class
        {
            return Kernel.GetAllResolutions(typeof(T)).Resolve<T>();
        }

        /// <summary>
        /// Resolve all instance of the requested type <paramref name="requestType"/>.
        /// </summary>
        /// <param name="requestType">The request type to resolve,</param>
        /// <returns>The resolved IEnumberable as an object (can safely be cast to IEnumerable).</returns>
        public IEnumerable ResolveAll(Type requestType)
        {
            return Kernel.GetAllResolutions(requestType).Resolve();
        }

        /// <summary>
        /// ThreadTracker the requested type with the list of paramaters specified.
        /// </summary>
        /// <param name="key">The name of the resolution request.</param>
        /// <typeparam name="T">The request type</typeparam>
        /// <param name="parameters">
        /// A list of ordered parameters, these must match a constructor order.
        /// </param>
        /// <returns>The resolved type.</returns>
        public T ResolveNamed<T>(string key, params object[] parameters) where T : class
        {
            return (T)Resolve(key, typeof(T), parameters);
        }

        /// <summary>
        /// ThreadTracker the requested type with the list of paramaters specified.
        /// </summary>
        /// <param name="key">The name of the resolution request.</param>
        /// <typeparam name="T">The request type</typeparam>
        /// <param name="parameters">
        /// A list of named parameters, these must match a constructor parameter name and associated types.
        /// </param>
        /// <returns>The resolved type.</returns>
        public T ResolveNamed<T>(string key, params (string, object)[] parameters) where T : class
        {
            return (T)Resolve(key, typeof(T), parameters);
        }

        /// <summary>
        /// ThreadTracker the requested type.
        /// </summary>
        /// <param name="key">The name of the resolution request.</param>
        /// <typeparam name="T">The request type</typeparam>
        /// <returns>The resolved type.</returns>
        public T ResolveNamed<T>(string key) where T : class
        {
            return (T)Kernel.GetResolution(key, typeof(T)).Resolve();
        }

        /// <summary>
        /// ThreadTracker the requested type with the list of paramaters specified.
        /// </summary>
        /// <param name="key">The key of the resolution request.</param>
        /// <typeparam name="T">The request type</typeparam>
        /// <param name="parameters">
        /// A list of ordered parameters, these must match a constructor order.
        /// </param>
        /// <returns>The resolved type.</returns>
        public T ResolveWithKey<T>(Enum key, params object[] parameters) where T : class
        {
            return (T)Resolve(key, typeof(T), parameters);
        }

        /// <summary>
        /// ThreadTracker the requested type with the list of paramaters specified.
        /// </summary>
        /// <param name="key">The key of the resolution request.</param>
        /// <typeparam name="T">The request type</typeparam>
        /// <param name="parameters">
        /// A list of named parameters, these must match a constructor parameter name and associated types.
        /// </param>
        /// <returns>The resolved type.</returns>
        public T ResolveWithKey<T>(object key, params object[] parameters) where T : class
        {
            return (T)Resolve(key, typeof(T), parameters);
        }

        /// <summary>
        /// ThreadTracker the requested type with the list of paramaters specified.
        /// </summary>
        /// <param name="key">The key of the resolution request.</param>
        /// <typeparam name="T">The request type</typeparam>
        /// <param name="parameters">
        /// A list of ordered parameters, these must match a constructor order.
        /// </param>
        /// <returns>The resolved type.</returns>
        public T ResolveWithKey<T>(Enum key, params (string, object)[] parameters) where T : class
        {
            return (T)Resolve(key, typeof(T), parameters);
        }

        /// <summary>
        /// ThreadTracker the requested type with the list of paramaters specified.
        /// </summary>
        /// <param name="key">The key of the resolution request.</param>
        /// <typeparam name="T">The request type</typeparam>
        /// <param name="parameters">
        /// A list of named parameters, these must match a constructor parameter name and associated types.
        /// </param>
        /// <returns>The resolved type.</returns>
        public T ResolveWithKey<T>(object key, params (string, object)[] parameters) where T : class
        {
            return (T)Resolve(key, typeof(T), parameters);
        }

        /// <summary>
        /// ThreadTracker the requested type with the list of paramaters specified.
        /// </summary>
        /// <param name="key">The key of the resolution request.</param>
        /// <typeparam name="T">The request type</typeparam>
        /// <returns>The resolved type.</returns>
        public T ResolveWithKey<T>(Enum key) where T : class
        {
            return (T)Kernel.GetResolution(key, typeof(T)).Resolve();
        }

        /// <summary>
        /// ThreadTracker the requested type with the list of paramaters specified.
        /// </summary>
        /// <param name="key">The key of the resolution request.</param>
        /// <typeparam name="T">The request type</typeparam>
        /// <returns>The resolved type.</returns>
        public T ResolveWithKey<T>(object key) where T : class
        {
            return (T)Kernel.GetResolution(key, typeof(T)).Resolve();
        }

        /// <summary>
        /// Try and get all the resolutions for the type <typeparamref name="T"/>
        /// </summary>
        /// <typeparam name="T">The type to resolve</typeparam>
        /// <returns></returns>
        public IDependencyResolutions TryGetAllResolutions<T>()
        {
            return Kernel.GetAllResolutions(typeof(T));
        }

        /// <summary>
        /// Try and get all the resolutions for the type <paramref name="typeToResolve"/>
        /// </summary>
        /// <param name="typeToResolve">The type to resolve</param>
        /// <returns></returns>
        public IDependencyResolutions TryGetAllResolutions(Type typeToResolve)
        {
            return Kernel.GetAllResolutions(typeToResolve);
        }

        /// <summary>
        /// Try and get the resolution for the type <typeparamref name="T"/>
        /// </summary>
        /// <typeparam name="T">The type to resolve</typeparam>
        /// <param name="key">An optional key, to only resolve those.</param>
        /// <returns></returns>
        public IDependencyResolution TryGetResolution<T>(object key = null)
        {
            Type typeToResolve = typeof(T);
            return key == null ? Kernel.TryGetResolution(typeToResolve) : Kernel.TryGetResolution(key, typeToResolve);
        }

        /// <summary>
        /// Try and get the resolution for the type <paramref name="typeToResolve"/>
        /// </summary>
        /// <param name="typeToResolve">The type to resolve</param>
        /// <param name="key">An optional key, to only resolve those.</param>
        /// <returns></returns>
        public IDependencyResolution TryGetResolution(Type typeToResolve, object key = null)
        {
            return key == null ? Kernel.TryGetResolution(typeToResolve) : Kernel.TryGetResolution(key, typeToResolve);
        }

        /// <summary>
        /// Try resolved the requested type with the list of paramaters specified.
        /// </summary>
        /// <param name="requestType">The request type</param>
        /// <returns>The resolved type.</returns>
        public object TryResolve(Type requestType)
        {
            return Kernel.TryGetResolution(requestType).Resolve();
        }

        /// <summary>
        /// Try resolved the requested type with the list of paramaters specified.
        /// </summary>
        /// <typeparam name="T">The request type</typeparam>
        /// <param name="parameters">
        /// A list of ordered parameters, these must match a constructor order.
        /// </param>
        /// <returns>The resolved type.</returns>
        public T TryResolve<T>(params object[] parameters) where T : class
        {
            return (T)Kernel.TryGetResolution(typeof(T)).Resolve(parameters);
        }

        /// <summary>
        /// Try resolved the requested type with the list of paramaters specified.
        /// </summary>
        /// <typeparam name="T">The request type</typeparam>
        /// <param name="parameters">
        /// A list of named parameters, these must match a constructor parameter name and associated types.
        /// </param>
        /// <returns>The resolved type.</returns>
        public T TryResolve<T>(params (string, object)[] parameters) where T : class
        {
            return (T)Kernel.TryGetResolution(typeof(T)).Resolve(parameters);
        }

        /// <summary>
        /// Try resolved the requested type with the list of paramaters specified.
        /// </summary>
        /// <typeparam name="T">The request type</typeparam>
        /// <returns>The resolved type.</returns>
        public T TryResolve<T>() where T : class
        {
            return (T)Kernel.TryGetResolution(typeof(T)).Resolve();
        }

        /// <summary>
        /// Try resolved the requested type with the list of paramaters specified.
        /// </summary>
        /// <typeparam name="T">The request type</typeparam>
        /// <param name="key">The name of the resolution request.</param>
        /// <param name="parameters">
        /// A list of named parameters, these must match a constructor order.
        /// </param>
        /// <returns>The resolved type.</returns>
        public T TryResolveNamed<T>(string key, params object[] parameters) where T : class
        {
            return (T)TryResolve(key, typeof(T), parameters);
        }

        /// <summary>
        /// Try resolved the requested type with the list of paramaters specified.
        /// </summary>
        /// <typeparam name="T">The request type</typeparam>
        /// <param name="key">The name of the resolution request.</param>
        /// <param name="parameters">
        /// A list of named parameters, these must match a constructor parameter name and associated types.
        /// </param>
        /// <returns>The resolved type.</returns>
        public T TryResolveNamed<T>(string key, params (string, object)[] parameters) where T : class
        {
            return (T)TryResolve(key, typeof(T), parameters);
        }

        /// <summary>
        /// Try resolved the requested type with the list of paramaters specified.
        /// </summary>
        /// <typeparam name="T">The request type</typeparam>
        /// <param name="key">The name of the resolution request.</param>
        /// <returns>The resolved type.</returns>
        public T TryResolveNamed<T>(string key) where T : class
        {
            return (T)Kernel.TryGetResolution(key, typeof(T)).Resolve();
        }

        /// <summary>
        /// Try resolved the requested type with the list of paramaters specified.
        /// </summary>
        /// <param name="requestType">The request type</param>
        /// <param name="key">The name of the resolution request.</param>
        /// <param name="parameters">
        /// A list of named parameters, these must match a constructor order.
        /// </param>
        /// <returns>The resolved type.</returns>
        public object TryResolveNamed(Type requestType, string key, params object[] parameters)
        {
            return Kernel.TryGetResolution(key, requestType).Resolve(parameters);
        }

        /// <summary>
        /// Try resolved the requested type with the list of paramaters specified.
        /// </summary>
        /// <typeparam name="T">The request type</typeparam>
        /// <param name="key">The key of the resolution request.</param>
        /// <param name="parameters">
        /// A list of named parameters, these must match a constructor order.
        /// </param>
        /// <returns>The resolved type.</returns>
        public T TryResolveWithKey<T>(Enum key, params object[] parameters) where T : class
        {
            return (T)TryResolve(key, typeof(T), parameters);
        }

        /// <summary>
        /// Try resolved the requested type with the list of paramaters specified.
        /// </summary>
        /// <typeparam name="T">The request type</typeparam>
        /// <param name="key">The key of the resolution request.</param>
        /// <param name="parameters">
        /// A list of named parameters, these must match a constructor parameter name and associated types.
        /// </param>
        /// <returns>The resolved type.</returns>
        public T TryResolveWithKey<T>(object key, params object[] parameters) where T : class
        {
            return (T)TryResolve(key, typeof(T), parameters);
        }

        /// <summary>
        /// Try resolved the requested type with the list of paramaters specified.
        /// </summary>
        /// <typeparam name="T">The request type</typeparam>
        /// <param name="key">The key of the resolution request.</param>
        /// <param name="parameters">
        /// A list of named parameters, these must match a constructor order.
        /// </param>
        /// <returns>The resolved type.</returns>
        public T TryResolveWithKey<T>(Enum key, params (string, object)[] parameters) where T : class
        {
            return (T)TryResolve(key, typeof(T), parameters);
        }

        /// <summary>
        /// Try resolved the requested type with the list of paramaters specified.
        /// </summary>
        /// <typeparam name="T">The request type</typeparam>
        /// <param name="key">The key of the resolution request.</param>
        /// <param name="parameters">
        /// A list of named parameters, these must match a constructor parameter name and associated types.
        /// </param>
        /// <returns>The resolved type.</returns>
        public T TryResolveWithKey<T>(object key, params (string, object)[] parameters) where T : class
        {
            return (T)TryResolve(key, typeof(T), parameters);
        }

        /// <summary>
        /// Try resolved the requested type with the list of paramaters specified.
        /// </summary>
        /// <typeparam name="T">The request type</typeparam>
        /// <param name="key">The key of the resolution request.</param>
        /// <returns>The resolved type.</returns>
        public T TryResolveWithKey<T>(Enum key) where T : class
        {
            return (T)Kernel.TryGetResolution(key, typeof(T)).Resolve();
        }

        /// <summary>
        /// Try resolved the requested type with the list of paramaters specified.
        /// </summary>
        /// <typeparam name="T">The request type</typeparam>
        /// <param name="key">The key of the resolution request.</param>
        /// <returns>The resolved type.</returns>
        public T TryResolveWithKey<T>(object key) where T : class
        {
            return (T)Kernel.TryGetResolution(key, typeof(T)).Resolve();
        }

        /// <summary>
        /// Try resolved the requested type with the list of paramaters specified.
        /// </summary>
        /// <param name="requestType">The request type</param>
        /// <param name="key">The name of the resolution request.</param>
        /// <param name="parameters">
        /// A list of named parameters, these must match a constructor order.
        /// </param>
        /// <returns>The resolved type.</returns>
        public object TryResolveWithKey(Type requestType, Enum key, params object[] parameters)
        {
            return Kernel.TryGetResolution(key, requestType).Resolve();
        }

        /// <summary>
        /// Try resolved the requested type with the list of paramaters specified.
        /// </summary>
        /// <param name="requestType">The request type</param>
        /// <param name="key">The name of the resolution request.</param>
        /// <param name="parameters">
        /// A list of named parameters, these must match a constructor order.
        /// </param>
        /// <returns>The resolved type.</returns>
        public object TryResolveWithKey(Type requestType, object key, params object[] parameters)
        {
            return Kernel.TryGetResolution(key, requestType).Resolve();
        }

        /// <summary>
        /// Dispose of any resources
        /// </summary>
        /// <param name="disposing">Indicate that the method was called during the dispose method.</param>
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
            return Kernel.TryGetResolution(key, requestType).Resolve(parameters);
        }

        private object TryResolve(object key, Type requestType, params (string, object)[] parameters)
        {
            return Kernel.TryGetResolution(key, requestType).Resolve(parameters);
        }

        #endregion Methods
    }
}