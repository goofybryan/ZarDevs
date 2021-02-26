using System;

namespace ZarDevs.DependencyInjection
{
    /// <summary>
    /// Bind in which scope and key is used to when resolving the request type.
    /// </summary>
    public interface IDependencyBuilderBindingMetaData : IDependencyBuilderInfo
    {
        #region Methods

        /// <summary>
        /// Create the binding in Singleton Scope resolution.
        /// </summary>
        IDependencyBuilderInfo InSingletonScope();

        /// <summary>
        /// Create the binding in Transient Scope resolution. This is the default scope.
        /// </summary>
        IDependencyBuilderInfo InTransientScope();

        #endregion Methods
    }

    /// <summary>
    /// Dependency builder binding request to start the binding.
    /// </summary>
    public interface IDependencyBuilderBindingRequest
    {
        #region Methods

        /// <summary>
        /// Bind the specified type.
        /// </summary>
        /// <param name="type">Specified type to bind as the request type.</param>
        IDependencyBuilderBindingResolve Bind(Type type);

        /// <summary>
        /// Bind the specified type.
        /// </summary>
        /// <typeparam name="T">Specified type to bind as the request type</typeparam>
        IDependencyBuilderBindingResolve Bind<T>() where T : class;

        #endregion Methods
    }

    /// <summary>
    /// One the request type has been bound in <see cref="IDependencyBuilderBindingRequest"/> the
    /// binding must be configured to a s resolution type, instance or method.
    /// </summary>
    public interface IDependencyBuilderBindingResolve
    {
        #region Methods

        /// <summary>
        /// Bind the specified type to
        /// </summary>
        /// <param name="type">Specified type to that will be resolved.</param>
        IDependencyBuilderBindingMetaData To(Type type);

        /// <summary>
        /// Bind the specified type to that will be resolved.
        /// </summary>
        /// <typeparam name="T">Specified type to that will be resolved.</typeparam>
        IDependencyBuilderBindingMetaData To<T>() where T : class;

        /// <summary>
        /// Bind the instance that will be resolved. This will be a singleton instance regardless of configuration.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="instance"></param>
        /// <returns></returns>
        IDependencyBuilderBindingMetaData To<T>(T instance);

        /// <summary>
        /// Bind to a method that will be used to reolve the request type.
        /// </summary>
        /// <param name="method">
        /// The <see cref="Func{T1, T2, TResult}"/> that will pass in a builder context and the
        /// named value if any. The result must be able to cast to the request type, <see
        /// cref="IDependencyBuilderBindingRequest.Bind(Type)"/> or <seealso cref="IDependencyBuilderBindingRequest.Bind{T}"/>
        /// </param>
        IDependencyBuilderBindingMetaData To(Func<DepencyBuilderInfoContext, object, object> method);

        #endregion Methods
    }

    /// <summary>
    /// Dependency builder information for a specific binding.
    /// </summary>
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
        /// Create the binding with the key value.
        /// </summary>
        IDependencyBuilderInfo WithKey(object key);

        #endregion Methods
    }
}