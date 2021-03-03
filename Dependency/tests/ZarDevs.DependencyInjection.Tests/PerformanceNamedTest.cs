namespace ZarDevs.DependencyInjection.Tests
{
    public interface IPerformanceNamedTest
    {

    }

    public class PerformanceNamedTest : IPerformanceNamedTest
    {
        public const string Name = nameof(PerformanceNamedTest);
        public enum PerformanceNamedTestEnum { EnumAsKey }
        public static object Key { get; } = typeof(PerformanceNamedTest);
    }
}