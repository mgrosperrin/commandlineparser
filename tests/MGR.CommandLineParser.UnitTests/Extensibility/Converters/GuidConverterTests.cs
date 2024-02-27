using System;
using MGR.CommandLineParser.Extensibility.Converters;
using Xunit;

namespace MGR.CommandLineParser.UnitTests.Extensibility.Converters
{
    public class GuidConverterTests
    {
        [Fact]
        public void TargetType()
        {
            // Arrange
            IConverter converter = new GuidConverter();
            var expectedType = typeof (Guid);

            // Act
            var actualType = converter.TargetType;

            // Assert
            Assert.Equal(expectedType, actualType);
        }

        [Fact]
        public void Conversion()
        {
            // Arrange
            IConverter converter = new GuidConverter();
            var expectedValue = Guid.NewGuid();
            var value = expectedValue.ToString();

            // Act
            var actualValue = converter.Convert(value, converter.TargetType);

            // Assert
            Assert.NotNull(actualValue);
            Assert.IsType<Guid>(actualValue);
            Assert.Equal(expectedValue, (Guid) actualValue);
        }

        [Fact]
        public void BadValueConversion()
        {
            // Arrange
            IConverter converter = new GuidConverter();
            var value = "Hello";
            var expectedExceptionMessage = Constants.ExceptionMessages.FormatConverterUnableConvert(value, typeof(Guid));
#if NET
            var expectedInnerExceptionMessage = "Unrecognized Guid format.";
#else
            var expectedInnerExceptionMessage = "Guid should contain 32 digits with 4 dashes (xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx).";
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