using Microsoft.Extensions.DependencyInjection;
using ZarDevs.DependencyInjection.Tests;

namespace ZarDevs.DependencyInjection.Microsoft.Tests
{
    public sealed class IocTestFixture : IIocTestFixture
    {
        #region Constructors

        public IocTestFixture()
        {
            var serviceCollection = new ServiceCollection();
            var kernel = IocServiceProvider.CreateBuilder(serviceCollection);

            Ioc.StartInitialization(kernel, builder => builder.ConfigureTest());

            var serviceProvider = serviceCollection.BuildServiceProvider();
            Container = Ioc.FinializeInitialization(builder => ((IIocKernelServiceProviderBuilder)builder).ConfigureServiceProvider(serviceProvider));
        }

        #endregion Constructors

        #region Properties

        public IIocContainer Container { get; }

        #endregion Properties

        #region Methods

        public void Dispose()
        {
            Ioc.Instance.Dispose();
        }

        #endregion Methods
    }
}