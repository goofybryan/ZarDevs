using System;

namespace ZarDevs.DependencyInjection
{
    public sealed class Ioc : IDisposable
    {
        #region Fields

        private static IIocKernelContainer _kernel;

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

        public IIocKernelContainer Initialize(IIocKernelContainer container)
        {
            lock (this)
            {
                _kernel = container ?? throw new ArgumentNullException(nameof(container));
                return _kernel;
            }
        }

        public IDependencyBuilder InitializeWithBuilder(IIocKernelContainer container)
        {
            var dependencyContainer = Initialize(container).CreateDependencyContainer();
            return new DependencyBuilder(dependencyContainer);
        }

        #endregion Methods
    }
}