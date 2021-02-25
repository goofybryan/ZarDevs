using System;
using System.Collections.Generic;
using System.Text;

namespace ZarDevs.DependencyInjection.Tests
{
    public interface IMultipleConstructorClass
    {
        IDictionary<string,object> Args { get; }
    }

    public class MultipleConstructorClass : IMultipleConstructorClass
    {
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

        public IDictionary<string, object> Args { get; }
    }
}
