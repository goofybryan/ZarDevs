using Microsoft.Extensions.DependencyInjection;
using System;
using ZarDevs.DependencyInjection.Tests;

namespace ZarDevs.DependencyInjection.AutoFac.Tests
{
    public sealed class IocTestFixture : IDisposable
    {
        private IServiceCollection _services;
        #region Constructors

        public IocTestFixture()
        {
            _services = new ServiceCollection();
            _services.ConfigureIocBindings(Bindings.ConfigureTest);
            _services.BuildServiceProvider().ConfigureIocProvider();
        }

        #endregion Constructors

        #region Methods

        public void Dispose()
        {
            Ioc.Dispose();
        }

        #endregion Methods
    }
}