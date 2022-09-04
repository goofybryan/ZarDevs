using System;
using System.Collections.Generic;
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
            _output.WriteLine("Time taken for IOC {0} ms", iocTests.TotalTime.TotalMilliseconds);
            _output.WriteLine("Time taken for IOC with generic infrastructure {0} ms", iocTestsDirect.TotalTime.TotalMilliseconds);

            if (iocTestsComparison != null)
            {
                _output.WriteLine("Time taken for IOC with non-generic infrasturcture {0} ms", iocTestsComparison.TotalTime.TotalMilliseconds);
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

            StringBuilder builder = new StringBuilder()
                .AppendLine() 
                .AppendLine("       \tIOC\tDirect\tComparison")
                .Append("Fastest\t").Append(fastestIoc.Ticks).Append('\t').Append(fastestDirectIoc.Ticks).Append('\t').AppendLine(fastestComparisonIoc?.Ticks.ToString() ?? "N/A")
                .Append("Slowest\t").Append(slowestIoc.Ticks).Append('\t').Append(slowestDirectIoc.Ticks).Append('\t').AppendLine(slowestComparisonIoc?.Ticks.ToString() ?? "N/A")
                .Append("Average\t").Append((long)averageIoc).Append('\t').Append((long)averageDirectIoc).Append('\t').AppendLine(averageComparisonIoc != null ? ((long)averageComparisonIoc).ToString() : "N/A");

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