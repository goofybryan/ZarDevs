namespace ZarDevs.DependencyInjection
{
    public class DependencyMethodSingletonResolution : DependencyMethodResolution
    {
        #region Constructors

        public DependencyMethodSingletonResolution(IDependencyMethodInfo info) : base(info)
        {
        }

        #endregion Constructors

        #region Properties

        public object Resolved { get; private set; }

        #endregion Properties

        #region Methods

        public override object Resolve(IIocContainer ioc, params object[] args)
        {
            return Resolved ??= base.Resolve(ioc, args);
        }

        #endregion Methods
    }
}