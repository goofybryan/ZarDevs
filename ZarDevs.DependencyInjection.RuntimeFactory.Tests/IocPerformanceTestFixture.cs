using ZarDevs.DependencyInjection.Tests;

namespace ZarDevs.DependencyInjection.RuntimeFactory.Tests
{
    public sealed class IocPerformanceTestFixture : IIocPerformanceTestFixture
    {
        #region Constructors

        public IocPerformanceTestFixture()
        {
            Container = Ioc.Initialize(IocRuntimeFactory.Initialize(), builder =>
            {
                builder.ConfigurePerformanceTest();
            });
        }

        #endregion Constructors

        #region Properties

        public IIocContainer Container { get; }

        public bool RunComparisonTests => false;

        #endregion Properties

        #region Methods

        public void Dispose()
        {
            Ioc.Instance.Dispose();
        }

        #endregion Methods
    }
}