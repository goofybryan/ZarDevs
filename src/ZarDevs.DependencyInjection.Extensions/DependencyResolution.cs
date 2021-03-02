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

        #endregion Methods
    }
}