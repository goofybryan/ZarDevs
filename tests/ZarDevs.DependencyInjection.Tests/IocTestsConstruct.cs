using System;
using Xunit;

namespace ZarDevs.DependencyInjection.Tests
{
    public interface IIocTests : IDisposable
    {
        #region Properties

        IIocContainer Container { get; }

        #endregion Properties
    }

    public abstract class IocTestsConstruct<T> where T : class, IIocTests
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
        public void Resolve_Singleton_ReturnsSameInstance()
        {
            // Act
            ISingletonClass instance1 = Ioc.Resolve<ISingletonClass>();
            ISingletonClass instance2 = Ioc.Resolve<ISingletonClass>();

            // Assert
            Assert.NotNull(instance1);
            Assert.NotNull(instance2);
            Assert.Same(instance1, instance2);
            Assert.IsType<SingletonClass>(instance1);
        }

        [Fact]
        public void Resolve_ToMethodWithNoParameters_ReturnsInstance()
        {
            // Act
            IMultipleConstructorClass constructorClass = Ioc.ResolveNamed<IMultipleConstructorClass>(Bindings.MethodWithNoArgs);

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
            IMultipleConstructorClass constructorClass = Ioc.ResolveNamed<IMultipleConstructorClass>(Bindings.MethodWithArgs, values);

            // Assert
            Assert.NotNull(constructorClass);
            Assert.Equal(values.Length, constructorClass.Args.Count);
            for (int i = 0; i < values.Length; )
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
            Assert.NotNull(instance1);
            Assert.NotNull(instance2);
            Assert.NotSame(instance1, instance2);
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

        [Fact]
        public void Resolve_WithNoParameters_ReturnsInstance()
        {
            // Act
            IMultipleConstructorClass constructorClass = Ioc.ResolveNamed<IMultipleConstructorClass>(Bindings.MethodWithNoArgs);

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
            IMultipleConstructorClass constructorClass = Ioc.ResolveNamed<IMultipleConstructorClass>(Bindings.MethodWithArgs, values);

            // Assert
            Assert.NotNull(constructorClass);
            Assert.Equal(values.Length, constructorClass.Args.Count);
            for (int i = 0; i < values.Length; )
            {
                object expectedValue = values[i];
                object actualValue = constructorClass.Args[$"value{++i}"];

                Assert.Equal(expectedValue, actualValue);
            }
        }

        #endregion Methods
    }
}