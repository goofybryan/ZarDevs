using Ninject;
using Xunit;
using Xunit.Abstractions;
using ZarDevs.DependencyInjection.Tests;

namespace ZarDevs.DependencyInjection.Ninject.Tests
{
    public class IocPerformanceTests : IocPerformanceConstruct<IocPerformanceTestFixture>, IClassFixture<IocPerformanceTestFixture>
    {
        #region Constructors

        public IocPerformanceTests(IocPerformanceTestFixture fixture, ITestOutputHelper output) : base(fixture, output)
        {
        }

        #endregion Constructors

        #region Methods

        protected override T PerformanceResolveComparison<T>()
        {
            return Fixture.ContainerComparison.Get<T>();
        }

        protected override T PerformanceResolveDirect<T>()
        {
            return Fixture.ContainerDirect.Get<T>();
        }

        #endregion Methods
    }
}