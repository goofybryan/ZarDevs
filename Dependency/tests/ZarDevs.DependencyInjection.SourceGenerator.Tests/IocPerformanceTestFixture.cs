using ZarDevs.DependencyInjection.Tests;
using ZarDevs.DependencyInjection.ZarIoc;
using ZarDevs.DependencyInjection.Tests.Ioc;

namespace ZarDevs.DependencyInjection.RuntimeFactory.Tests
{
    public sealed class IocPerformanceTestFixture : IIocPerformanceTestFixture
    {
        #region Constructors

        public IocPerformanceTestFixture()
        {
            DependencyInfoToResolutionMapper mapper = new(new ResolutionDefaultMapper());

            mapper.ConfigureIocTestsMappers();

            Container = Ioc.Initialize(new IocKernelBuilder(mapper), builder =>
            {
                builder.ConfigurePerformanceTest();
            });
        }

        #endregion Constructors

        #region Properties

        public IIocContainer Container { get; }

        public bool RunComparisonTests => true;

        #endregion Properties

        #region Methods

        public void Dispose()
        {
            Ioc.Instance.Dispose();
        }

        #endregion Methods
    }
}