using System;
using ZarDevs.DependencyInjection.Tests;

namespace ZarDevs.DependencyInjection.AutoFac.Tests
{
    public sealed class IocTestFixture : IIocTestFixture, IDisposable
    {
        #region Constructors

        public IocTestFixture()
        {
            Container = Ioc.Initialize(IocRuntimeFactory.Initialize(), builder =>
            {
                builder.ConfigureTest();
            });
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