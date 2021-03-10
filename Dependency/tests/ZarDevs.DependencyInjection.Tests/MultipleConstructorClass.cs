using System.Collections.Generic;

namespace ZarDevs.DependencyInjection.Tests
{
    public interface IMultipleConstructorClass
    {
        #region Properties

        IDictionary<string, object> Args { get; }

        #endregion Properties
    }

    public class MultipleConstructorClass : IMultipleConstructorClass
    {
        #region Constructors

        public MultipleConstructorClass()
        {
            Args = new Dictionary<string, object>();
        }

        public MultipleConstructorClass(int value1) : this()
        {
            Args[nameof(value1)] = value1;
        }

        public MultipleConstructorClass(int value1, string value2) : this(value1)
        {
            Args[nameof(value2)] = value2;
        }

        public MultipleConstructorClass(int value1, string value2, object value3) : this(value1, value2)
        {
            Args[nameof(value3)] = value3;
        }

        #endregion Constructors

        #region Properties

        public IDictionary<string, object> Args { get; }

        #endregion Properties
    }
}