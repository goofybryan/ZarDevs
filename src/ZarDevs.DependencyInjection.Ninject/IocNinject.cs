using Ninject;

namespace ZarDevs.DependencyInjection
{
    public static class IocNinject
    {
        public static IIocKernelContainer Initialize(INinjectSettings settings = null)
        {
#pragma warning disable CA2000 // Dispose objects before losing scope
            return InitializeKernel(settings);
#pragma warning restore CA2000 // Dispose objects before losing scope
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Reliability", "CA2000:Dispose objects before losing scope", Justification = "<Pending>")]
        private static IocKernelContainer InitializeKernel(INinjectSettings settings)
        {
            var kernel = new StandardKernel(settings ?? new NinjectSettings());
            return new IocKernelContainer(kernel);
        }
    }
}
