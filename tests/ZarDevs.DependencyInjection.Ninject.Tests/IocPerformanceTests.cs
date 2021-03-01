using Xunit;
using ZarDevs.DependencyInjection.Tests;
using Xunit.Abstractions;
using Ninject;

namespace ZarDevs.DependencyInjection.Ninject.Tests
{
    public class IocPerformanceTests : IocPerformanceConstruct<IocPerformanceTestFixture>, IClassFixture<IocPerformanceTestFixture>
    {
        public IocPerformanceTests(IocPerformanceTestFixture fixture, ITestOutputHelper output) : base(fixture, output)
        {
        }

        protected override T PerformanceResolveComparison<T>()
        {
            return Fixture.ContainerComparison.Get<T>();
        }

        protected override T PerformanceResolveDirect<T>()
        {
            return Fixture.ContainerDirect.Get<T>();
        }
    }
}