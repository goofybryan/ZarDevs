using System;
using System.Collections.Generic;

namespace ZarDevs.DependencyInjection
{
    public interface IDependencyBuilder
    {
        #region Properties

        IDependencyContainer Container { get; }

        IList<IDependencyBuilderInfo> Definitions { get; }

        #endregion Properties

        #region Methods

        IDependencyBuilderInfo Bind(Type type);

        IDependencyBuilderInfo Bind<T>() where T : class;

        void Build();

        #endregion Methods
    }
}