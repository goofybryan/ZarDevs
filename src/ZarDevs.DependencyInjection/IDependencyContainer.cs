using System.Collections.Generic;

namespace ZarDevs.DependencyInjection
{
    public interface IDependencyContainer
    {
        #region Methods

        void Build(IList<IDependencyInfo> definitions);

        #endregion Methods
    }
}