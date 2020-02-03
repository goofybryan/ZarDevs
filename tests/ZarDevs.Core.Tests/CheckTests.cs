using System;
using System.Collections.Generic;
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
            Assert.Same(value, returnValue);
        }

        [Fact]
        public void TestThatIsNotNullOrEmptyWithNullStringThrows()
        {
            //Assert
            const string param = "param";

            // Act
            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(() => Check.IsNotNullOrEmpty(null, param));

            // Assert
            Assert.Equal(param, exception.ParamName);
        }

        [Fact]
        public void TestThatIsNotNullOrEmptyWithEmptyStringThrows()
        {
            //Assert
            const string param = "param";

            // Act
            ArgumentException exception = Assert.Throws<ArgumentException>(() => Check.IsNotNullOrEmpty(string.Empty, param));

            // Assert
            Assert.Equal(param, exception.ParamName);
        }

        [Fact]
        public void TestThatIsNotNullOrEmptyWithStringDoesNotThrow()
        {
            // Act
            string returnValue = Check.IsNotNullOrEmpty("value", "param");

            // Assert
            Assert.Equal("value", returnValue);
        }

        [Fact]
        public void TestThatIsNotNullOrEmptyWithNullIEnumerableThrows()
        {
            //Assert
            const string param = "param";

            // Act
            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(() => Check.IsNotNullOrEmpty<object>(null, param));

            // Assert
            Assert.Equal(param, exception.ParamName);
        }

        [Fact]
        public void TestThatIsNotNullOrEmptyWithEmptyIEnumerableThrows()
        {
            //Assert
            const string param = "param";
            IEnumerable<object> enumerable = new object[0];

            // Act
            ArgumentException exception = Assert.Throws<ArgumentException>(() => Check.IsNotNullOrEmpty(enumerable, param));

            // Assert
            Assert.Equal(param, exception.ParamName);
        }

        [Fact]
        public void TestThatIsNotNullOrEmptyWithIEnumerableDoesNotThrow()
        {
            // Arrange
            IEnumerable<object> enumerable = new []{ new object() };

            // Act
            IEnumerable<object> returnValue = Check.IsNotNullOrEmpty(enumerable, "param");

            // Assert
            Assert.Same(enumerable, returnValue);
        }

        #endregion Methods
    }
}