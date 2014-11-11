using System;
using MGR.CommandLineParser.Converters;
using Xunit;

namespace MGR.CommandLineParser.UnitTests.Converters
{
    public class DoubleConverterTests
    {
        [Fact]
        public void TargetType()
        {
            // Arrange
            IConverter converter = new DoubleConverter();
            Type expectedType = typeof (Double);

            // Act
            Type actualType = converter.TargetType;

            // Assert
            Assert.Equal(expectedType, actualType);
        }

        [Fact]
        public void Conversion()
        {
            // Arrange
            IConverter converter = new DoubleConverter();
            string value = "42,35";
            double expectedValue = 42.35;

            // Act
            using (new LangageSwitcher("fr-fr"))
            {
                object actualValue = converter.Convert(value, converter.TargetType);

                // Assert
                Assert.NotNull(actualValue);
                Assert.IsType<double>(actualValue);
                Assert.Equal(expectedValue, (Double) actualValue);
            }
        }

        [Fact]
        public void BadValueConversion()
        {
            // Arrange
            IConverter converter = new DoubleConverter();
            string value = "Hello";
            string expectedExceptionMessage = "Unable to parse 'Hello' to Double.";
            string expectedInnerExceptionMessage = "Input string was not in a correct format.";

            // Act
            using (new LangageSwitcher("en-us"))
            {
                var actualException = Assert.Throws<CommandLineParserException>(() => converter.Convert(value, converter.TargetType));

                // Assert
                Assert.Equal(expectedExceptionMessage, actualException.Message);
                Assert.NotNull(actualException.InnerException);
                var actualInnerExecption = Assert.IsAssignableFrom<FormatException>(actualException.InnerException);
                Assert.Equal(expectedInnerExceptionMessage, actualInnerExecption.Message);
            }
        }
    }
}