using System;
using Xunit;

namespace ZarDevs.Core.Tests
{
    public class CheckTests
    {
        #region Methods

        [Fact]
        public void TestThatIsNotNullAndThrowWithNullValueThrows()
        {
            //Assert
            const string message = "Should throw and display this message";

            // Act
            NotSupportedException exception = Assert.Throws<NotSupportedException>(() => Check.IsNotNullAndThrow<NotSupportedException>(null, message));

            // Assert
            Assert.Equal(message, exception.Message);
        }

        [Fact]
        public void TestThatIsNotNullAndThrowWithValueDoesNotThrows()
        {
            //Assert
            const string message = "Should not throw and not display this message";
            object value = new object();

            // Act
            Check.IsNotNullAndThrow<NotSupportedException>(value, message);
        }

        [Fact]
        public void TestThatIsNotNullWithNullValueThrows()
        {
            //Assert
            const string param = "param";

            // Act
            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(() => Check.IsNotNull<object>(null, param));

            // Assert
            Assert.Equal(param, exception.ParamName);
        }

        [Fact]
        public void TestThatIsNotNullWithValueDoesNotThrow()
        {
            // Arrange
            object value = new object();

            // Act
            object returnValue = Check.IsNotNull(value, "param");

            // Assert
            Assert.Same(value, value);
        }

        #endregion Methods
    }
}