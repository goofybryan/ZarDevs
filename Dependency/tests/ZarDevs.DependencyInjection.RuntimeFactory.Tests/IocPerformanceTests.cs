﻿using Xunit;
using Xunit.Abstractions;
using ZarDevs.DependencyInjection.Tests;

namespace ZarDevs.DependencyInjection.RuntimeFactory.Tests
{
    public class IocPerformanceTests : IocPerformanceConstruct<IocPerformanceTestFixture>, IClassFixture<IocPerformanceTestFixture>
    {
        #region Constructors

        public IocPerformanceTests(IocPerformanceTestFixture fixture, ITestOutputHelper output) : base(fixture, output)
        {
        }

        #endregion Constructors

        #region Methods

        protected override T PerformanceResolveComparison<T>()
        {
            return (T)Fixture.InstanceResolution.GetResolution(typeof(T)).Resolve();
        }

        protected override T PerformanceResolveDirect<T>()
        {
            return (T)Fixture.InstanceResolution.GetResolution(typeof(T)).Resolve();
        }

        #endregion Methods
    }
}