using System.Collections.Generic;

namespace ZarDevs.DependencyInjection.Tests
{
    public interface IMultipleBindingConstructorClassTest
    {
        IEnumerable<IMultipleBindingClassTest> MultipleBindings { get; }
    }

    public interface IMultipleBindingConstructorClassTest<T>
    { 
        IEnumerable<IMultipleBindingClassTest<T>> MultipleBindings { get; }
    }

    public class MultipleBindingConstructorClassTest : IMultipleBindingConstructorClassTest
    {
        public MultipleBindingConstructorClassTest(IEnumerable<IMultipleBindingClassTest> multipleBindings) => MultipleBindings = multipleBindings;

        public IEnumerable<IMultipleBindingClassTest> MultipleBindings { get; }
    }

    public class MultipleBindingConstructorClassTest<T> : IMultipleBindingConstructorClassTest<T>
    {
        public MultipleBindingConstructorClassTest(IEnumerable<IMultipleBindingClassTest<T>> multipleBindings)
        {
            MultipleBindings = multipleBindings;
        }

        public IEnumerable<IMultipleBindingClassTest<T>> MultipleBindings { get; }
    }
}
