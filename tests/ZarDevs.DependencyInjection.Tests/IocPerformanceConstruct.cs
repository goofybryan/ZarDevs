using System;
using Xunit;
using Xunit.Abstractions;

namespace ZarDevs.DependencyInjection.Tests
{
    [CollectionDefinition("Performance", DisableParallelization = true)]
    public abstract class IocPerformanceConstruct<TFixture> where TFixture : class, IIocPerformanceTestFixture
    {
        #region Fields

        private const int TotalRuns = 100000;
        private readonly ITestOutputHelper _output;

        #endregion Fields

        #region Constructors

        public IocPerformanceConstruct(TFixture fixture, ITestOutputHelper output)
        {
            Fixture = fixture ?? throw new ArgumentNullException(nameof(fixture));
            _output = output ?? throw new ArgumentNullException(nameof(output));
            Ioc = Fixture.Container;
        }

        #endregion Constructors

        #region Properties

        protected TFixture Fixture { get; }
        protected IIocContainer Ioc { get; }

        #endregion Properties

        #region Methods

        [Fact]
        public void PerformanceComparisonConstructorTest()
        {
            RunPerformanceTest<IPerformanceConstructTest>();
        }

        [Fact]
        public void PerformanceComparisonMethodTest()
        {
            RunPerformanceTest<IPerformanceMethodResultTest>();
        }

        [Fact]
        public void PerformanceComparisonSingletonTest()
        {
            RunPerformanceTest<IPerformanceSingletonTest>();
        }

        [Fact]
        public void PerformanceComparisonInstanceTest()
        {
            RunPerformanceTest<IPerformanceInstanceTest>();
        }

        protected abstract T PerformanceResolveComparison<T>() where T : class;
        protected abstract T PerformanceResolveDirect<T>() where T : class;

        private void AssertPerformance(TimeSpan iocTests, TimeSpan iocTestsDirect, TimeSpan iocTestsComparison)
        {
            _output.WriteLine("Total objected created {0}", TotalRuns);
            _output.WriteLine("Time taken for IOC {0} ms", iocTests.TotalMilliseconds);

            if(!Fixture.RunComparisonTests)
            {
                _output.WriteLine("No comparison available, this test is ending.");
            }
            _output.WriteLine("Time taken for IOC Direct {0} ms", iocTestsDirect.TotalMilliseconds);
            _output.WriteLine("Time taken for IOC Comparison {0} ms", iocTestsComparison.TotalMilliseconds);

            double iocTicks = iocTests.TotalMilliseconds;
            double iocDirectTicks = iocTestsDirect.TotalMilliseconds;
            double iocComparisonTicks = iocTestsComparison.TotalMilliseconds;

            var differenceIndirect = (iocTicks - iocComparisonTicks) / iocComparisonTicks * 100;
            var differenceDirect = (iocDirectTicks - iocComparisonTicks) / iocComparisonTicks * 100;

            _output.WriteLine("Difference in time percentage indirect: {0}% (This will normally be slower as there is infrastructure between the IOC resolution and the generic infrastructure).", differenceIndirect);
            _output.WriteLine("Difference in time percentage direct: {0}% (This will vary depending on how many only IOC resolves there are compared to some that require the generic infrastructure, especially methods).", differenceDirect);
        }

        private T PerformanceResolve<T>() where T : class => Ioc.Resolve<T>();

        private TimeSpan RunIocTests<T>(Func<T> creation) where T : class
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();

            for (int i = 0; i < TotalRuns; i++)
            {
                var result = creation();
                Assert.NotNull(result);
                Assert.IsAssignableFrom<T>(result);
            }

            watch.Stop();

            return watch.Elapsed;
        }

        private void RunPerformanceTest<T>() where T : class
        {
            // Act
            var iocTests = RunIocTests(PerformanceResolve<T>);

            if (Fixture.RunComparisonTests)
            {
                var iocTestsDirect = RunIocTests(PerformanceResolveDirect<T>);
                var iocTestsComparison = RunIocTests(PerformanceResolveComparison<T>);

                // Assert
                AssertPerformance(iocTests, iocTestsDirect, iocTestsComparison);
            }
            else
            {
                AssertPerformance(iocTests, TimeSpan.MinValue, TimeSpan.MinValue);
            }
        }

        #endregion Methods
    }
}