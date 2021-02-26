namespace ZarDevs.DependencyInjection
{
    public interface IDependencyTypeActivator
    {
        #region Methods

        object Resolve(IIocContainer ioc, IDependencyTypeInfo info);

        object Resolve(IIocContainer ioc, IDependencyTypeInfo info, params object[] args);

        object Resolve(IIocContainer ioc, IDependencyTypeInfo info, params (string, object)[] args);

        #endregion Methods
    }
}