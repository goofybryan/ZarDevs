using System;

namespace ZarDevs.DependencyInjection
{
    /// <summary>
    /// Dependency type resolution that will resolve the <see cref="IDependencyInfo.ResolvedTypes"/>
    /// and will call the <see cref="IDependencyFactoryInfo.FactoryType"/><see cref="IDependencyFactoryInfo.MethodName"/>
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
        /// <param name="activator">
        /// The activator that will be used to return an instance of the <see cref="IDependencyTypeInfo.ResolutionType"/>
        /// </param>
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

        /// <summary>
        /// Resolve and return the instance
        /// </summary>
        /// <param name="context">The dependency context.</param>
        /// <returns>An instance for this resolution.</returns>
        public override object Resolve(IDependencyContext context)
        {
            return _activator.Resolve(Info, context.GetArguments());
        }

        /// <summary>
        /// Override to make a resolution, otherwise a <see cref="NotSupportedException"/> will be thrown.
        /// </summary>
        /// <param name="genericTypeArguments">The generic type arguments.</param>
        /// <returns></returns>
        protected override IDependencyResolution OnMakeConcrete(params Type[] genericTypeArguments)
        {
            var concreteInfo = Info.As(genericTypeArguments);

            return new DependencyTypeResolution(concreteInfo, _activator);
        }

        #endregion Methods
    }
}