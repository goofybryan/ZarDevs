namespace ZarDevs.DependencyInjection.Tests
{
    public interface IGenericSingletonTests<T>
    {
        #region Methods

        T GetValue();

        #endregion Methods
    }

    public interface IGenericTypeTests<T>
    {
        #region Properties

        T Value { get; set; }

        #endregion Properties
    }

    public class GenericTypeTest<T> : IGenericTypeTests<T>, IGenericSingletonTests<T>
    {
        #region Properties

        public T Value { get; set; }

        #endregion Properties

        #region Methods

        public T GetValue()
        {
            return Value;
        }

        #endregion Methods
    }
}