namespace ZarDevs.DependencyInjection.Tests
{
    public interface IFactoryMethodClass
    {
        #region Methods

        IFactoryMethodResolutionClass Method(INormalClass normalClass, params IMultipleBindingClassTest[] multipleBindings);

        IFactoryMethodResolutionSingletonClass Singleton(INormalClass normalClass, params IMultipleBindingClassTest[] multipleBindings);

        #endregion Methods
    }

    public interface IFactoryMethodClass<T>
    {
        #region Methods

        IFactoryMethodResolutionClass<T> Method(IGenericTypeTests<T> genericType, params IMultipleBindingClassTest<T>[] multipleBindings);

        IFactoryMethodResolutionSingletonClass<T> Singleton(IGenericTypeTests<T> genericType, params IMultipleBindingClassTest<T>[] multipleBindings);

        #endregion Methods
    }

    public interface IFactoryMethodResolutionClass
    {
        #region Properties

        IMultipleBindingClassTest[] MultipleBindings { get; }
        INormalClass NormalClass { get; }

        #endregion Properties
    }

    public interface IFactoryMethodResolutionClass<T>
    {
        #region Properties

        IGenericTypeTests<T> GenericType { get; }
        IMultipleBindingClassTest<T>[] MultipleBindings { get; }

        #endregion Properties
    }

    public interface IFactoryMethodResolutionNamedClass : IFactoryMethodResolutionClass { }

    public interface IFactoryMethodResolutionSingletonClass : IFactoryMethodResolutionClass { }

    public interface IFactoryMethodResolutionSingletonClass<T> : IFactoryMethodResolutionClass<T> { }

    public class FactoryMethodClass : IFactoryMethodClass
    {
        #region Methods

        public IFactoryMethodResolutionClass Method(INormalClass normalClass, params IMultipleBindingClassTest[] multipleBindings)
        {
            return new FactoryMethodResolutionClass(normalClass, multipleBindings);
        }

        public IFactoryMethodResolutionSingletonClass Singleton(INormalClass normalClass, params IMultipleBindingClassTest[] multipleBindings)
        {
            return new FactoryMethodResolutionClass(normalClass, multipleBindings);
        }

        #endregion Methods
    }

    public class FactoryMethodClass<T> : IFactoryMethodClass<T>
    {
        #region Methods

        public IFactoryMethodResolutionClass<T> Method(IGenericTypeTests<T> genericType, params IMultipleBindingClassTest<T>[] multipleBindings)
        {
            return new FactoryMethodResolutionClass<T>(genericType, multipleBindings);
        }

        public IFactoryMethodResolutionSingletonClass<T> Singleton(IGenericTypeTests<T> genericType, params IMultipleBindingClassTest<T>[] multipleBindings)
        {
            return new FactoryMethodResolutionClass<T>(genericType, multipleBindings);
        }

        #endregion Methods
    }

    public class FactoryMethodResolutionClass : IFactoryMethodResolutionClass, IFactoryMethodResolutionSingletonClass, IFactoryMethodResolutionNamedClass
    {
        #region Constructors

        public FactoryMethodResolutionClass(INormalClass normalClass, params IMultipleBindingClassTest[] multipleBindings)
        {
            NormalClass = normalClass;
            MultipleBindings = multipleBindings;
        }

        #endregion Constructors

        #region Properties

        public IMultipleBindingClassTest[] MultipleBindings { get; }
        public INormalClass NormalClass { get; }

        #endregion Properties
    }

    public class FactoryMethodResolutionClass<T> : IFactoryMethodResolutionClass<T>, IFactoryMethodResolutionSingletonClass<T>
    {
        #region Constructors

        public FactoryMethodResolutionClass(IGenericTypeTests<T> genericType, params IMultipleBindingClassTest<T>[] multipleBindings)
        {
            GenericType = genericType;
            MultipleBindings = multipleBindings;
        }

        #endregion Constructors

        #region Properties

        public IGenericTypeTests<T> GenericType { get; }
        public IMultipleBindingClassTest<T>[] MultipleBindings { get; }

        #endregion Properties
    }
}