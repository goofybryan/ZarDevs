using Ninject;

namespace ZarDevs.DependencyInjection
{
    public static class IocNinject
    {
        public static IIocKernelContainer Initialize(INinjectSettings settings = null)
        {
            return InitializeKernel(settings);
        }

        private static IocKernelContainer InitializeKernel(INinjectSettings settings)
        {
            var kernel = new StandardKernel(settings ?? new NinjectSettings());
            return new IocKernelContainer(kernel);
        }
    }
}