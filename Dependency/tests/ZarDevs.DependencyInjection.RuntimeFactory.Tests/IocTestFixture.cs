using System;
using ZarDevs.DependencyInjection.Tests;

namespace ZarDevs.DependencyInjection.RuntimeFactory.Tests
{
    public sealed class IocTestFixture : IIocTestFixture, IDisposable
    {
        #region Constructors

        public IocTestFixture()
        {
            Container = Ioc.Initialize(new IocKernelBuilder(), builder =>
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