using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace ZarDevs.DependencyInjection.Tests
{
    public abstract class IocTestsConstruct
    {
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
        }

        [Fact]
        public void Resolve_ToMethod_ReturnsInstances()
        {
            // Act
            ICallingClass callingClass = Ioc.Resolve<ICallingClass>();

            // Assert
            Assert.NotNull(callingClass);
            IChildClass childClass = callingClass.Call();
            Assert.NotNull(childClass);
            Assert.Equal(childClass.CallingClassType, callingClass.GetType());
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
        }

        #endregion Methods
    }
}
