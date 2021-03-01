using System;

namespace ZarDevs.DependencyInjection.Tests
{
    public interface IIocTestFixture : IDisposable
    {
        #region Properties

        IIocContainer Container { get; }

        #endregion Properties
    }

    public interface IIocPerformanceTestFixture : IIocTestFixture
    {
        bool RunComparisonTests { get; }
    }
}