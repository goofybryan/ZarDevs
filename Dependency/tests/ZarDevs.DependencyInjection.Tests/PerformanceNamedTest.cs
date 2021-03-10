namespace ZarDevs.DependencyInjection.Tests
{
    public interface IPerformanceNamedTest
    {
    }

    public class PerformanceNamedTest : IPerformanceNamedTest
    {
        #region Fields

        public const string Name = nameof(PerformanceNamedTest);

        #endregion Fields

        #region Properties

        public static object Key { get; } = typeof(PerformanceNamedTest);

        #endregion Properties

        #region Enums

        public enum PerformanceNamedTestEnum { EnumAsKey }

        #endregion Enums
    }
}