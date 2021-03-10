using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Xunit.Abstractions;
using ZarDevs.DependencyInjection.Tests;

namespace ZarDevs.DependencyInjection.Microsoft.Tests
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
            return Fixture.ContainerComparison.GetRequiredService<T>();
        }

        protected override T PerformanceResolveDirect<T>()
        {
            return Fixture.ContainerDirect.GetRequiredService<T>();
        }

        #endregion Methods
    }
}