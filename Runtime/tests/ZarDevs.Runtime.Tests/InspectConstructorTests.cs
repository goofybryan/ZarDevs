using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace ZarDevs.Runtime.Tests
{
    public class InspectConstructorTests
    {
        #region Methods

        [Theory]
        [InlineData(false, false, false, false)]
        [InlineData(true, true, true, true)]
        [InlineData(false, false, true, false)]
        [InlineData(true, true, false, false)]
        [InlineData(true, false, true, true)]
        public void FindParameterNames_WithListOfOrderedParamenters_ReturnsNamedMap(bool hasInteger, bool hasValue, bool hasGeneric, bool hasInterface)
        {
            // Arrange
            const string value = "Value";
            IList<object> parameters = [];
            int intValue = new Random().Next(1, 100);
            ITestInterface testInterface = new TestClass();

            if (hasInteger) parameters.Add(intValue);
            if (hasValue) parameters.Add(value);
            if (hasGeneric) parameters.Add(this);
            if (hasInterface) parameters.Add(testInterface);

            // Act
            IList<(string, object)> namedParameters = InspectConstructor.Instance.FindParameterNames(typeof(TestClass), [.. parameters]);

            // Assert
            int paramCount = 0;
            if (hasInteger) paramCount += AssertParam("integer", intValue, namedParameters);
            if (hasValue) paramCount += AssertParam("value", value, namedParameters);
            if (hasGeneric) paramCount += AssertParam("generic", this, namedParameters);
            if (hasInterface) paramCount += AssertParam("test", testInterface, namedParameters);

            Assert.Equal(namedParameters.Count, paramCount);
        }

        [Theory]
        [InlineData(false, false, false, false)]
        [InlineData(true, true, true, true)]
        [InlineData(false, false, true, false)]
        [InlineData(true, true, false, false)]
        [InlineData(true, false, true, true)]
        public void OrderParameters_WithUnorderedDictionaryParamaters_ReturnsOrderedList(bool hasInteger, bool hasValue, bool hasGeneric, bool hasInterface)
        {
            // Arrange
            const string value = "Value";
            int intValue = new Random().Next(1, 100);
            ITestInterface testInterface = new TestClass();
            Dictionary<string, object> parameters = [];

            if (hasGeneric) parameters.Add("generic", this);
            if (hasInteger) parameters.Add("integer", intValue);
            if (hasInterface) parameters.Add("test", testInterface);
            if (hasValue) parameters.Add("value", value);

            // Act
            IList<object> orderedList = InspectConstructor.Instance.OrderParameters(typeof(TestClass), parameters);

            // Assert
            OrderParametersAssert(hasInteger, hasValue, hasGeneric, hasInterface, value, intValue, testInterface, orderedList);
        }

        [Theory]
        [InlineData(false, false, false, false)]
        [InlineData(true, true, true, true)]
        [InlineData(false, false, true, false)]
        [InlineData(true, true, false, false)]
        [InlineData(true, false, true, true)]
        public void OrderParameters_WithUnorderedListParamaters_ReturnsOrderedList(bool hasInteger, bool hasValue, bool hasGeneric, bool hasInterface)
        {
            // Arrange
            const string value = "Value";
            int intValue = new Random().Next(1, 100);
            ITestInterface testInterface = new TestClass();
            List<(string, object)> parameters = [];

            if (hasGeneric) parameters.Add(ValueTuple.Create("generic", this));
            if (hasInteger) parameters.Add(ValueTuple.Create("integer", intValue));
            if (hasInterface) parameters.Add(ValueTuple.Create("test", testInterface));
            if (hasValue) parameters.Add(ValueTuple.Create("value", value));

            // Act
            IList<object> orderedList = InspectConstructor.Instance.OrderParameters(typeof(TestClass), parameters);

            // Assert
            OrderParametersAssert(hasInteger, hasValue, hasGeneric, hasInterface, value, intValue, testInterface, orderedList);
        }

        [Theory]
        [InlineData(false, false, false, false)]
        [InlineData(true, true, true, true)]
        [InlineData(false, false, true, false)]
        [InlineData(true, true, false, false)]
        [InlineData(true, false, true, true)]
        public void OrderParametersMap_WithDictionaryListParamaters_ReturnsOrderedList(bool hasInteger, bool hasValue, bool hasGeneric, bool hasInterface)
        {
            // Arrange
            const string value = "Value";
            int intValue = new Random().Next(1, 100);
            ITestInterface testInterface = new TestClass();
            Dictionary<string, object> parameters = [];

            if (hasGeneric) parameters.Add("generic", this);
            if (hasInteger) parameters.Add("integer", intValue);
            if (hasInterface) parameters.Add("test", testInterface);
            if (hasValue) parameters.Add("value", value);

            // Act
            IList<(Type, object)> orderedList = InspectConstructor.Instance.OrderParametersMap(typeof(TestClass), parameters);

            // Assert
            OrderParametersAssert(hasInteger, hasValue, hasGeneric, hasInterface, value, intValue, testInterface, orderedList);
        }

        [Theory]
        [InlineData(false, false, false, false)]
        [InlineData(true, true, true, true)]
        [InlineData(false, false, true, false)]
        [InlineData(true, true, false, false)]
        [InlineData(true, false, true, true)]
        public void OrderParametersMap_WithUnorderedListParamaters_ReturnsOrderedList(bool hasInteger, bool hasValue, bool hasGeneric, bool hasInterface)
        {
            // Arrange
            const string value = "Value";
            int intValue = new Random().Next(1, 100);
            ITestInterface testInterface = new TestClass();
            Dictionary<string, object> parameters = [];

            if (hasGeneric) parameters.Add("generic", this);
            if (hasInteger) parameters.Add("integer", intValue);
            if (hasInterface) parameters.Add("test", testInterface);
            if (hasValue) parameters.Add("value", value);

            // Act
            IList<(Type, object)> orderedList = InspectConstructor.Instance.OrderParametersMap(typeof(TestClass), parameters);

            // Assert
            OrderParametersAssert(hasInteger, hasValue, hasGeneric, hasInterface, value, intValue, testInterface, orderedList);
        }

        private static int AssertOrder<T>(T expectedValue, int index, IList<object> orderedParameters)
        {
            Assert.InRange(index, 0, orderedParameters.Count - 1);
            Assert.Equal(expectedValue, orderedParameters[index]);

            return index + 1;
        }

        private static int AssertOrder<T>(T expectedValue, int index, IList<(Type type, object value)> orderedParameters)
        {
            Assert.InRange(index, 0, orderedParameters.Count - 1);
            Assert.IsAssignableFrom(orderedParameters[index].type, expectedValue);
            Assert.Equal(expectedValue, orderedParameters[index].value);

            return index + 1;
        }

        private static int AssertParam<T>(string expectedName, T expectedValue, IList<(string, object)> namedParameters)
        {
            Assert.Contains(ValueTuple.Create(expectedName, expectedValue), namedParameters);
            return 1;
        }

        private void OrderParametersAssert(bool hasInteger, bool hasValue, bool hasGeneric, bool hasInterface, string value, int intValue, ITestInterface testInterface, IList<object> orderedList)
        {
            // Assert
            int index = 0;
            if (hasInteger) index = AssertOrder(intValue, index, orderedList);
            if (hasValue) index = AssertOrder(value, index, orderedList);
            if (hasGeneric) index = AssertOrder(this, index, orderedList);
            if (hasInterface) index = AssertOrder(testInterface, index, orderedList);

            Assert.Equal(orderedList.Count, index);
        }

        private void OrderParametersAssert(bool hasInteger, bool hasValue, bool hasGeneric, bool hasInterface, string value, int intValue, ITestInterface testInterface, IList<(Type, object)> orderedList)
        {
            // Assert
            int index = 0;
            if (hasInteger) index = AssertOrder(intValue, index, orderedList);
            if (hasValue) index = AssertOrder(value, index, orderedList);
            if (hasGeneric) index = AssertOrder(this, index, orderedList);
            if (hasInterface) index = AssertOrder(testInterface, index, orderedList);

            Assert.Equal(orderedList.Count, index);
        }

        #endregion Methods

        #region Interfaces

        private interface ITestInterface
        { }

        #endregion Interfaces

        #region Classes

        private class TestClass : ITestInterface
        {
            #region Constructors

            public TestClass()
            {
            }

            public TestClass(object generic) : this(null, null, generic, null)
            {
            }

            public TestClass(ITestInterface test) : this(null, null, null, test)
            {
            }

            public TestClass(int integer, string value) : this(integer, value, null, null)
            {
            }

            public TestClass(int integer, object generic, ITestInterface test) : this(integer, null, generic, test)
            {
            }

            public TestClass(int? integer, string value, object generic, ITestInterface test)
            {
                Integer = integer;
                Value = value;
                Generic = generic;
                Test = test;
            }

            private TestClass(int integer, string value, object generic, ITestInterface test) : this((int?)integer, value, generic, test)
            {
            }

            #endregion Constructors

            #region Properties

            public object Generic { get; }
            public int? Integer { get; }
            public ITestInterface Test { get; }
            public string Value { get; }

            #endregion Properties
        }

        #endregion Classes
    }
}