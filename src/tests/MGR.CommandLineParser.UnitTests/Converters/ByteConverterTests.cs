using System;
using MGR.CommandLineParser.Converters;
using Xunit;

namespace MGR.CommandLineParser.UnitTests.Converters
{
    public class ByteConverterTests
    {
        [Fact]
        public void TargetType()
        {
            // Arrange
            IConverter converter = new ByteConverter();
            Type expectedType = typeof (Byte);

            // Act
            Type actualType = converter.TargetType;

            // Assert
            Assert.Equal(expectedType, actualType);
        }

        [Fact]
        public void Conversion()
        {
            // Arrange
            IConverter converter = new ByteConverter();
            string value = "42";
            byte expectedValue = 42;

            // Act
            object actualValue = converter.Convert(value, converter.TargetType);

            // Assert
            Assert.NotNull(actualValue);
            Assert.IsType<byte>(actualValue);
            Assert.Equal(expectedValue, (Byte) actualValue);
        }

        [Fact]
        public void BadValueConversion()
        {
            // Arrange
            IConverter converter = new ByteConverter();
            string value = "Hello";
            string expectedExceptionMessage = "Unable to parse 'Hello' to Byte.";
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