using System;
using MGR.CommandLineParser.Converters;
using Xunit;

namespace MGR.CommandLineParser.UnitTests.Converters
{
    public class Int32ConverterTests
    {
        [Fact]
        public void TargetType()
        {
            // Arrange
            IConverter converter = new Int32Converter();
            Type expectedType = typeof (Int32);

            // Act
            Type actualType = converter.TargetType;

            // Assert
            Assert.Equal(expectedType, actualType);
        }

        [Fact]
        public void Conversion()
        {
            // Arrange
            IConverter converter = new Int32Converter();
            string value = "42";
            int expectedValue = 42;

            // Act
            object actualValue = converter.Convert(value, converter.TargetType);

            // Assert
            Assert.NotNull(actualValue);
            Assert.IsType<Int32>(actualValue);
            Assert.Equal(expectedValue, (Int32) actualValue);
        }

        [Fact]
        public void BadValueConversion()
        {
            // Arrange
            IConverter converter = new Int32Converter();
            string value = "Hello";
            string expectedExceptionMessage = "Unable to parse 'Hello' to Int32.";
            string expectedInnerExceptionMessage = "Input string was not in a correct format.";

            // Act
            using (new LangageSwitcher("en-us"))
            {
                var actualException =
                    Assert.Throws<CommandLineParserException>(() => converter.Convert(value, converter.TargetType));

                // Assert
                Assert.Equal(expectedExceptionMessage, actualException.Message);
                Assert.NotNull(actualException.InnerException);
                var actualInnerExecption = Assert.IsAssignableFrom<FormatException>(actualException.InnerException);
                Assert.Equal(expectedInnerExceptionMessage, actualInnerExecption.Message);
            }
        }
    }
}