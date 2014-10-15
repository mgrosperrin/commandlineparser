using System;
using System.Collections.Generic;
using System.Reflection;
using Xunit;

namespace MGR.CommandLineParser.UnitTests.Extensions
{
    public partial class PropertyInfoExtensionsTests
    {
        public class IsValidOptionProperty
        {
            public int WritableProperty { get; set; }

            public int NonWritableProperty
            {
                get { return 0; }
            }

            public List<int> NonWritableMultiValueProperty
            {
                get { return new List<int>(); }
            }

            [Fact]
            public void WritableTest()
            {
                // Arrange
                PropertyInfo propertyToTest =
                    GetType().GetProperty(TypeHelpers.ExtractPropertyName(() => WritableProperty));
                bool expected = true;

                // Act
                bool actual = propertyToTest.IsValidOptionProperty();

                // Assert
                Assert.Equal(expected, actual);
            }

            [Fact]
            public void NonWritableTest()
            {
                // Arrange
                PropertyInfo propertyToTest =
                    GetType().GetProperty(TypeHelpers.ExtractPropertyName(() => NonWritableProperty));
                bool expected = false;

                // Act
                bool actual = propertyToTest.IsValidOptionProperty();

                // Assert
                Assert.Equal(expected, actual);
            }

            [Fact]
            public void NonWritableMultiValueTest()
            {
                // Arrange
                PropertyInfo propertyToTest =
                    GetType().GetProperty(TypeHelpers.ExtractPropertyName(() => NonWritableMultiValueProperty));
                bool expected = true;

                // Act
                bool actual = propertyToTest.IsValidOptionProperty();

                // Assert
                Assert.Equal(expected, actual);
            }

            [Fact]
            public void NullPropertyInfoException()
            {
                // Arrange
                PropertyInfo testedProperty = null;
                string expectedExceptionMessage = @"source";

                // Act
                var actualException = Assert.Throws<ArgumentNullException>(() => testedProperty.IsValidOptionProperty());

                // Assert
                Assert.Equal(expectedExceptionMessage, actualException.ParamName);
            }
        }
    }
}