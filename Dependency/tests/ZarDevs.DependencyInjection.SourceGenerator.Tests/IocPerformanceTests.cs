using Xunit;
using Xunit.Abstractions;
using ZarDevs.DependencyInjection.Tests;

namespace ZarDevs.DependencyInjection.SourceGenerator.Tests
{
    public class IocPerformanceTests : IocPerformanceConstruct<IocPerformanceTestFixture>, IClassFixture<IocPerformanceTestFixture>
    {
        #region Constructors

        public IocPerformanceTests(IocPerformanceTestFixture fixture, ITestOutputHelper output) : base(fixture, output)
        {
        }

        protected override T PerformanceResolveComparison<T>()
        {
            return (T)Fixture.TypeFactoryContainer.Get(typeof(T)).Resolve();
        }

        protected override T PerformanceResolveDirect<T>()
        {
            return (T)Fixture.TypeFactoryContainer.Get(typeof(T)).Resolve();
        }

        #endregion Constructors
    }
}