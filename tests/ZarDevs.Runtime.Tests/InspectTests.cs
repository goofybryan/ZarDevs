using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace ZarDevs.Runtime.Tests
{
    public class InspectTests
    {
        [Theory]
        [InlineData(false, false, false, false)]
        [InlineData(true, true, true, true)]
        [InlineData(false, false, true, false)]
        [InlineData(true, true, false, false)]
        [InlineData(true, false, true, true)]
        public void TestFindConstructParameterNames(bool hasInteger, bool hasValue, bool hasGeneric, bool hasInterface)
        {
            // Arrange
            const string value = "Value";
            IList<object> parameters = new List<object>();
            int intValue = new Random().Next(1, 100);
            ITestInterface testInterface = new TestClass();

            if (hasInteger) parameters.Add(intValue);
            if (hasValue) parameters.Add(value);
            if (hasGeneric) parameters.Add(this);
            if (hasInterface) parameters.Add(testInterface);

            // Act
            IList<(string, object)> namedParameters = InspectConstructor.Instance.FindParameterNames(typeof(TestClass), parameters);

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
        public void TestOrderConstructorParameters(bool hasInteger, bool hasValue, bool hasGeneric, bool hasInterface)
        {
            // Arrange
            const string value = "Value";
            int intValue = new Random().Next(1, 100);
            ITestInterface testInterface = new TestClass();
            IDictionary<string, object> parameters = new Dictionary<string, object>();

            if (hasGeneric) parameters.Add("generic", this);
            if (hasInteger) parameters.Add("integer", intValue);
            if (hasInterface) parameters.Add("test", testInterface);
            if (hasValue) parameters.Add("value", value);

            // Act
            IList<object> orderedList = InspectConstructor.Instance.OrderParameters(typeof(TestClass), parameters);

            // Assert
            int index = 0;
            if (hasInteger) index = AssertOrder(intValue, index, orderedList);
            if (hasValue) index = AssertOrder(value, index, orderedList);
            if (hasGeneric) index = AssertOrder(this, index, orderedList);
            if (hasInterface) index = AssertOrder(testInterface, index, orderedList);

            Assert.Equal(orderedList.Count, index);
        }

        private int AssertParam<T>(string expectedName, T expectedValue, IList<(string, object)> namedParameters)
        {
            Assert.Contains(ValueTuple.Create(expectedName, expectedValue), namedParameters);
            return 1;
        }

        private int AssertOrder<T>(T expectedValue, int index, IList<object> orderedParameters)
        {
            Assert.Equal(expectedValue, orderedParameters[index]);

            return index+1;
        }

        private class TestClass : ITestInterface
        {
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

            private TestClass(int integer, string value, object generic, ITestInterface test) : this((int?)integer, value, generic, test)
            {
            }

            public TestClass(int? integer, string value, object generic, ITestInterface test)
            {
                Integer = integer;
                Value = value;
                Generic = generic;
                Test = test;
            }

            public int? Integer { get; }
            public string Value { get; }
            public object Generic { get; }
            public ITestInterface Test { get; }
        }

        private interface ITestInterface
        { }
    }
}
