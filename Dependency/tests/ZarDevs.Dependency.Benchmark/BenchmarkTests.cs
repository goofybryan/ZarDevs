using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Loggers;
using BenchmarkDotNet.Running;
using Xunit.Abstractions;

namespace ZarDevs.Dependency.Benchmark;

public class BenchmarkTests
{
    private readonly ITestOutputHelper _output;

    public BenchmarkTests(ITestOutputHelper output)
    {
        _output = output;
    }

    //[Fact]
    public void BenchmarkIoc()
    {
        var logger = new AccumulationLogger();

        var config = ManualConfig.Create(DefaultConfig.Instance)
            .AddLogger(logger);

        BenchmarkRunner.Run<PerformanceBenchmark>(new DebugInProcessConfig().AddLogger(logger));

        // write benchmark summary
        _output.WriteLine(logger.GetLog());
    }
}
