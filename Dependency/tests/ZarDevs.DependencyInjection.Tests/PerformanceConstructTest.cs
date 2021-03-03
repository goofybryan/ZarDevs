namespace ZarDevs.DependencyInjection.Tests
{
    public interface IPerformanceConstructTest
    { }
    public interface IPerformanceConstructParam1Test
    { }
    public interface IPerformanceConstructParam2Test
    { }
    public interface IPerformanceConstructParam3Test
    { }

    public class PerformanceConstructTest : IPerformanceConstructTest
    {
        public PerformanceConstructTest(IPerformanceConstructParam1Test param1, IPerformanceConstructParam2Test param2, IPerformanceConstructParam3Test param3)
        {
            Param1 = param1 ?? throw new System.ArgumentNullException(nameof(param1));
            Param2 = param2 ?? throw new System.ArgumentNullException(nameof(param2));
            Param3 = param3 ?? throw new System.ArgumentNullException(nameof(param3));
        }

        public IPerformanceConstructParam1Test Param1 { get; }
        public IPerformanceConstructParam2Test Param2 { get; }
        public IPerformanceConstructParam3Test Param3 { get; }
    }

    public class PerformanceConstructParamTest : IPerformanceConstructParam1Test, IPerformanceConstructParam2Test, IPerformanceConstructParam3Test
    {
        public PerformanceConstructParamTest()
        {
        }
    }
}
