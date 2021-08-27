using Ninject;

namespace ZarDevs.DependencyInjection
{
    internal class IocKernelBuilder : IIocKernelBuilder
    {
        #region Fields

        private readonly IDependencyContainer _container;

        #endregion Fields

        #region Constructors

        public IocKernelBuilder(IKernel kernel, IDependencyFactory dependencyFactory)
        {
            Kernel = kernel ?? throw new System.ArgumentNullException(nameof(kernel));
            _container = DependencyContainer.Create(kernel, dependencyFactory);
        }

        #endregion Constructors

        #region Properties

        public IKernel Kernel { get; }

        #endregion Properties

        #region Methods

        public IDependencyBuilder CreateDependencyBuilder()
        {
            var builder = new DependencyBuilder(_container);

            builder.Resolve<IDependencyContainer>().With(_container);

            return builder;
        }

        public IIocContainer CreateIocContainer()
        {
            return new IocContainer(Kernel, _container);
        }

        #endregion Methods
    }
}