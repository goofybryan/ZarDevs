using System;
using System.Collections.Generic;

namespace ZarDevs.DependencyInjection
{
    /// <summary>
    /// Dependency builder interface that is used to start the binding process.
    /// </summary>
    public interface IDependencyBuilder
    {
        #region Methods

        /// <summary>
        /// Create a new binding with the specified type.
        /// </summary>
        /// <param name="bindType">The specified type to bind.</param>
        /// <returns></returns>
        IDependencyBuilderBindingResolve Bind(Type bindType);

        /// <summary>
        /// Create a new binding with the specified type.
        /// </summary>
        /// <typeparam name="TBind">The specified type to Bind.</typeparam>
        /// <returns></returns>
        IDependencyBuilderBindingResolve Bind<TBind>() where TBind : class;

        /// <summary>
        /// Create a new binding with the specified type.
        /// </summary>
        /// <param name="bindType">The specified type to bind.</param>
        /// <param name="resolveType">The resolved type.</param>
        /// <returns></returns>
        IDependencyBuilderInfo Bind(Type bindType, Type resolveType);

        /// <summary>
        /// Create a new binding with the specified type.
        /// </summary>
        /// <typeparam name="TBind">The specified type to Bind.</typeparam>
        /// <typeparam name="TResolve">The specified type to resolve.</typeparam>
        /// <returns></returns>
        IDependencyBuilderInfo Bind<TBind, TResolve>() where TBind : class where TResolve : class;

        /// <summary>
        /// Bind to a factory method that will be used to resolve the request.
        /// </summary>
        /// <typeparam name="T">The factory type</typeparam>
        /// <param name="methodName">The factory method</param>
        /// <returns></returns>
        IDependencyBuilderBindingResolve BindFactory<T>(string methodName);

        /// <summary>
        /// Bind to a factory method that will be used to resolve the request.
        /// </summary>
        /// <param name="factoryType">The factory type</param>
        /// <param name="methodName">The factory method</param>
        /// <returns></returns>
        IDependencyBuilderBindingResolve BindFactory(Type factoryType, string methodName);

        /// <summary>
        /// Bind to a method that will be used to reolve the request type.
        /// </summary>
        /// <param name="method">
        /// The function <see cref="Func{T1, TResult}"/> will be executed, if any parameters are
        /// available, they will be passed in.
        /// </param>
        IDependencyBuilderBindingResolve BindFunction(Func<IDependencyContext, object> method);

        /// <summary>
        /// Bind the instance that will be resolved. This will be a singleton instance regardless of configuration.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="instance"></param>
        /// <returns></returns>
        IDependencyBuilderBindingResolve BindInstance<T>(T instance);

        /// <summary>
        /// Get the definitions that are contained in this builder.
        /// </summary>
        /// <returns>A list of the current definitions</returns>
        IList<IDependencyInfo> GetDefinitions();


#endregion Methods
    }
}