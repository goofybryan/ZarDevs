using System;
using System.Collections.Generic;

namespace ZarDevs.DependencyInjection
{
    public interface IDependencyContainer
    {
        #region Methods

        void Build(IList<IDependencyInfo> definitions);

        IDependencyInfo GetBinding<T>(object key);

        IDependencyInfo GetBinding(Type requestType, object key);

        #endregion Methods
    }
}