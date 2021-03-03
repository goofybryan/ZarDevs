using Ninject;

namespace ZarDevs.DependencyInjection
{
    /// <summary>
    /// IOC NInject initializing class.
    /// </summary>
    public static class IocNinject
    {
        /// <summary>
        /// Create a new instance of the IOC Kernel Builder with either the specified Ninject Setting or empty settings. A <see cref="StandardKernel"/> will be used.
        /// </summary>
        /// <param name="settings"></param>
        /// <returns></returns>
        public static IIocKernelBuilder CreateBuilder(INinjectSettings settings = null)
        {
            var kernel = new StandardKernel(settings ?? new NinjectSettings());
            return new IocKernelBuilder(kernel);
        }
    }
}