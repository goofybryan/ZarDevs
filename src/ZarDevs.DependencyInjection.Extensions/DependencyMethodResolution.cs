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

        public override object Resolve(object[] args)
        {
            return Info.Execute(args);
        }

        public override object Resolve((string, object)[] args)
        {
            return Info.Execute(args);
        }

        public override object Resolve()
        {
            return Info.Execute();
        }

        #endregion Methods
    }
}