using System;

namespace ZarDevs.DependencyInjection
{
    public interface IIocKernelContainer : IIocContainer, IDisposable
    {
        #region Methods

        IDependencyContainer CreateDependencyContainer();

        #endregion Methods
    }
}