using System;

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
        /// <param name="type">The specified type to bind.</param>
        /// <returns></returns>
        IDependencyBuilderBindingResolve Bind(Type type);

        /// <summary>
        /// Create a new binding with the specified type.
        /// </summary>
        /// <typeparam name="T">The specified type to bind.</typeparam>
        /// <returns></returns>
        IDependencyBuilderBindingResolve Bind<T>() where T : class;

        /// <summary>
        /// Build the dependencies.
        /// </summary>
        void Build();

        #endregion Methods
    }
}