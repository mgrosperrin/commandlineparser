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
                var propertyToTest =
                    GetType().GetProperty(TypeHelpers.ExtractPropertyName(() => WritableProperty));
                var expected = true;

                // Act
                var actual = propertyToTest.IsValidOptionProperty();

                // Assert
                Assert.Equal(expected, actual);
            }

            [Fact]
            public void NonWritableTest()
            {
                // Arrange
                var propertyToTest =
                    GetType().GetProperty(TypeHelpers.ExtractPropertyName(() => NonWritableProperty));
                var expected = false;

                // Act
                var actual = propertyToTest.IsValidOptionProperty();

                // Assert
                Assert.Equal(expected, actual);
            }

            [Fact]
            public void NonWritableMultiValueTest()
            {
                // Arrange
                var propertyToTest =
                    GetType().GetProperty(TypeHelpers.ExtractPropertyName(() => NonWritableMultiValueProperty));
                var expected = true;

                // Act
                var actual = propertyToTest.IsValidOptionProperty();

                // Assert
                Assert.Equal(expected, actual);
            }

            [Fact]
            public void NullPropertyInfoException()
            {
                // Arrange
                PropertyInfo testedProperty = null;
                var expectedExceptionMessage = SourceParameterName;

                // Act
                var actualException = Assert.Throws<ArgumentNullException>(() => testedProperty.IsValidOptionProperty());

                // Assert
                Assert.Equal(expectedExceptionMessage, actualException.ParamName);
            }
        }
    }
}