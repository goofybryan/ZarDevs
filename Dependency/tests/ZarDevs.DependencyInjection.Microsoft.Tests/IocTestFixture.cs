using Microsoft.Extensions.DependencyInjection;
using ZarDevs.DependencyInjection.Tests;

namespace ZarDevs.DependencyInjection.Microsoft.Tests
{
    public sealed class IocTestFixture : IIocTestFixture
    {
        #region Constructors

        public IocTestFixture()
        {
            var services = new ServiceCollection();

            var kernel = IocServiceProvider.CreateBuilder(services);

            Container = Ioc.Initialize(kernel,
                builder => builder.ConfigureTest(),
                () => kernel.ConfigureServiceProvider(services.BuildServiceProvider())
            );
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