using Ninject;
using System;

namespace ZarDevs.DependencyInjection
{
    internal class IocKernelBuilder : IIocKernelBuilder
    {
        #region Fields

        private readonly IDependencyContainer _container;

        #endregion Fields

        #region Constructors

        public IocKernelBuilder(IKernel kernel)
        {
            Kernel = kernel ?? throw new ArgumentNullException(nameof(kernel));
            _container = DependencyContainer.Create(Kernel);
        }

        #endregion Constructors

        #region Properties

        public IKernel Kernel { get; }

        #endregion Properties

        #region Methods

        public IDependencyBuilder CreateDependencyBuilder()
        {
            var builder = new DependencyBuilder(_container);

            builder.Bind<IDependencyContainer>().To(_container);

            return builder;
        }

        public IIocContainer CreateIocContainer()
        {
            return new IocContainer(Kernel, _container);
        }

        #endregion Methods
    }
}