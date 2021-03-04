﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace ZarDevs.DependencyInjection.Tests
{
    public abstract class IocTestsConstruct<T> where T : class, IIocTestFixture
    {
        #region Constructors

        public IocTestsConstruct(T fixture)
        {
            Fixture = fixture ?? throw new ArgumentNullException(nameof(fixture));
        }

        #endregion Constructors

        #region Properties

        protected T Fixture { get; }
        protected IIocContainer Ioc => Fixture.Container;

        #endregion Properties

        #region Methods

        [Fact]
        public void Resolve_GenericImplementations_ReturnsInstance()
        {
            // Act
            IGenericTypeTests<int> intGeneric = Ioc.Resolve<IGenericTypeTests<int>>();
            IGenericTypeTests<bool> boolGeneric = Ioc.Resolve<IGenericTypeTests<bool>>();

            // Assert
            Assert.NotNull(intGeneric);
            Assert.NotNull(boolGeneric);
            Assert.NotEqual(intGeneric.GetType(), boolGeneric.GetType());
        }

        [Fact]
        public void Resolve_GenericSingltonImplementations_ReturnsSameInstanceForSameType()
        {
            // Act
            IGenericSingletonTests<int> intGeneric1 = Ioc.Resolve<IGenericSingletonTests<int>>();
            IGenericSingletonTests<int> intGeneric2 = Ioc.Resolve<IGenericSingletonTests<int>>();
            IGenericSingletonTests<bool> boolGeneric1 = Ioc.Resolve<IGenericSingletonTests<bool>>();
            IGenericSingletonTests<bool> boolGeneric2 = Ioc.Resolve<IGenericSingletonTests<bool>>();

            // Assert
            Assert.NotNull(intGeneric1);
            Assert.NotNull(boolGeneric1);
            Assert.NotNull(intGeneric2);
            Assert.NotNull(boolGeneric2);
            Assert.NotEqual(intGeneric1.GetType(), boolGeneric1.GetType());
            Assert.Same(intGeneric1, intGeneric2);
            Assert.Same(boolGeneric1, boolGeneric2);
        }

        [Fact]
        public void Resolve_Singleton_ReturnsSameInstance()
        {
            // Act
            ISingletonClass instance1 = Ioc.Resolve<ISingletonClass>();
            ISingletonClass instance2 = Ioc.Resolve<ISingletonClass>();

            // Assert
            AssertInstanceIsSame(instance1, instance2);
            Assert.IsType<SingletonClassTest>(instance1);
        }

        [Fact]
        public void Resolve_SingletonKeyTest_ReturnsSameInstanceForEachKey()
        {
            // Act
            ISingletonNamedClass instance1 = Ioc.ResolveNamed<ISingletonNamedClass>(nameof(ISingletonNamedClass));
            ISingletonNamedClass instance2 = Ioc.ResolveNamed<ISingletonNamedClass>(nameof(ISingletonNamedClass));
            ISingletonEnumClass instance3 = Ioc.ResolveWithKey<ISingletonEnumClass>(Bindings.EnumAsKey.Key);
            ISingletonEnumClass instance4 = Ioc.ResolveWithKey<ISingletonEnumClass>(Bindings.EnumAsKey.Key);
            ISingletonKeyClass instance5 = Ioc.ResolveWithKey<ISingletonKeyClass>(typeof(ISingletonKeyClass));
            ISingletonKeyClass instance6 = Ioc.ResolveWithKey<ISingletonKeyClass>(typeof(ISingletonKeyClass));

            // Assert
            AssertInstanceIsSame(instance1, instance2);
            AssertInstanceIsSame(instance3, instance4);
            AssertInstanceIsSame(instance5, instance6);
            AssertInstanceIsNotSame(instance1, instance3);
            AssertInstanceIsNotSame(instance1, instance5);
            AssertInstanceIsNotSame(instance3, instance5);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(1, "2")]
        [InlineData(1, "2", typeof(MultipleConstructorClass))]
        [InlineData(1, "2", 3)]
        public void Resolve_ToMethodWithNamedParameters_ReturnsInstance(params object[] values)
        {
            // Arrange
            var args = new (string, object)[values.Length];

            for (int i = 0; i < values.Length;)
            {
                object value = values[i];
                args[i] = ValueTuple.Create($"value{++i}", value);
            }

            // Act
            IFactoryResolutionClass constructorClass = Ioc.ResolveNamed<IFactoryResolutionClass>(Bindings.MethodWithArgs, args);

            // Assert
            Assert.NotNull(constructorClass);
            Assert.Equal(values.Length, constructorClass.Args.Count);
            for (int i = 0; i < values.Length;)
            {
                object expectedValue = values[i];
                object actualValue = constructorClass.Args[$"value{++i}"];

                Assert.Equal(expectedValue, actualValue);
            }
        }

        [Fact]
        public void Resolve_ToMethodWithNoParameters_ReturnsInstance()
        {
            // Act
            IFactoryResolutionClass constructorClass = Ioc.ResolveNamed<IFactoryResolutionClass>(Bindings.MethodWithNoArgs);

            // Assert
            Assert.NotNull(constructorClass);
            Assert.Equal(0, constructorClass.Args.Count);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(1, "2")]
        [InlineData(1, "2", typeof(MultipleConstructorClass))]
        [InlineData(1, "2", 3)]
        public void Resolve_ToMethodWithOrderedParameters_ReturnsInstance(params object[] values)
        {
            // Act
            IFactoryResolutionClass constructorClass = Ioc.ResolveNamed<IFactoryResolutionClass>(Bindings.MethodWithArgs, values);

            // Assert
            Assert.NotNull(constructorClass);
            Assert.Equal(values.Length, constructorClass.Args.Count);
            for (int i = 0; i < values.Length;)
            {
                object expectedValue = values[i];
                object actualValue = constructorClass.Args[$"value{++i}"];

                Assert.Equal(expectedValue, actualValue);
            }
        }

        [Fact]
        public void Resolve_Transient_ReturnsDifferentInstance()
        {
            // Act
            INormalClass instance1 = Ioc.Resolve<INormalClass>();
            INormalClass instance2 = Ioc.Resolve<INormalClass>();

            // Assert
            AssertInstanceIsNotSame(instance1, instance2);
            Assert.IsType<NormalClass>(instance1);
            Assert.IsType<NormalClass>(instance2);
        }

        [Fact]
        public void Resolve_WithConstructorArgsResolved_ReturnsInstances()
        {
            // Act
            ICallingClass callingClass = Ioc.Resolve<ICallingClass>();

            // Assert
            Assert.NotNull(callingClass);
            IChildClass childClass = callingClass.Call();
            Assert.NotNull(childClass);
            Assert.IsType<CallingClass>(callingClass);
            Assert.IsType<ChildClass>(childClass);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(1, "2")]
        [InlineData(1, "2", typeof(MultipleConstructorClass))]
        [InlineData(1, "2", 3)]
        public void Resolve_WithNamedParameters_ReturnsInstance(params object[] values)
        {
            // Arrange
            var args = new (string, object)[values.Length];

            for (int i = 0; i < values.Length;)
            {
                object value = values[i];
                args[i] = ValueTuple.Create($"value{++i}", value);
            }

            // Act
            IMultipleConstructorClass constructorClass = Ioc.Resolve<IMultipleConstructorClass>(args);

            // Assert
            Assert.NotNull(constructorClass);
            Assert.Equal(values.Length, constructorClass.Args.Count);
            for (int i = 0; i < values.Length;)
            {
                object expectedValue = values[i];
                object actualValue = constructorClass.Args[$"value{++i}"];

                Assert.Equal(expectedValue, actualValue);
            }
        }

        [Fact]
        public void Resolve_WithNoParameters_ReturnsInstance()
        {
            // Act
            IMultipleConstructorClass constructorClass = Ioc.Resolve<IMultipleConstructorClass>();

            // Assert
            Assert.NotNull(constructorClass);
            Assert.Equal(0, constructorClass.Args.Count);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(1, "2")]
        [InlineData(1, "2", typeof(MultipleConstructorClass))]
        [InlineData(1, "2", 3)]
        public void Resolve_WithOrderedParameters_ReturnsInstance(params object[] values)
        {
            // Act
            IMultipleConstructorClass constructorClass = Ioc.Resolve<IMultipleConstructorClass>(values);

            // Assert
            Assert.NotNull(constructorClass);
            Assert.Equal(values.Length, constructorClass.Args.Count);
            for (int i = 0; i < values.Length;)
            {
                object expectedValue = values[i];
                object actualValue = constructorClass.Args[$"value{++i}"];

                Assert.Equal(expectedValue, actualValue);
            }
        }

        [Fact]
        public void Resolve_MultipleBindings_ReturnsAll()
        {
            // Act
            IEnumerable<IMultipleBindingClassTest> all = Ioc.ResolveAll<IMultipleBindingClassTest>();

            // Assert
            Assert.NotNull(all);
            var listAll = all.ToList();
            Assert.Equal(3, listAll.Count);
            Assert.Contains(listAll, a => a is MultipleBindingClassTest1);
            Assert.Contains(listAll, a => a is MultipleBindingClassTest2);
            Assert.Contains(listAll, a => a is MultipleBindingClassTest3);
        }

        [Fact]
        public void Resolve_MultipleBindingsConstructor_ReturnsAll()
        {
            // Act
            IMultipleBindingConstructorClassTest test = Ioc.Resolve<IMultipleBindingConstructorClassTest>();

            // Assert
            Assert.NotNull(test);
            Assert.NotNull(test.MultipleBindings);
            var listAll = test.MultipleBindings.ToList();
            Assert.Equal(3, listAll.Count);
            Assert.Contains(listAll, a => a is MultipleBindingClassTest1);
            Assert.Contains(listAll, a => a is MultipleBindingClassTest2);
            Assert.Contains(listAll, a => a is MultipleBindingClassTest3);
        }

        [Fact]
        public void Resolve_MultipleBindingsGeneric_ReturnsAll()
        {
            // Act
            IEnumerable<IMultipleBindingClassTest<int>> allInt = Ioc.ResolveAll<IMultipleBindingClassTest<int>>();
            IEnumerable<IMultipleBindingClassTest<bool>> allBool = Ioc.ResolveAll<IMultipleBindingClassTest<bool>>();

            // Assert
            Assert.NotNull(allInt);
            var listAllInt = allInt.ToList();
            Assert.Equal(3, listAllInt.Count);
            Assert.All(listAllInt, Assert.NotNull);
            Assert.Contains(listAllInt, a => a is MultipleBindingClassTest1<int>);
            Assert.Contains(listAllInt, a => a is MultipleBindingClassTest2<int>);
            Assert.Contains(listAllInt, a => a is MultipleBindingClassTest3<int>);

            Assert.NotNull(allBool);
            var listAllBool = allBool.ToList();
            Assert.Equal(3, listAllBool.Count);
            Assert.Contains(listAllBool, a => a is MultipleBindingClassTest1<bool>);
            Assert.Contains(listAllBool, a => a is MultipleBindingClassTest2<bool>);
            Assert.Contains(listAllBool, a => a is MultipleBindingClassTest3<bool>);
        }

        [Fact]
        public void Resolve_MultipleBindingsGenericConstructor_ReturnsAll()
        {
            // Act
            IMultipleBindingConstructorClassTest<int> testInt = Ioc.Resolve<IMultipleBindingConstructorClassTest<int>>();
            IMultipleBindingConstructorClassTest<bool> testBool = Ioc.Resolve<IMultipleBindingConstructorClassTest<bool>>();

            // Assert
            Assert.NotNull(testInt);
            Assert.NotNull(testInt.MultipleBindings);
            var listAllInt = testInt.MultipleBindings.ToList();
            Assert.Equal(3, listAllInt.Count);
            Assert.All(listAllInt, Assert.NotNull);
            Assert.Contains(listAllInt, a => a is MultipleBindingClassTest1<int>);
            Assert.Contains(listAllInt, a => a is MultipleBindingClassTest2<int>);
            Assert.Contains(listAllInt, a => a is MultipleBindingClassTest3<int>);

            Assert.NotNull(testBool);
            Assert.NotNull(testBool.MultipleBindings);
            var listAllBool = testBool.MultipleBindings.ToList();
            Assert.Equal(3, listAllBool.Count);
            Assert.Contains(listAllBool, a => a is MultipleBindingClassTest1<bool>);
            Assert.Contains(listAllBool, a => a is MultipleBindingClassTest2<bool>);
            Assert.Contains(listAllBool, a => a is MultipleBindingClassTest3<bool>);
        }

        [Fact]
        public void TryResolve_NotBinded_ReturnsNull()
        {
            // Act
            var instance1 = Ioc.TryResolve<INotBindedClass>();
            var instance2 = Ioc.TryResolve<INotBindedClass>("value1");

            // Assert
            Assert.Null(instance1);
            Assert.Null(instance2);
        }

        [Fact]
        public void TryResolve_NotBindedEnum_ReturnsNull()
        {
            // Arrange
            var key = Bindings.EnumAsKey.DifferentKey;

            // Act
            var instance1 = Ioc.TryResolveWithKey<INotBindedKeyed>(key);
            var instance2 = Ioc.TryResolveWithKey<INotBindedKeyed>(key, "value1");

            // Assert
            Assert.Null(instance1);
            Assert.Null(instance2);
        }

        [Fact]
        public void TryResolve_NotBindedKeyed_ReturnsNull()
        {
            // Arrange
            var key = new object();

            // Act
            var instance1 = Ioc.TryResolveWithKey<INotBindedKeyed>(key);
            var instance2 = Ioc.TryResolveWithKey<INotBindedKeyed>(key, "value1");

            // Assert
            Assert.Null(instance1);
            Assert.Null(instance2);
        }

        [Fact]
        public void TryResolve_NotBindedNamed_ReturnsNull()
        {
            // Arrange
            var key = "Some Name";

            // Act
            var instance1 = Ioc.TryResolveNamed<INotBindedKeyed>(key);
            var instance2 = Ioc.TryResolveNamed<INotBindedKeyed>(key, "value1");

            // Assert
            Assert.Null(instance1);
            Assert.Null(instance2);
        }

        [Fact]
        public void TryResolve_Singleton_ReturnsSameInstance()
        {
            // Act
            ISingletonClass instance1 = Ioc.TryResolve<ISingletonClass>();
            ISingletonClass instance2 = Ioc.TryResolve<ISingletonClass>();

            // Assert
            AssertInstanceIsSame(instance1, instance2);
            Assert.IsType<SingletonClassTest>(instance1);
        }

        [Fact]
        public void TryResolve_SingletonKeyTest_ReturnsSameInstanceForEachKey()
        {
            // Act
            ISingletonNamedClass instance1 = Ioc.TryResolveNamed<ISingletonNamedClass>(nameof(ISingletonNamedClass));
            ISingletonNamedClass instance2 = Ioc.TryResolveNamed<ISingletonNamedClass>(nameof(ISingletonNamedClass));
            ISingletonEnumClass instance3 = Ioc.TryResolveWithKey<ISingletonEnumClass>(Bindings.EnumAsKey.Key);
            ISingletonEnumClass instance4 = Ioc.TryResolveWithKey<ISingletonEnumClass>(Bindings.EnumAsKey.Key);
            ISingletonKeyClass instance5 = Ioc.TryResolveWithKey<ISingletonKeyClass>(typeof(ISingletonKeyClass));
            ISingletonKeyClass instance6 = Ioc.TryResolveWithKey<ISingletonKeyClass>(typeof(ISingletonKeyClass));

            // Assert
            AssertInstanceIsSame(instance1, instance2);
            AssertInstanceIsSame(instance3, instance4);
            AssertInstanceIsSame(instance5, instance6);
            AssertInstanceIsNotSame(instance1, instance3);
            AssertInstanceIsNotSame(instance1, instance5);
            AssertInstanceIsNotSame(instance3, instance5);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(1, "2")]
        [InlineData(1, "2", typeof(MultipleConstructorClass))]
        [InlineData(1, "2", 3)]
        public void TryResolve_ToMethodWithNamedParameters_ReturnsInstance(params object[] values)
        {
            // Arrange
            var args = new (string, object)[values.Length];

            for (int i = 0; i < values.Length;)
            {
                object value = values[i];
                args[i] = ValueTuple.Create($"value{++i}", value);
            }

            // Act
            IFactoryResolutionClass constructorClass = Ioc.TryResolveNamed<IFactoryResolutionClass>(Bindings.MethodWithArgs, args);

            // Assert
            Assert.NotNull(constructorClass);
            Assert.Equal(values.Length, constructorClass.Args.Count);
            for (int i = 0; i < values.Length;)
            {
                object expectedValue = values[i];
                object actualValue = constructorClass.Args[$"value{++i}"];

                Assert.Equal(expectedValue, actualValue);
            }
        }

        [Fact]
        public void TryResolve_ToMethodWithNoParameters_ReturnsInstance()
        {
            // Act
            IFactoryResolutionClass constructorClass = Ioc.TryResolveNamed<IFactoryResolutionClass>(Bindings.MethodWithNoArgs);

            // Assert
            Assert.NotNull(constructorClass);
            Assert.Equal(0, constructorClass.Args.Count);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(1, "2")]
        [InlineData(1, "2", typeof(MultipleConstructorClass))]
        [InlineData(1, "2", 3)]
        public void TryResolve_ToMethodWithOrderedParameters_ReturnsInstance(params object[] values)
        {
            // Act
            IFactoryResolutionClass constructorClass = Ioc.TryResolveNamed<IFactoryResolutionClass>(Bindings.MethodWithArgs, values);

            // Assert
            Assert.NotNull(constructorClass);
            Assert.Equal(values.Length, constructorClass.Args.Count);
            for (int i = 0; i < values.Length;)
            {
                object expectedValue = values[i];
                object actualValue = constructorClass.Args[$"value{++i}"];

                Assert.Equal(expectedValue, actualValue);
            }
        }

        [Fact]
        public void TryResolve_Transient_ReturnsDifferentInstance()
        {
            // Act
            INormalClass instance1 = Ioc.TryResolve<INormalClass>();
            INormalClass instance2 = Ioc.TryResolve<INormalClass>();

            // Assert
            AssertInstanceIsNotSame(instance1, instance2);
            Assert.IsType<NormalClass>(instance1);
            Assert.IsType<NormalClass>(instance2);
        }

        [Fact]
        public void TryResolve_WithConstructorArgsTryResolved_ReturnsInstances()
        {
            // Act
            ICallingClass callingClass = Ioc.TryResolve<ICallingClass>();

            // Assert
            Assert.NotNull(callingClass);
            IChildClass childClass = callingClass.Call();
            Assert.NotNull(childClass);
            Assert.IsType<CallingClass>(callingClass);
            Assert.IsType<ChildClass>(childClass);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(1, "2")]
        [InlineData(1, "2", typeof(MultipleConstructorClass))]
        [InlineData(1, "2", 3)]
        public void TryResolve_WithNamedParameters_ReturnsInstance(params object[] values)
        {
            // Arrange
            var args = new (string, object)[values.Length];

            for (int i = 0; i < values.Length;)
            {
                object value = values[i];
                args[i] = ValueTuple.Create($"value{++i}", value);
            }

            // Act
            IMultipleConstructorClass constructorClass = Ioc.TryResolve<IMultipleConstructorClass>(args);

            // Assert
            Assert.NotNull(constructorClass);
            Assert.Equal(values.Length, constructorClass.Args.Count);
            for (int i = 0; i < values.Length;)
            {
                object expectedValue = values[i];
                object actualValue = constructorClass.Args[$"value{++i}"];

                Assert.Equal(expectedValue, actualValue);
            }
        }

        [Fact]
        public void TryResolve_WithNoParameters_ReturnsInstance()
        {
            // Act
            IMultipleConstructorClass constructorClass = Ioc.TryResolve<IMultipleConstructorClass>();

            // Assert
            Assert.NotNull(constructorClass);
            Assert.Equal(0, constructorClass.Args.Count);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(1, "2")]
        [InlineData(1, "2", typeof(MultipleConstructorClass))]
        [InlineData(1, "2", 3)]
        public void TryResolve_WithOrderedParameters_ReturnsInstance(params object[] values)
        {
            // Act
            IMultipleConstructorClass constructorClass = Ioc.TryResolve<IMultipleConstructorClass>(values);

            // Assert
            Assert.NotNull(constructorClass);
            Assert.Equal(values.Length, constructorClass.Args.Count);
            for (int i = 0; i < values.Length;)
            {
                object expectedValue = values[i];
                object actualValue = constructorClass.Args[$"value{++i}"];

                Assert.Equal(expectedValue, actualValue);
            }
        }

        private static void AssertInstanceIsNotSame(object instance1, object instance2)
        {
            Assert.NotNull(instance1);
            Assert.NotNull(instance2);
            Assert.NotSame(instance1, instance2);
        }

        private static void AssertInstanceIsSame(object instance1, object instance2)
        {
            Assert.NotNull(instance1);
            Assert.NotNull(instance2);
            Assert.Same(instance1, instance2);
        }

        #endregion Methods
    }
}