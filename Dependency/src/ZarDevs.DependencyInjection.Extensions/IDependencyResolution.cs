using System;
using System.Collections.Generic;

namespace ZarDevs.DependencyInjection
{
    /// <summary>
    /// Define the dependency resolution and how to resolve it.
    /// </summary>
    public interface IDependencyResolution
    {
        #region Properties

        /// <summary>
        /// The key that is associated to this resolution.
        /// </summary>
        object Key { get; }

        /// <summary>
        /// Get the request type that this resolution is for.
        /// </summary>
        ISet<Type> ResolvedTypes { get; }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Create a concrete resolution out of a generic.
        /// </summary>
        /// <param name="concreteRequest">The concrete request type.</param>
        /// <returns>A resolution</returns>
        IDependencyResolution MakeConcrete(Type concreteRequest);

        /// <summary>
        /// Resolve and return the instance
        /// </summary>
        /// <returns>An instance for this resolution.</returns>
        object Resolve();

        /// <summary>
        /// Resolve and return the instance
        /// </summary>
        /// <param name="args">A list of ordered constructor arguments.</param>
        /// <returns>An instance for this resolution.</returns>
        object Resolve(object[] args);

        /// <summary>
        /// Resolve and return the instance
        /// </summary>
        /// <param name="args">A list of named constructor arguments.</param>
        /// <returns>An instance for this resolution.</returns>
        object Resolve((string, object)[] args);

        /// <summary>
        /// Resolve and return the instance
        /// </summary>
        /// <param name="context">The dependency context.</param>
        /// <returns>An instance for this resolution.</returns>
        object Resolve(IDependencyContext context);

        #endregion Methods
    }

    /// <summary>
    /// Define the dependency resolution for the <see cref="IDependencyInfo"/> implementation and
    /// how to resolve it.
    /// </summary>
    public interface IDependencyResolution<TInfo> : IDependencyResolution where TInfo : IDependencyInfo
    {
        #region Properties

        /// <summary>
        /// The <see cref="IDependencyInfo"/> impementation describing this resolution.
        /// </summary>
        TInfo Info { get; }

        #endregion Properties
    }
}