using ZarDevs.DependencyInjection.Tests;
using ZarDevs.DependencyInjection.ZarIoc;
using ZarDevs.DependencyInjection.Tests.Ioc;

namespace ZarDevs.DependencyInjection.SourceGenerator.Tests
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

            TypeFactoryContainer = ((IIocContainer<ITypeFactoryContainter>)Container).Kernel;
        }

        #endregion Constructors

        #region Properties

        public IIocContainer Container { get; }

        public ITypeFactoryContainter TypeFactoryContainer { get; }

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