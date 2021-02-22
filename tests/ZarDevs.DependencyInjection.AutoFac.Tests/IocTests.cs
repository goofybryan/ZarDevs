using Xunit;
using ZarDevs.DependencyInjection.Tests;

namespace ZarDevs.DependencyInjection.AutoFac.Tests
{
    public class IocTests : IocTestsConstruct<IocTestFixture>, IClassFixture<IocTestFixture>
    {
        #region Constructors

        public IocTests(IocTestFixture fixture) : base(fixture)
        {
        }

        #endregion Constructors
    }
}