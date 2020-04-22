using System;
using ZarDevs.DependencyInjection.Tests;

namespace ZarDevs.DependencyInjection.AutoFac.Tests
{
    public sealed class IocTestFixture : IDisposable
    {
        #region Constructors

        public IocTestFixture()
        {
            var builder = Ioc.InitializeWithBuilder(IocNinject.Initialize());

            Bindings.ConfigureTest(builder);
            builder.Build();
        }

        #endregion Constructors

        #region Methods

        public void Dispose()
        {
        }

        #endregion Methods
    }
}