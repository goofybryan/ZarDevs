using System;

namespace ZarDevs.DependencyInjection
{
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

        public static IIocContainer Container => _kernel ?? throw new InvalidOperationException("Ioc for the solution has not been initialized.");
        public static Ioc Instance { get; }

        #endregion Properties

        #region Methods

        public void Dispose()
        {
            _kernel?.Dispose();
            _kernel = null;
        }

        public static IIocContainer Initialize(IIocKernelContainer container, Action<IDependencyBuilder> buildDependencies=null, Action afterBuild=null) => Instance.InitializeInternal(container, buildDependencies, afterBuild);

        public IIocContainer InitializeInternal(IIocKernelContainer container, Action<IDependencyBuilder> buildDependencies, Action afterBuild)
        {
            lock (this)
            {
                var builder = container.CreateDependencyBuilder();

                builder.Bind<IIocContainer>()
                    .To((ctx, key) => 
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