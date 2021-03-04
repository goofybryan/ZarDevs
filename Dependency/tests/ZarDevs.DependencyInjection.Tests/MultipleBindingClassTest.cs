using System;
using System.Collections.Generic;
using System.Text;

namespace ZarDevs.DependencyInjection.Tests
{
    public interface IMultipleBindingClassTest
    {
    }

    public interface IMultipleBindingClassTest<T>
    {
    }

    public class MultipleBindingClassTest1 : IMultipleBindingClassTest
    {
    }

    public class MultipleBindingClassTest2 : IMultipleBindingClassTest
    {
    }

    public class MultipleBindingClassTest3 : IMultipleBindingClassTest
    {
    }

    public class MultipleBindingClassTest1<T> : IMultipleBindingClassTest<T>
    {
    }

    public class MultipleBindingClassTest2<T> : IMultipleBindingClassTest<T>
    {
    }

    public class MultipleBindingClassTest3<T> : IMultipleBindingClassTest<T>
    {
    }
}
