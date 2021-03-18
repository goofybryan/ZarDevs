using System;

namespace ZarDevs.DependencyInjection
{
    /// <summary>
    /// The over arching IOC implementation
    /// </summary>
    public sealed class Ioc : IDisposable
    {
        #region Fields

        private static IIocContainer _kernel;

        #endregion Fields

        #region Constructors

        static Ioc()
        {
            Instance = new Ioc();
        }

        private Ioc()
        {
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Get the current IOC container.
        /// </summary>
        public static IIocContainer Container => _kernel ?? throw new InvalidOperationException("Ioc for the solution has not been initialized.");

        /// <summary>
        /// IOC instance
        /// </summary>
        public static Ioc Instance { get; }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Initialize the IOC solution. This must be called for the IOC to work.
        /// </summary>
        /// <param name="container">
        /// Specifiy the kernel container that housed the underlying IOC methodology.
        /// </param>
        /// <param name="buildDependencies">
        /// Specify the build dependency action to invoke, this is where you can add additional
        /// dependencies to the builder.
        /// </param>
        /// <param name="afterBuild">Specify an after build action.</param>
        /// <returns></returns>
        public static IIocContainer Initialize(IIocKernelBuilder container, Action<IDependencyBuilder> buildDependencies, Action afterBuild = null) => Instance.InitializeInternal(container, buildDependencies, afterBuild);

        /// <summary>
        /// Dispose of the IOC implementations.
        /// </summary>
        public void Dispose()
        {
            _kernel?.Dispose();
            _kernel = null;
        }

        private IIocContainer InitializeInternal(IIocKernelBuilder container, Action<IDependencyBuilder> buildDependencies, Action afterBuild)
        {
            lock (this)
            {
                if (_kernel != null) return _kernel;

                var builder = container.CreateDependencyBuilder();

                builder.Bind<IIocContainer>()
                    .To((ctx) =>
                    Container).InSingletonScope();

                buildDependencies?.Invoke(builder);

                builder.Build();

                afterBuild?.Invoke();

                _kernel = container.CreateIocContainer();

                return _kernel;
            }
        }

        #endregion Methods
    }
}