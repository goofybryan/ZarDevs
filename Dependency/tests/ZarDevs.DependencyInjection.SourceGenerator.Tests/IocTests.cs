﻿using Xunit;
using ZarDevs.DependencyInjection.Tests;

namespace ZarDevs.DependencyInjection.RuntimeFactory.Tests
{
    public class IocTests : IocTestsConstruct<IocTestFixture>, IClassFixture<IocTestFixture>
    {
        #region Constructors

        public IocTests(IocTestFixture fixture) : base(fixture)
        {
        }

        #endregion Constructors
    }
}