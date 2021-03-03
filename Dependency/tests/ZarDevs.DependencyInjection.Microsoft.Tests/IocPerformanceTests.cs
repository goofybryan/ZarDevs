using Xunit;
using ZarDevs.DependencyInjection.Tests;
using Xunit.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace ZarDevs.DependencyInjection.Microsoft.Tests
{
    public class IocPerformanceTests : IocPerformanceConstruct<IocPerformanceTestFixture>, IClassFixture<IocPerformanceTestFixture>
    {
        public IocPerformanceTests(IocPerformanceTestFixture fixture, ITestOutputHelper output) : base(fixture, output)
        {
        }

        protected override T PerformanceResolveComparison<T>()
        {
            return Fixture.ContainerComparison.GetRequiredService<T>();
        }

        protected override T PerformanceResolveDirect<T>()
        {
            return Fixture.ContainerDirect.GetRequiredService<T>();
        }
    }
}