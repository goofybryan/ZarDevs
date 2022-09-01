using Xunit;
using Xunit.Abstractions;
using ZarDevs.DependencyInjection.Tests;

namespace ZarDevs.DependencyInjection.RuntimeFactory.Tests
{
    public class IocPerformanceTests : IocPerformanceConstruct<IocPerformanceTestFixture>, IClassFixture<IocPerformanceTestFixture>
    {
        #region Constructors

        public IocPerformanceTests(IocPerformanceTestFixture fixture, ITestOutputHelper output) : base(fixture, output)
        {
        }

        protected override T PerformanceResolveComparison<T>()
        {
            return Ioc.Resolve<T>();
        }

        protected override T PerformanceResolveDirect<T>()
        {
            return Ioc.Resolve<T>();
        }

        #endregion Constructors
    }
}