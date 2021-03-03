﻿using Autofac.Builder;

namespace ZarDevs.DependencyInjection
{
    /// <summary>
    /// AutoFac initialization
    /// </summary>
    public static class IocAutoFac
    {
        #region Methods

        /// <summary>
        /// Create a new IOC Kernel container that will start the binding process.
        /// </summary>
        /// <param name="buildOptions"></param>
        /// <returns></returns>
        public static IIocKernelBuilder Initialize(ContainerBuildOptions buildOptions = ContainerBuildOptions.None)
        {
            return new IocKernelBuilder(buildOptions);
        }

        #endregion Methods
    }
}