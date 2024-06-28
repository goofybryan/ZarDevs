using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Xunit;
using Xunit.Abstractions;

namespace ZarDevs.DependencyInjection.Tests
{
    [CollectionDefinition("Performance", DisableParallelization = true)]
    public abstract class IocPerformanceConstruct<TFixture> where TFixture : class, IIocPerformanceTestFixture
    {
        #region Fields

        private const int _totalRuns = 100000;
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
        public void PerformanceComparisonInstanceTest()
        {
            RunPerformanceTest<IPerformanceInstanceTest>();
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

        protected abstract T PerformanceResolveComparison<T>() where T : class;

        protected abstract T PerformanceResolveDirect<T>() where T : class;

        private void AssertPerformance(PerformenceMeasurement iocTests, PerformenceMeasurement iocTestsDirect, PerformenceMeasurement? iocTestsComparison)
        {
            _output.WriteLine("Total objects created per run {0}", _totalRuns);
            _output.WriteLine("Time taken for IOC {0} ns", iocTests.TotalTime.TotalNanoseconds);
            _output.WriteLine("Time taken for IOC with generic infrastructure {0} ns", iocTestsDirect.TotalTime.TotalNanoseconds);

            if (iocTestsComparison != null)
            {
                _output.WriteLine("Time taken for IOC with non-generic infrasturcture {0} ns", iocTestsComparison.TotalTime.TotalNanoseconds);
            }

            TimeSpan fastestIoc = iocTests.SubsequentCalls.OrderBy(c => c.Ticks).First();
            double averageIoc = iocTests.SubsequentCalls.Average(c => c.Ticks);
            TimeSpan slowestIoc = iocTests.SubsequentCalls.OrderByDescending(c => c.Ticks).First();
            TimeSpan fastestDirectIoc = iocTestsDirect.SubsequentCalls.OrderBy(c => c.Ticks).First();
            double averageDirectIoc = iocTests.SubsequentCalls.Average(c => c.Ticks);
            TimeSpan slowestDirectIoc = iocTestsDirect.SubsequentCalls.OrderByDescending(c => c.Ticks).First();
            TimeSpan? fastestComparisonIoc = iocTestsComparison?.SubsequentCalls.OrderBy(c => c.Ticks).First();
            double? averageComparisonIoc = iocTests.SubsequentCalls.Average(c => c.Ticks);
            TimeSpan? slowestComparisonIoc = iocTestsComparison?.SubsequentCalls.OrderByDescending(c => c.Ticks).First();

            const string format = "{0, -10} | {1, 7} | {2, 7} | {3, 7}";

            StringBuilder builder = new StringBuilder()
                .AppendLine()
                .AppendFormat(format, " ", "IOC", "Direct", "Comp").AppendLine()
                .AppendFormat(format, "Fastest", fastestIoc.TotalNanoseconds, fastestDirectIoc.TotalNanoseconds, fastestComparisonIoc?.TotalNanoseconds.ToString() ?? "N/A").AppendLine()
                .AppendFormat(format, "Slowest", slowestIoc.TotalNanoseconds, slowestDirectIoc.TotalNanoseconds, slowestComparisonIoc?.TotalNanoseconds.ToString() ?? "N/A").AppendLine()
                .AppendFormat(format, "Average", TimeSpan.FromTicks((long)averageIoc).TotalNanoseconds, TimeSpan.FromTicks((long)averageDirectIoc).TotalNanoseconds, averageComparisonIoc != null ? TimeSpan.FromTicks((long)averageComparisonIoc).TotalNanoseconds.ToString() : "N/A").AppendLine();

            _output.WriteLine(builder.ToString());
        }

        private T PerformanceResolve<T>() where T : class => Ioc.Resolve<T>();

        private static PerformenceMeasurement RunIocTests<T>(Func<T> creation) where T : class
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();

            PerformenceMeasurement measurement = new()
            {
                // Run once to ensure any lazy initialization is completed.
                FirstCall = Measure(creation)
            };


            for (int i = 0; i < _totalRuns; i++)
            {
                TimeSpan timeTaken = Measure(creation);
                measurement.SubsequentCalls.Add(timeTaken);
            }

            watch.Stop();

            measurement.TotalTime = watch.Elapsed;

            return measurement;
        }

        private static TimeSpan Measure<T>(Func<T> creation)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            T result = creation();
            watch.Stop();

            Assert.NotNull(result);
            Assert.IsAssignableFrom<T>(result);

            return watch.Elapsed;
        }

        private void RunPerformanceTest<T>() where T : class
        {
            // Act
            var iocTests = IocPerformanceConstruct<TFixture>.RunIocTests(PerformanceResolve<T>);
            var iocTestsDirect = IocPerformanceConstruct<TFixture>.RunIocTests(PerformanceResolveDirect<T>);
            var iocTestsComparison = Fixture.RunComparisonTests ? IocPerformanceConstruct<TFixture>.RunIocTests(PerformanceResolveComparison<T>) : null;

            AssertPerformance(iocTests, iocTestsDirect, iocTestsComparison);
        }

        #endregion Methods
    }

    internal class PerformenceMeasurement
    {
        public TimeSpan FirstCall { get; set; }
        public List<TimeSpan> SubsequentCalls { get; } = new();
        public TimeSpan TotalTime { get; set; }
    }
}