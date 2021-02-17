using Xunit;
using ZarDevs.DependencyInjection.Tests;

namespace ZarDevs.DependencyInjection.AutoFac.Tests
{
    public class IocTests : IocTestsConstruct, IClassFixture<IocTestFixture>
    {
    }
}