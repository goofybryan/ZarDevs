using ZarDevs.DependencyInjection.Tests;

namespace ZarDevs.DependencyInjection.RuntimeFactory.Tests
{
    public sealed class IocPerformanceTestFixture : IIocPerformanceTestFixture
    {
        #region Constructors

        public IocPerformanceTestFixture()
        {
            Container = Ioc.Initialize(new IocKernelBuilder(), builder =>
            {
                builder.ConfigurePerformanceTest();
            });

            InstanceResolution = ((IIocContainer<IDependencyInstanceResolution>)Container).Kernel;
        }

        #endregion Constructors

        #region Properties

        public IIocContainer Container { get; }
        public IDependencyInstanceResolution InstanceResolution { get; }

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