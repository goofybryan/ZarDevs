using Autofac.Builder;

namespace ZarDevs.DependencyInjection
{
    public static class IocAutoFac
    {
        #region Methods

        public static IIocKernelContainer Initialize(ContainerBuildOptions buildOptions = ContainerBuildOptions.None)
        {
            return new IocKernelContainer(buildOptions);
        }

        #endregion Methods
    }
}