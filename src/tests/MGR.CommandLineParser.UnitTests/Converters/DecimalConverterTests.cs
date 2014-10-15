using System;
using MGR.CommandLineParser.Converters;
using Xunit;

namespace MGR.CommandLineParser.UnitTests.Converters
{
    public class DecimalConverterTests
    {
        [Fact]
        public void TargetType()
        {
            // Arrange
            IConverter converter = new DecimalConverter();
            Type expectedType = typeof (Decimal);

            // Act
            Type actualType = converter.TargetType;

            // Assert
            Assert.Equal(expectedType, actualType);
        }

        [Fact]
        public void Conversion()
        {
            // Arrange
            IConverter converter = new DecimalConverter();
            string value = "42,1";
            var expectedValue = (Decimal) 42.1;

            // Act
            using (new LangageSwitcher("fr-fr"))
            {
                object actualValue = converter.Convert(value, converter.TargetType);

                // Assert
                Assert.NotNull(actualValue);
                Assert.IsType<decimal>(actualValue);
                Assert.Equal(expectedValue, (Decimal) actualValue);
            }
        }

        [Fact]
        public void BadValueConversion()
        {
            // Arrange
            IConverter converter = new DecimalConverter();
            string value = "Hello";
            string expectedExceptionMessage = "Unable to parse 'Hello' to Decimal.";
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