using System;

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
            if (info is null) throw new ArgumentNullException("Info cannot be null", nameof(info));

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
        public object Key => Info.Key ?? string.Empty;


        /// <summary>
        /// Create a concrete resolution out of a generic.
        /// </summary>
        /// <param name="concreteRequest">The concrete request type.</param>
        /// <exception cref="InvalidOperationException">Throws when the current <see cref="IDependencyInfo.RequestType"/> is not compatible with the <paramref name="concreteRequest"/></exception>
        /// <exception cref="NotSupportedException">Throws this when the resolution does not support making a</exception>
        /// <returns>A resolution </returns>
        public IDependencyResolution MakeConcrete(Type concreteRequest)
        {
            var requestType = Info.RequestType;

            if (!requestType.IsGenericType)
                throw new InvalidOperationException($"The resolution request type '{Info.RequestType}' is not a generic type.");

            if(!concreteRequest.IsConstructedGenericType)
                throw new InvalidOperationException($"The concrete request type '{Info.RequestType}' does not contain generic parameters.");

            return OnMakeConcrete(concreteRequest);
        }

        #endregion Properties

        #region Methods

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
        /// Override to make a resolution, otherwise a <see cref="NotSupportedException"/> will be thrown.
        /// </summary>
        /// <param name="concreteRequest">The concrete request type.</param>
        /// <returns></returns>
        protected virtual IDependencyResolution OnMakeConcrete(Type concreteRequest)
        {
            throw new NotSupportedException($"This resolution '{this}' does not support making concrete versions.");
        }

        #endregion Methods
    }
}