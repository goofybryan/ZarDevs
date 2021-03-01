namespace ZarDevs.DependencyInjection
{
    public interface IDependencyTypeActivator
    {
        #region Methods

        object Resolve(IDependencyTypeInfo info);

        object Resolve(IDependencyTypeInfo info, params object[] args);

        object Resolve(IDependencyTypeInfo info, params (string, object)[] args);

        #endregion Methods
    }
}