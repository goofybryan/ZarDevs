using System;
using ZarDevs.Runtime;

namespace ZarDevs.DependencyInjection
{
    public static class IocRuntimeFactory
    {
        #region Methods

        public static IIocKernelContainer Initialize()
        {
            return new IocKernelContainer();
        }

        #endregion Methods
    }
}