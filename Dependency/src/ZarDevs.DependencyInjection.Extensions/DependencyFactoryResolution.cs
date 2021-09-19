using System;

namespace ZarDevs.DependencyInjection
{
    /// <summary>
    /// Dependency type resolution that will resolve the <see cref="IDependencyInfo.ResolvedTypes"/>
    /// and will call the <see cref="IDependencyFactoryInfo.FactoryType"/><see cref="IDependencyFactoryInfo.MethodName"/>
    /// </summary>
    public class DependencyFactoryResolution : DependencyResolution<IDependencyFactoryInfo>
    {
        #region Fields

        private readonly IDependencyFactory _factory;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Create a new instance of the dependency type resolution.
        /// </summary>
        /// <param name="info">The type information describing this resolution.</param>
        /// <param name="factory">The dependency factory</param>
        public DependencyFactoryResolution(IDependencyFactoryInfo info, IDependencyFactory factory) : base(info)
        {
            _factory = factory ?? throw new ArgumentNullException(nameof(factory));
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Resolve and return the instance
        /// </summary>
        /// <returns>An instance for this resolution.</returns>
        public override object Resolve()
        {
            return _factory.Resolve(Info.CreateContext(Ioc.Container));
        }

        /// <summary>
        /// Resolve and return the instance
        /// </summary>
        /// <param name="args">A list of ordered constructor arguments.</param>
        /// <returns>An instance for this resolution.</returns>
        public override object Resolve(object[] args)
        {
            return _factory.Resolve(Info.CreateContext(Ioc.Container).SetArguments(args));
        }

        /// <summary>
        /// Resolve and return the instance
        /// </summary>
        /// <param name="args">A list of named constructor arguments.</param>
        /// <returns>An instance for this resolution.</returns>
        public override object Resolve((string, object)[] args)
        {
            return _factory.Resolve(Info.CreateContext(Ioc.Container).SetArguments(args));
        }

        /// <summary>
        /// Resolve and return the instance
        /// </summary>
        /// <param name="context">The dependency context.</param>
        /// <returns>An instance for this resolution.</returns>
        public override object Resolve(IDependencyContext context)
        {
            return _factory.Resolve(context);
        }

        /// <summary>
        /// Override to make a resolution, otherwise a <see cref="NotSupportedException"/> will be thrown.
        /// </summary>
        /// <param name="genericTypeArguments">The generic type arguments.</param>
        /// <returns></returns>
        protected override IDependencyResolution OnMakeConcrete(params Type[] genericTypeArguments)
        {
            var concreteInfo = Info.As(genericTypeArguments);

            var factory = _factory.MakeConcrete(concreteInfo);

            return new DependencyFactoryResolution(concreteInfo, factory);
        }

        #endregion Methods
    }
}