using System.Collections.Generic;

namespace ZarDevs.DependencyInjection.Tests
{
    public interface IMultipleBindingConstructorClassTest
    {
        #region Properties

        IEnumerable<IMultipleBindingClassTest> MultipleBindings { get; }

        #endregion Properties
    }

    public interface IMultipleBindingConstructorClassTest<T>
    {
        #region Properties

        IEnumerable<IMultipleBindingClassTest<T>> MultipleBindings { get; }

        #endregion Properties
    }

    public class MultipleBindingConstructorClassTest : IMultipleBindingConstructorClassTest
    {
        #region Constructors

        public MultipleBindingConstructorClassTest(IEnumerable<IMultipleBindingClassTest> multipleBindings) => MultipleBindings = multipleBindings;

        #endregion Constructors

        #region Properties

        public IEnumerable<IMultipleBindingClassTest> MultipleBindings { get; }

        #endregion Properties
    }

    public class MultipleBindingConstructorClassTest<T> : IMultipleBindingConstructorClassTest<T>
    {
        #region Constructors

        public MultipleBindingConstructorClassTest(IEnumerable<IMultipleBindingClassTest<T>> multipleBindings)
        {
            MultipleBindings = multipleBindings;
        }

        #endregion Constructors

        #region Properties

        public IEnumerable<IMultipleBindingClassTest<T>> MultipleBindings { get; }

        #endregion Properties
    }
}