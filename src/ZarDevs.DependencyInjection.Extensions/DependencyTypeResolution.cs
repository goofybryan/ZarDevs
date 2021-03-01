using System;

namespace ZarDevs.DependencyInjection
{
    public class DependencyTypeResolution : DependencyResolution<IDependencyTypeInfo>
    {
        #region Fields

        private readonly IDependencyTypeActivator _activator;

        #endregion Fields

        #region Constructors

        public DependencyTypeResolution(IDependencyTypeInfo info, IDependencyTypeActivator activator) : base(info)
        {
            _activator = activator ?? throw new ArgumentNullException(nameof(activator));
        }

        #endregion Constructors

        #region Methods

        public override object Resolve()
        {
            return _activator.Resolve(Info);
        }

        public override object Resolve(object[] args)
        {
            return _activator.Resolve(Info, args);
        }

        public override object Resolve((string, object)[] args)
        {
            return _activator.Resolve(Info, args);
        }

        #endregion Methods
    }
}