namespace ZarDevs.DependencyInjection.Tests
{
    public interface IPerformanceConstructParam1Test
    { }

    public interface IPerformanceConstructParam2Test
    { }

    public interface IPerformanceConstructParam3Test
    { }

    public interface IPerformanceConstructTest
    { }

    public class PerformanceConstructParamTest : IPerformanceConstructParam1Test, IPerformanceConstructParam2Test, IPerformanceConstructParam3Test
    {
        #region Constructors

        public PerformanceConstructParamTest()
        {
        }

        #endregion Constructors
    }

    public class PerformanceConstructTest : IPerformanceConstructTest
    {
        #region Constructors

        public PerformanceConstructTest(IPerformanceConstructParam1Test param1, IPerformanceConstructParam2Test param2, IPerformanceConstructParam3Test param3)
        {
            Param1 = param1 ?? throw new System.ArgumentNullException(nameof(param1));
            Param2 = param2 ?? throw new System.ArgumentNullException(nameof(param2));
            Param3 = param3 ?? throw new System.ArgumentNullException(nameof(param3));
        }

        #endregion Constructors

        #region Properties

        public IPerformanceConstructParam1Test Param1 { get; }
        public IPerformanceConstructParam2Test Param2 { get; }
        public IPerformanceConstructParam3Test Param3 { get; }

        #endregion Properties
    }
}