using System;
using MGR.CommandLineParser.Converters;
using Xunit;

namespace MGR.CommandLineParser.UnitTests.Converters
{
    public class TimeSpanConverterTests
    {
        [Fact]
        public void TargetType()
        {
            // Arrange
            IConverter converter = new TimeSpanConverter();
            Type expectedType = typeof (TimeSpan);

            // Act
            Type actualType = converter.TargetType;

            // Assert
            Assert.Equal(expectedType, actualType);
        }

        [Fact]
        public void Conversion()
        {
            // Arrange
            IConverter converter = new TimeSpanConverter();
            string value = "3:15:14";
            var expectedValue = new TimeSpan(3, 15, 14);

            // Act
            object actualValue = converter.Convert(value, converter.TargetType);

            // Assert
            Assert.NotNull(actualValue);
            Assert.IsType<TimeSpan>(actualValue);
            Assert.Equal(expectedValue, (TimeSpan) actualValue);
        }

        [Fact]
        [Trait("Exception", "CommandLineParserException")]
        public void BadValueConversion()
        {
            // Arrange
            IConverter converter = new TimeSpanConverter();
            string value = "Hello";
            string expectedExceptionMessage = Constants.ExceptionMessages.FormatConverterUnableConvert(value, typeof(TimeSpan));
            string expectedInnerExceptionMessage = "String was not recognized as a valid TimeSpan.";

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