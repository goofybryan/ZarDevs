using Xunit;
using ZarDevs.DependencyInjection.AutoFac.Tests;
using ZarDevs.DependencyInjection.Tests;

namespace ZarDevs.DependencyInjection.Microsoft.Tests
{
    public class IocTests : IocTestsConstruct, IClassFixture<IocTestFixture>
    {
    }
}