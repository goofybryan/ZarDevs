using Ninject;
using System;

namespace ZarDevs.DependencyInjection
{
    internal class DependencyContainer : DependencyContainerBase<IKernel>
    {
        #region Fields

        private readonly IDependencyFactory _dependencyFactory;

        #endregion Fields

        #region Constructors

        public DependencyContainer(IKernel kernel, IDependencyFactory dependencyFactory, IDependencyScopeCompiler<IKernel> scopeCompiler)
            : base(kernel, scopeCompiler)
        {
            Kernel = kernel ?? throw new ArgumentNullException(nameof(kernel));
            _dependencyFactory = dependencyFactory ?? throw new ArgumentNullException(nameof(dependencyFactory));
        }

        #endregion Constructors

        #region Properties

        public IKernel Kernel { get; }

        #endregion Properties

        #region Methods

        public override IDependencyInfo TryGetBinding(Type requestType, object key)
        {
            return base.TryGetBinding(requestType, key) ?? _dependencyFactory.FindFactory(requestType, key);
        }

        protected override void OnBuild(IDependencyScopeBinder<IKernel> binder, IDependencyInfo definition)
        {
            base.OnBuild(binder, definition);

            Definitions.Add(definition);
        }

        #endregion Methods
    }
}