using System.ComponentModel;
using ZarDevs.DependencyInjection.Tests;
using ZarDevs.DependencyInjection.ZarIoc;

namespace ZarDevs.DependencyInjection.SourceGenerator.Tests
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