using System;
using ZarDevs.DependencyInjection.Tests;

namespace ZarDevs.DependencyInjection.Ninject.Tests
{
    public sealed class IocTestFixture : IIocTestFixture, IDisposable
    {
        #region Constructors

        public IocTestFixture()
        {
            var kernel = (IocKernelContainer)IocNinject.Initialize();

            Container = Ioc.Initialize(kernel, builder =>
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