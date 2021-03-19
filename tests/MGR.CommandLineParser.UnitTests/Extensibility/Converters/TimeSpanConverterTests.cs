using System;
using MGR.CommandLineParser.Extensibility.Converters;
using Xunit;

namespace MGR.CommandLineParser.UnitTests.Extensibility.Converters
{
    public class TimeSpanConverterTests
    {
        [Fact]
        public void TargetType()
        {
            // Arrange
            IConverter converter = new TimeSpanConverter();
            var expectedType = typeof (TimeSpan);

            // Act
            var actualType = converter.TargetType;

            // Assert
            Assert.Equal(expectedType, actualType);
        }

        [Fact]
        public void Conversion()
        {
            // Arrange
            IConverter converter = new TimeSpanConverter();
            var value = "3:15:14";
            var expectedValue = new TimeSpan(3, 15, 14);

            // Act
            var actualValue = converter.Convert(value, converter.TargetType);

            // Assert
            Assert.NotNull(actualValue);
            Assert.IsType<TimeSpan>(actualValue);
            Assert.Equal(expectedValue, (TimeSpan) actualValue);
        }

        [Fact]
        [Trait(nameof(Exception), nameof(CommandLineParserException))]
        public void BadValueConversion()
        {
            // Arrange
            IConverter converter = new TimeSpanConverter();
            var value = "Hello";
            var expectedExceptionMessage = Constants.ExceptionMessages.FormatConverterUnableConvert(value, typeof(TimeSpan));
#if NET48
            var expectedInnerExceptionMessage = "String was not recognized as a valid TimeSpan.";
#else
            var expectedInnerExceptionMessage = "String 'Hello' was not recognized as a valid TimeSpan.";
#endif

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