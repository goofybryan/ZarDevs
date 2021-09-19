using System;
using System.Collections.Generic;
using System.Linq;

namespace ZarDevs.DependencyInjection
{
    /// <summary>
    /// Abstract resolution class that is implementing the base requirements of <see cref="IDependencyResolution"/>
    /// </summary>
    /// <typeparam name="TInfo">The dependency info type describing the resolution</typeparam>
    public abstract class DependencyResolution<TInfo> : IDependencyResolution<TInfo> where TInfo : IDependencyInfo
    {
        #region Constructors

        /// <summary>
        /// Base constructor
        /// </summary>
        /// <param name="info">The dependency info describing the resolution.</param>
        protected DependencyResolution(TInfo info)
        {
            if (info is null) throw new ArgumentNullException(nameof(info), "Info cannot be null");

            Info = info;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// The <see cref="IDependencyInfo"/> impementation describing this resolution.
        /// </summary>
        public TInfo Info { get; }

        /// <summary>
        /// The key that is associated to this resolution.
        /// </summary>
        public object Key => Info.Key;

        /// <summary>
        /// Get the type that this resolution is for.
        /// </summary>
        public ISet<Type> ResolvedTypes => Info.ResolvedTypes;

        #endregion Properties

        #region Methods

        /// <summary>
        /// Create a concrete resolution out of a generic.
        /// </summary>
        /// <param name="concreteRequest">The concrete request type.</param>
        /// <exception cref="InvalidOperationException">
        /// Throws when the current <see cref="IDependencyInfo.ResolvedTypes"/> is not compatible with the <paramref name="concreteRequest"/>
        /// </exception>
        /// <exception cref="NotSupportedException">
        /// Throws this when the resolution does not support making a
        /// </exception>
        /// <returns>A resolution</returns>
        public IDependencyResolution MakeConcrete(Type concreteRequest)
        {
            if (Info.ResolvedTypes.All(t => !t.IsGenericType))
                throw new InvalidOperationException($"The resolution request type '{Info}' is not a generic type.");

            if (!concreteRequest.IsConstructedGenericType)
                throw new InvalidOperationException($"The concrete request type '{Info}' does not contain generic parameters.");

            return OnMakeConcrete(concreteRequest.GenericTypeArguments);
        }

        /// <summary>
        /// Resolve and return the instance
        /// </summary>
        /// <returns>An instance for this resolution.</returns>
        public abstract object Resolve();

        /// <summary>
        /// Resolve and return the instance
        /// </summary>
        /// <param name="args">A list of named constructor arguments.</param>
        /// <returns>An instance for this resolution.</returns>
        public abstract object Resolve(params object[] args);

        /// <summary>
        /// Resolve and return the instance
        /// </summary>
        /// <param name="args">A list of ordered constructor arguments.</param>
        /// <returns>An instance for this resolution.</returns>
        public abstract object Resolve(params (string, object)[] args);

        /// <summary>
        /// Resolve and return the instance
        /// </summary>
        /// <param name="context">The dependency context.</param>
        /// <returns>An instance for this resolution.</returns>
        public abstract object Resolve(IDependencyContext context);

        /// <summary>
        /// Override to make a resolution, otherwise a <see cref="NotSupportedException"/> will be thrown.
        /// </summary>
        /// <param name="genericTypeArguments">The generic type arguments.</param>
        /// <returns></returns>
        protected virtual IDependencyResolution OnMakeConcrete(params Type[] genericTypeArguments)
        {
            throw new NotSupportedException($"This resolution '{this}' does not support making concrete versions.");
        }

        #endregion Methods
    }
}