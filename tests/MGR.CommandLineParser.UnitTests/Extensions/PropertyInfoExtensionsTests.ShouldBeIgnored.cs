using System;
using System.Reflection;
using MGR.CommandLineParser.Command;
using Xunit;

namespace MGR.CommandLineParser.UnitTests.Extensions
{
    public partial class PropertyInfoExtensionsTests
    {
        public class ShouldBeIgnored
        {
            public int WritableProperty { get; set; }

            [IgnoreOptionProperty]
            public int WritableIgnoredProperty { get; set; }

            [Fact]
            public void WritableTest()
            {
                // Arrange
                var propertyToTest =
                    GetType().GetProperty(TypeHelpers.ExtractPropertyName(() => WritableProperty));
                var expected = false;

                // Act
                var actual = propertyToTest.ShouldBeIgnored();

                // Assert
                Assert.Equal(expected, actual);
            }

            [Fact]
            public void NonWritableTest()
            {
                // Arrange
                var propertyToTest =
                    GetType().GetProperty(TypeHelpers.ExtractPropertyName(() => WritableIgnoredProperty));
                var expected = true;

                // Act
                var actual = propertyToTest.ShouldBeIgnored();

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
                var actualException = Assert.Throws<ArgumentNullException>(() => testedProperty.ShouldBeIgnored());

                // Assert
                Assert.Equal(expectedExceptionMessage, actualException.ParamName);
            }
        }
    }
}