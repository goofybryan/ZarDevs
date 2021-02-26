using System;

namespace ZarDevs.DependencyInjection
{
    public interface IIocKernelContainer
    {
        #region Methods

        IIocContainer CreateIocContainer();

        IDependencyBuilder CreateDependencyBuilder();

        #endregion Methods
    }
}