using Microsoft.Extensions.DependencyInjection;

namespace ZarDevs.DependencyInjection
{
    /// <summary>
    /// Ioc service provider that is used to create a Ioc Kernel builder.
    /// </summary>
    public static class IocServiceProvider
    {
        #region Methods

        /// <summary>
        /// Create a new instance of the Ioc kernel builder
        /// </summary>
        /// <param name="services">The service collection that will be used for the bindings.</param>
        public static IIocKernelServiceProviderBuilder CreateBuilder(IServiceCollection services) => new IocKernelBuilder(services);

        #endregion Methods
    }
}