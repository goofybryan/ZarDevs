using System;
using ZarDevs.DependencyInjection.Tests;

namespace ZarDevs.DependencyInjection.AutoFac.Tests
{
    public sealed class IocTestFixture : IDisposable
    {
        #region Constructors

        public IocTestFixture()
        {
            Kernel = (IocKernelContainer)IocNinject.Initialize();
            var builder = Ioc.InitializeWithBuilder(Kernel);

            Bindings.ConfigureTest(builder);
            builder.Build();
        }

        public IocKernelContainer Kernel { get; }

        #endregion Constructors

        #region Methods

        public void Dispose()
        {
            Ioc.Dispose();
        }

        #endregion Methods
    }
}