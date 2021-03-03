using System;
using ZarDevs.DependencyInjection.Tests;

namespace ZarDevs.DependencyInjection.AutoFac.Tests
{
    public sealed class IocTestFixture : IIocTestFixture
    {
        #region Constructors

        public IocTestFixture()
        {
            var kernel = IocAutoFac.Initialize();

            Container = Ioc.Initialize(kernel, builder =>
            {
                builder.ConfigureTest();
            });
        }

        public IIocContainer Container { get; }

        public bool RunComparisonTests => true;

        #endregion Constructors

        #region Methods

        public void Dispose()
        {
            Ioc.Instance.Dispose();
        }

        #endregion Methods
    }
}