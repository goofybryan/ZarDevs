﻿using System;

namespace ZarDevs.DependencyInjection
{
    public interface IDependencyBuilderInfo
    {
        #region Properties

        /// <summary>
        /// Get the dependency info
        /// </summary>
        IDependencyInfo DependencyInfo { get; }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Bind the specified type.
        /// </summary>
        /// <param name="type">Specified type to bind as the request type.</param>
        IDependencyBuilderInfo Bind(Type type);

        /// <summary>
        /// Bind the specified type.
        /// </summary>
        /// <typeparam name="T">Specified type to bind as the request type</typeparam>
        IDependencyBuilderInfo Bind<T>() where T : class;

        /// <summary>
        /// Create the binding in Request Scope resolution.
        /// </summary>
        IDependencyBuilderInfo InRequestScope();

        /// <summary>
        /// Create the binding in Singleton Scope resolution.
        /// </summary>
        IDependencyBuilderInfo InSingletonScope();

        /// <summary>
        /// Create the binding in Transient Scope resolution.
        /// </summary>
        IDependencyBuilderInfo InTransientScope();

        /// <summary>
        /// Create the binding in Request Scope resolution.
        /// </summary>
        IDependencyBuilderInfo Named(object key);

        /// <summary>
        /// Bind the specified type to
        /// </summary>
        /// <param name="type">Specified type to that will be resolved.</param>
        IDependencyBuilderInfo To(Type type);

        /// <summary>
        /// Bind the specified type to that will be resolved.
        /// </summary>
        /// <typeparam name="T">Specified type to that will be resolved.</typeparam>
        IDependencyBuilderInfo To<T>() where T : class;

        /// <summary>
        /// Bind to a method that will be used to reolve the request type.
        /// </summary>
        /// <param name="method">The <see cref="Func{T1, T2, TResult}"/> that will pass in a builder context and the named value if any. The result must be able to cast to the request type, <see cref="Bind(Type)"/> or <seealso cref="Bind{T}"/></param>
        IDependencyBuilderInfo To(Func<DepencyBuilderInfoContext, object, object> method);

        #endregion Methods
    }
}