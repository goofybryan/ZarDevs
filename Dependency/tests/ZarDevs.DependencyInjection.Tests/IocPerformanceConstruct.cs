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
            _output.WriteLine("Time taken for IOC with generic infrastructure {0} ms", iocTestsDirect.TotalMilliseconds);

            if (Fixture.RunComparisonTests)
            {
                _output.WriteLine("Time taken for IOC with non-generic infrasturcture {0} ms", iocTestsComparison.TotalMilliseconds);
            }

            double iocTicks = iocTests.TotalMilliseconds;
            double iocDirectTicks = iocTestsDirect.TotalMilliseconds;

            var differenceOfGet = (iocTicks - iocDirectTicks) / iocDirectTicks * 100;
            _output.WriteLine("Difference of call IOC and generic binded IOC: {0}% (This generally will show the overhead that is added, in some case just a simple property get call adds a lot of cycles.).", differenceOfGet);

            if (!Fixture.RunComparisonTests)
            {
                _output.WriteLine("No non generic comparison available due to no third party software being used.");
                return;
            }

            double iocComparisonTicks = iocTestsComparison.TotalMilliseconds;

            var differenceDirect = (iocDirectTicks - iocComparisonTicks) / iocComparisonTicks * 100;
            _output.WriteLine("Difference in using the generic binded underlying IOC technolgy compared to the non-generic binding: {0}% (This will vary depending on how many only IOC resolves there are compared to some that require the generic infrastructure, especially methods).", differenceDirect);

            var differenceComparison = (iocTicks - iocComparisonTicks) / iocComparisonTicks * 100;
            _output.WriteLine("Difference in using the generic IOC calls compared to the non-generic calls: {0}% (This will normally be slower as there is infrastructure between the IOC resolution and the generic infrastructure, sometime just a property call adds a huge amount of overhead).", differenceComparison);
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
            var iocTestsDirect = RunIocTests(PerformanceResolveDirect<T>);

            if (Fixture.RunComparisonTests)
            {
                var iocTestsComparison = RunIocTests(PerformanceResolveComparison<T>);

                // Assert
                AssertPerformance(iocTests, iocTestsDirect, iocTestsComparison);
            }
            else
            {
                AssertPerformance(iocTests, iocTestsDirect, TimeSpan.MinValue);
            }
        }

        #endregion Methods
    }
}