using System;
using ZarDevs.DependencyInjection.Tests;

namespace ZarDevs.DependencyInjection.AutoFac.Tests
{
    public sealed class IocTestFixture : IIocTests
    {
        #region Constructors

        public IocTestFixture()
        {
            var builder = Ioc.Instance.InitializeWithBuilder(IocAutoFac.Initialize());

            Bindings.ConfigureTest(builder);
            builder.Build();
            Container = Ioc.Container;
        }

        public IIocContainer Container { get; }

        #endregion Constructors

        #region Methods

        public void Dispose()
        {
            Ioc.Instance.Dispose();
        }

        #endregion Methods
    }
}