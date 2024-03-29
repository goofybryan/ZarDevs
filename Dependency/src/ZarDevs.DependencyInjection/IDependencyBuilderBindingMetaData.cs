﻿using System;

namespace ZarDevs.DependencyInjection
{
    /// <summary>
    /// Bind in which scope and key is used to when resolving the request type.
    /// </summary>
    public interface IDependencyBuilderBindingMetaData : IDependencyBuilderBindingResolve
    {
        #region Methods

        /// <summary>
        /// Bind the specified type to
        /// </summary>
        /// <param name="type">Specified type to that will be resolved.</param>
        IDependencyBuilderBindingMetaData With(Type type);

        /// <summary>
        /// Bind the specified type to that will be resolved.
        /// </summary>
        /// <typeparam name="T">Specified type to that will be resolved.</typeparam>
        IDependencyBuilderBindingMetaData With<T>() where T : class;

        /// <summary>
        /// Bind the instance that will be resolved. This will be a singleton instance regardless of configuration.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="instance"></param>
        /// <returns></returns>
        IDependencyBuilderBindingMetaData With<T>(T instance);

        /// <summary>
        /// Bind to a method that will be used to reolve the request type.
        /// </summary>
        /// <param name="method">
        /// The function <see cref="Func{T1, TResult}"/> will be executed, if any parameters are
        /// available, they will be passed in.
        /// </param>
        IDependencyBuilderBindingMetaData With(Func<IDependencyContext, object> method);

        /// <summary>
        /// Bind to a factory method that will be used to resolve the request.
        /// </summary>
        /// <typeparam name="T">The factory type</typeparam>
        /// <param name="methodName">The factory method</param>
        /// <returns></returns>
        IDependencyBuilderBindingMetaData WithFactory<T>(string methodName);

        /// <summary>
        /// Bind to a factory method that will be used to resolve the request.
        /// </summary>
        /// <param name="factoryType">The factory type</param>
        /// <param name="methodName">The factory method</param>
        /// <returns></returns>
        IDependencyBuilderBindingMetaData WithFactory(Type factoryType, string methodName);

        #endregion Methods
    }
}