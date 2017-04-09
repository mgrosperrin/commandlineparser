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
            var expectedType = typeof (string);

            // Act
            var actualType = converter.TargetType;

            // Assert
            Assert.Equal(expectedType, actualType);
        }

        [Fact]
        public void Conversion()
        {
            // Arrange
            IConverter converter = new StringConverter();
            var expectedValue = "value";

            // Act
            var actualValue = converter.Convert(expectedValue, converter.TargetType);

            // Assert
            Assert.NotNull(actualValue);
            Assert.IsType<string>(actualValue);
            Assert.Equal(expectedValue, (string) actualValue);
        }
    }
}