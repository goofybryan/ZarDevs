using System;

namespace ZarDevs.DependencyInjection
{
    /// <summary>
    /// Dependency type resolution that will resolve the <see cref="IDependencyInfo.RequestType"/> and return an instance of the <see cref="IDependencyTypeInfo.ResolvedType"/>
    /// </summary>
    public class DependencyTypeResolution : DependencyResolution<IDependencyTypeInfo>
    {
        #region Fields

        private readonly IDependencyTypeActivator _activator;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Create a new instance of the dependency type resolution.
        /// </summary>
        /// <param name="info">The type information describing this resolution.</param>
        /// <param name="activator">The activator that will be used to return an instance of the <see cref="IDependencyTypeInfo.ResolvedType"/></param>
        public DependencyTypeResolution(IDependencyTypeInfo info, IDependencyTypeActivator activator) : base(info)
        {
            _activator = activator ?? throw new ArgumentNullException(nameof(activator));
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Resolve and return the instance
        /// </summary>
        /// <returns>An instance for this resolution.</returns>
        public override object Resolve()
        {
            return _activator.Resolve(Info);
        }

        /// <summary>
        /// Resolve and return the instance
        /// </summary>
        /// <param name="args">A list of ordered constructor arguments.</param>
        /// <returns>An instance for this resolution.</returns>
        public override object Resolve(object[] args)
        {
            return _activator.Resolve(Info, args);
        }

        /// <summary>
        /// Resolve and return the instance
        /// </summary>
        /// <param name="args">A list of named constructor arguments.</param>
        /// <returns>An instance for this resolution.</returns>
        public override object Resolve((string, object)[] args)
        {
            return _activator.Resolve(Info, args);
        }

        #endregion Methods
    }
}