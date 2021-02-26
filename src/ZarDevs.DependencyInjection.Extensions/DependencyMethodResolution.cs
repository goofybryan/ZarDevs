namespace ZarDevs.DependencyInjection
{
    public class DependencyMethodResolution : DependencyResolution<IDependencyMethodInfo>
    {
        #region Constructors

        public DependencyMethodResolution(IDependencyMethodInfo info) : base(info)
        {
        }

        #endregion Constructors

        #region Methods

        public override object Resolve(IIocContainer ioc, params object[] args)
        {
            return Info.Method(new DepencyBuilderInfoContext(ioc, args), Key);
        }

        public override object Resolve(IIocContainer ioc, params (string, object)[] args)
        {
            return Info.Method(new DepencyBuilderInfoContext(ioc, args), Key);
        }

        public override object Resolve(IIocContainer ioc)
        {
            return Info.Method(new DepencyBuilderInfoContext(ioc), Key);
        }

        #endregion Methods
    }
}