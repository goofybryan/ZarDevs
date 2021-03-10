namespace ZarDevs.DependencyInjection.Tests
{
    public interface IPerformanceMethodResultTest
    {
    }

    public interface IPerformanceMethodTest
    {
    }

    public class PerformanceMethodTest : IPerformanceMethodTest, IPerformanceMethodResultTest
    {
        #region Methods

        public static IPerformanceMethodResultTest Method()
        {
            return new PerformanceMethodTest();
        }

        #endregion Methods
    }
}