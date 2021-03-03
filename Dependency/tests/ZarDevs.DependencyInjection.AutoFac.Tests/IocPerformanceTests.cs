using Xunit;
using ZarDevs.DependencyInjection.Tests;
using Autofac;
using Xunit.Abstractions;

namespace ZarDevs.DependencyInjection.AutoFac.Tests
{
    public class IocPerformanceTests : IocPerformanceConstruct<IocPerformanceTestFixture>, IClassFixture<IocPerformanceTestFixture>
    {
        public IocPerformanceTests(IocPerformanceTestFixture fixture, ITestOutputHelper output) : base(fixture, output)
        {
        }

        protected override T PerformanceResolveComparison<T>()
        {
            return Fixture.ContainerComparison.Resolve<T>();
        }

        protected override T PerformanceResolveDirect<T>()
        {
            return Fixture.DirectContainer.Resolve<T>();
        }
    }
}