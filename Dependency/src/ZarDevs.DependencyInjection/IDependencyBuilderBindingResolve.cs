using System;

namespace ZarDevs.DependencyInjection
{
    /// <summary>
    /// One the request type has been bound in <see cref="IDependencyBuilderBindingMetaData"/> the
    /// binding must be configured to a s resolution type, instance or method.
    /// </summary>
    public interface IDependencyBuilderBindingResolve : IDependencyBuilderInfo
    {
        #region Methods

        /// <summary>
        /// Bind the specified type.
        /// </summary>
        /// <param name="type">Specified type to bind as the request type.</param>
        IDependencyBuilderBindingResolve Resolve(Type type);

        /// <summary>
        /// Bind the specified type.
        /// </summary>
        /// <param name="types">Specified a list of types to bind as the request type.</param>
        IDependencyBuilderBindingResolve Resolve(params Type[] types);

        /// <summary>
        /// Bind the specified type.
        /// </summary>
        /// <typeparam name="T">Specified type to bind as the request type</typeparam>
        IDependencyBuilderBindingResolve Resolve<T>() where T : class;

        /// <summary>
        /// Add all <code>interface</code>s and base <code>class</code>es to be resolved by this binding. This will not resolve for type <see cref="IDisposable"/>
        /// </summary>
        /// <param name="ignoredTypes">Specify a list of ignored types to also not bind to.</param>
        IDependencyBuilderInfo ResolveAll(params Type[] ignoredTypes);

        #endregion Methods
    }
}