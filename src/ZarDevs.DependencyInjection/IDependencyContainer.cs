using System;
using System.Collections.Generic;

namespace ZarDevs.DependencyInjection
{
    public interface IDependencyContainer
    {
        #region Methods

        void Build(IList<IDependencyInfo> definitions);

        IDependencyInfo TryGetBinding<T>(object key);

        IDependencyInfo TryGetBinding(Type requestType, object key);

        #endregion Methods
    }
}