using Microsoft.Extensions.DependencyInjection;
using System;
using ZarDevs.DependencyInjection.Tests;

namespace ZarDevs.DependencyInjection.AutoFac.Tests
{
    public sealed class IocTestFixture : IDisposable
    {
        #region Constructors

        public IocTestFixture()
        {
            var services = new ServiceCollection();
            services.ConfigureIocBindings(Bindings.ConfigureTest);
            Services = services.BuildServiceProvider().ConfigureIocProvider();
        }

        public IServiceProvider Services { get; }

        #endregion Constructors

        #region Methods

        public void Dispose()
        {
            Ioc.Dispose();
        }

        #endregion Methods
    }
}