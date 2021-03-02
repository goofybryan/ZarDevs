using Xunit;
using ZarDevs.DependencyInjection.Tests;
using Xunit.Abstractions;

namespace ZarDevs.DependencyInjection.RuntimeFactory.Tests
{
    public class IocPerformanceTests : IocPerformanceConstruct<IocPerformanceTestFixture>, IClassFixture<IocPerformanceTestFixture>
    {
        public IocPerformanceTests(IocPerformanceTestFixture fixture, ITestOutputHelper output) : base(fixture, output)
        {
        }

        protected override T PerformanceResolveComparison<T>()
        {
            throw new System.NotSupportedException("This test does not support a comparison");
        }

        protected override T PerformanceResolveDirect<T>()
        {
            return (T)Fixture.InstanceResolution.GetResolution(typeof(T)).Resolve();
        }
    }
}