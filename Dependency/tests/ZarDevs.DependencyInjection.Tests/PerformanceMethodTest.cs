namespace ZarDevs.DependencyInjection.Tests
{
    public interface IPerformanceMethodTest
    {
    }

    public interface IPerformanceMethodResultTest
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