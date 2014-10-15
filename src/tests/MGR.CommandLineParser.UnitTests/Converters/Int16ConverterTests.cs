using System;
using MGR.CommandLineParser.Converters;
using Xunit;

namespace MGR.CommandLineParser.UnitTests.Converters
{
    public class Int16ConverterTests
    {
        [Fact]
        public void TargetType()
        {
            // Arrange
            IConverter converter = new Int16Converter();
            Type expectedType = typeof (Int16);

            // Act
            Type actualType = converter.TargetType;

            // Assert
            Assert.Equal(expectedType, actualType);
        }

        [Fact]
        public void Conversion()
        {
            // Arrange
            IConverter converter = new Int16Converter();
            string value = "42";
            Int16 expectedValue = 42;

            // Act
            object actualValue = converter.Convert(value, converter.TargetType);

            // Assert
            Assert.NotNull(actualValue);
            Assert.IsType<Int16>(actualValue);
            Assert.Equal(expectedValue, (Int16) actualValue);
        }

        [Fact]
        public void BadValueConversion()
        {
            // Arrange
            IConverter converter = new Int16Converter();
            string value = "Hello";
            string expectedExceptionMessage = "Unable to parse 'Hello' to Int16.";
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