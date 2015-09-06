using System;
using MGR.CommandLineParser.Converters;
using Xunit;

namespace MGR.CommandLineParser.UnitTests.Converters
{
    public class SingleConvertersTests
    {
        [Fact]
        public void TargetType()
        {
            // Arrange
            IConverter converter = new SingleConverter();
            var expectedType = typeof (Single);

            // Act
            var actualType = converter.TargetType;

            // Assert
            Assert.Equal(expectedType, actualType);
        }

        [Fact]
        public void Conversion()
        {
            // Arrange
            IConverter converter = new SingleConverter();
            var value = "42,1";
            var expectedValue = (Single) 42.1;

            // Act
            using (new LangageSwitcher("fr-fr"))
            {
                var actualValue = converter.Convert(value, converter.TargetType);

                // Assert
                Assert.NotNull(actualValue);
                Assert.IsType<Single>(actualValue);
                Assert.Equal(expectedValue, (Single) actualValue);
            }
        }

        [Fact]
        [Trait(nameof(Exception), nameof(CommandLineParserException))]
        public void BadValueConversion()
        {
            // Arrange
            IConverter converter = new SingleConverter();
            var value = "Hello";
            var expectedExceptionMessage = Constants.ExceptionMessages.FormatConverterUnableConvert(value, typeof(float));
            var expectedInnerExceptionMessage = "Input string was not in a correct format.";

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