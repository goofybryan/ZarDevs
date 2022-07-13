namespace ZarDevs.DependencyInjection
{

    internal class MicrosoftDependencyContainer : DependencyContainer
    {
        #region Constructors

        public MicrosoftDependencyContainer(IDependencyResolutionConfiguration configuration, IDependencyScopeCompiler<IDependencyResolutionConfiguration> scopeCompiler)
            : base(configuration, scopeCompiler)
        {
        }

        #endregion Constructors
    }
}