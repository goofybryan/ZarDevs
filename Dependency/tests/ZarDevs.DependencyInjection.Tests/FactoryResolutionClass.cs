namespace ZarDevs.DependencyInjection.Tests
{
    public interface IFactoryResolutionClass : IMultipleConstructorClass
    { }

    public class FactoryResolutionClass : MultipleConstructorClass, IFactoryResolutionClass
    {
        #region Constructors

        public FactoryResolutionClass() : base()
        {
        }

        public FactoryResolutionClass(int value1) : base(value1)
        {
        }

        public FactoryResolutionClass(int value1, string value2) : base(value1, value2)
        {
        }

        public FactoryResolutionClass(int value1, string value2, object value3) : base(value1, value2, value3)
        {
        }

        #endregion Constructors
    }
}