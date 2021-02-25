using System;
using ZarDevs.DependencyInjection.Tests;

namespace ZarDevs.DependencyInjection.AutoFac.Tests
{
    public sealed class IocTestFixture : IIocTests, IDisposable
    {
        #region Constructors

        public IocTestFixture()
        {
            var kernel = (IocKernelContainer)IocNinject.Initialize();
            var builder = Ioc.Instance.InitializeWithBuilder(kernel);

            Bindings.ConfigureTest(builder);
            builder.Build();

            Container = kernel;
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