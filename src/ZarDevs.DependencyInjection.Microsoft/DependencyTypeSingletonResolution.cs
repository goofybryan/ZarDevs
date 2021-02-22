namespace ZarDevs.DependencyInjection
{
    public class DependencyTypeSingletonResolution : DependencyTypeResolution
    {
        #region Constructors

        public DependencyTypeSingletonResolution(IDependencyTypeInfo info) : base(info)
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