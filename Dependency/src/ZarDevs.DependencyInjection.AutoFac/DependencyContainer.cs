using Autofac;
using Autofac.Builder;
using System;

namespace ZarDevs.DependencyInjection
{
    /// <summary>
    /// AutoFac dependency container
    /// </summary>
    public interface IAutoFacDependencyContainer : IDependencyContainer
    {
        #region Properties

        /// <summary>
        /// AutoFac Container
        /// </summary>
        IContainer Container { get; }

        #endregion Properties
    }

    internal class DependencyContainer : DependencyContainerBase<ContainerBuilder>, IAutoFacDependencyContainer
    {
        #region Fields

        private readonly ContainerBuildOptions _buildOptions;
        private readonly IDependencyFactory _dependencyFactory;

        #endregion Fields

        #region Constructors

        public DependencyContainer(ContainerBuildOptions buildOptions, IDependencyFactory dependencyFactory, IDependencyScopeCompiler<ContainerBuilder> scopeCompiler) : base(new(), scopeCompiler)
        {
            _buildOptions = buildOptions;
            _dependencyFactory = dependencyFactory ?? throw new ArgumentNullException(nameof(dependencyFactory));
        }

        #endregion Constructors

        #region Properties

        public IContainer Container { get; private set; }

        #endregion Properties

        #region Methods

        protected override void OnBuildEnd()
        {
            Bindings.RegisterGeneric(typeof(MultipleResolver<>)).As(typeof(IMultipleResolver<>));
            Container = Bindings.Build(_buildOptions);
        }

        #endregion Methods
    }
}