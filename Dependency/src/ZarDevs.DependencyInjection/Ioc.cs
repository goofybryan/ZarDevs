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
        /// Initialize the IOC solution. This must be called for the IOC to work. This wraps methods <see cref="Ioc.StartInitialization(IIocKernelBuilder, Action{IDependencyBuilder})"/> and <see cref="Ioc.FinializeInitialization(Action{IIocKernelBuilder})"/>
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
        public static IIocContainer Initialize(IIocKernelBuilder container, Action<IDependencyBuilder> buildDependencies, Action<IIocKernelBuilder> afterBuild = null) => Instance.InitializeInternal(container, buildDependencies, afterBuild);

        /// <summary>
        /// Start initializing the container. This will create the IOC container and add all the bindings mapped.
        /// </summary>
        /// <param name="container">
        /// Specifiy the kernel container that housed the underlying IOC methodology.
        /// </param>
        /// <param name="buildDependencies">
        /// Specify the build dependency action to invoke, this is where you can add additional
        /// dependencies to the builder.
        /// </param>
        public static void StartInitialization(IIocKernelBuilder container, Action<IDependencyBuilder> buildDependencies) => Instance.BuildContainer(container, buildDependencies);

        /// <summary>
        /// Finialize the Ioc container initialization.
        /// </summary>
        /// <param name="afterBuild">Specify an after build action.</param>
        /// <returns></returns>
        public static IIocContainer FinializeInitialization(Action<IIocKernelBuilder> afterBuild = null) => Instance.CreateContainer(afterBuild);

        /// <summary>
        /// Dispose of the IOC implementations.
        /// </summary>
        public void Dispose()
        {

            _kernel?.Dispose();
            _kernel = null;
        }

        private IIocContainer InitializeInternal(IIocKernelBuilder container, Action<IDependencyBuilder> buildDependencies, Action<IIocKernelBuilder> afterBuild)
        {
            if (_kernel != null) return _kernel;

            BuildContainer(container, buildDependencies);

            return CreateContainer(afterBuild);
        }

        private void BuildContainer(IIocKernelBuilder container, Action<IDependencyBuilder> buildDependencies)
        {
            lock (this)
            {
                if (_kernel is not null) return;

                var builder = container.CreateDependencyBuilder();

                builder.BindFunction(ctx => Container).Resolve<IIocContainer>().InSingletonScope();

                buildDependencies?.Invoke(builder);

                builder.Build();

                _kernel = new PartialIocContainer(container);
            }
        }

        private IIocContainer CreateContainer(Action<IIocKernelBuilder> afterBuild)
        {
            lock (this)
            {
                if (_kernel is null) throw new InvalidOperationException("The kernel has not been initialized correctly. Please ensure that the kernel has initialization has started.");
                if (_kernel is not PartialIocContainer partialIoc) return _kernel;

                IIocKernelBuilder builder = partialIoc.Builder;

                afterBuild?.Invoke(builder);

                _kernel = builder.CreateIocContainer();

                return _kernel;
            }
        }

        #endregion Methods
    }
}