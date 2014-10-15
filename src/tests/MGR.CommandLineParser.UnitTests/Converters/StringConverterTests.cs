using System;
using MGR.CommandLineParser.Converters;
using Xunit;

namespace MGR.CommandLineParser.UnitTests.Converters
{
    public class StringConverterTests
    {
        [Fact]
        public void TargetType()
        {
            // Arrange
            IConverter converter = new StringConverter();
            Type expectedType = typeof (string);

            // Act
            Type actualType = converter.TargetType;

            // Assert
            Assert.Equal(expectedType, actualType);
        }

        [Fact]
        public void Conversion()
        {
            // Arrange
            IConverter converter = new StringConverter();
            string expectedValue = "value";

            // Act
            object actualValue = converter.Convert(expectedValue, converter.TargetType);

            // Assert
            Assert.NotNull(actualValue);
            Assert.IsType<string>(actualValue);
            Assert.Equal(expectedValue, (string) actualValue);
        }
    }
}