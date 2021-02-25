using Microsoft.Extensions.DependencyInjection;
using System;
using ZarDevs.DependencyInjection.Tests;

namespace ZarDevs.DependencyInjection.AutoFac.Tests
{
    public sealed class IocTestFixture : IIocTests
    {
        #region Constructors

        public IocTestFixture()
        {
            var services = new ServiceCollection();
            services.ConfigureIocBindings(Bindings.ConfigureTest);
            services.BuildServiceProvider().ConfigureIocProvider();
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