using Ninject;
using System.Collections.Generic;

namespace ZarDevs.DependencyInjection
{
    internal class IocKernelBuilder : IIocKernelBuilder
    {
        #region Fields

        private readonly IDependencyContainer _container;

        #endregion Fields

        #region Constructors

        public IocKernelBuilder(IKernel kernel, IDependencyFactory dependencyFactory, params IDependencyScopeBinder<IKernel>[] additionalBinders)
        {
            Kernel = kernel ?? throw new System.ArgumentNullException(nameof(kernel));
            List<IDependencyScopeBinder<IKernel>> binders = new List<IDependencyScopeBinder<IKernel>> { new NinjectDependencyScopeBinder(dependencyFactory) };
            binders.AddRange(additionalBinders);
            _container = new DependencyContainer(kernel, dependencyFactory, new DependencyScopeCompiler<IKernel>(binders));
        }

        #endregion Constructors

        #region Properties

        public IKernel Kernel { get; }

        #endregion Properties

        #region Methods

        public IDependencyBuilder CreateDependencyBuilder()
        {
            var builder = new DependencyBuilder(_container);

            builder.BindInstance(_container).Resolve<IDependencyContainer>();

            return builder;
        }

        public IIocContainer CreateIocContainer()
        {
            return new IocContainer(Kernel, _container);
        }

        #endregion Methods
    }
}