namespace ZarDevs.DependencyInjection
{
    public interface IDependencyResolution
    {
        #region Properties

        object Key { get; }

        #endregion Properties

        #region Methods

        object Resolve(IIocContainer ioc, params object[] args);

        object Resolve(IIocContainer ioc, params (string, object)[] args);

        #endregion Methods
    }

    public interface IDependencyResolution<TInfo> : IDependencyResolution where TInfo : IDependencyInfo
    {
        #region Properties

        TInfo Info { get; }

        #endregion Properties
    }
}