using System;
using ZarDevs.DependencyInjection.Tests;

namespace ZarDevs.DependencyInjection.Ninject.Tests
{
    public sealed class IocTestFixture : IIocTestFixture, IDisposable
    {
        #region Constructors

        public IocTestFixture()
        {
            var kernel = IocNinject.CreateBuilder();

            Container = Ioc.Initialize(kernel, builder =>
            {
                builder.ConfigureTest();
            });
        }

        #endregion Constructors

        #region Properties

        public IIocContainer Container { get; }

        #endregion Properties

        #region Methods

        public void Dispose()
        {
            Ioc.Instance.Dispose();
        }

        #endregion Methods
    }
}