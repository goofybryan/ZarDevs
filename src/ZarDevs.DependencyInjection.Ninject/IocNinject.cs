using Ninject;

namespace ZarDevs.DependencyInjection
{
    public static class IocNinject
    {
        public static IIocKernelBuilder Initialize(INinjectSettings settings = null)
        {
            return InitializeKernel(settings);
        }

        private static IocKernelBuilder InitializeKernel(INinjectSettings settings)
        {
            var kernel = new StandardKernel(settings ?? new NinjectSettings());
            return new IocKernelBuilder(kernel);
        }
    }
}