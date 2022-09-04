using ZarDevs.DependencyInjection.Tests;
using ZarDevs.DependencyInjection.ZarIoc;
using ZarDevs.DependencyInjection.Tests.Ioc;

namespace ZarDevs.DependencyInjection.SourceGenerator.Tests
{
    public sealed class IocTestFixture : IIocTestFixture, IDisposable
    {
        #region Constructors

        public IocTestFixture()
        {
            DependencyInfoToResolutionMapper mapper = new(new ResolutionDefaultMapper());

            mapper.ConfigureIocTestsMappers();

            Container = Ioc.Initialize(new IocKernelBuilder(mapper), builder =>
            {
                builder.ConfigureTest();
            });
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