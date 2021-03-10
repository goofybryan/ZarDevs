using System;

namespace ZarDevs.DependencyInjection.Tests
{
    public interface IIocPerformanceTestFixture : IIocTestFixture
    {
        #region Properties

        bool RunComparisonTests { get; }

        #endregion Properties
    }

    public interface IIocTestFixture : IDisposable
    {
        #region Properties

        IIocContainer Container { get; }

        #endregion Properties
    }
}