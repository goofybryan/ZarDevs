namespace ZarDevs.DependencyInjection
{
    public interface IDependencyResolution
    {
        #region Properties

        object Key { get; }

        #endregion Properties

        #region Methods

        object Resolve();

        object Resolve(object[] args);

        object Resolve((string, object)[] args);

        #endregion Methods
    }

    public interface IDependencyResolution<TInfo> : IDependencyResolution where TInfo : IDependencyInfo
    {
        #region Properties

        TInfo Info { get; }

        #endregion Properties
    }
}