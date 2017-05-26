using System;
using MGR.CommandLineParser.Extensibility.Converters;
using Xunit;

namespace MGR.CommandLineParser.UnitTests.Extensibility.Converters
{
    public class DateTimeConverterTests
    {
        [Fact]
        public void TargetType()
        {
            // Arrange
            IConverter converter = new DateTimeConverter();
            var expectedType = typeof (DateTime);

            // Act
            var actualType = converter.TargetType;

            // Assert
            Assert.Equal(expectedType, actualType);
        }

        [Fact]
        public void ConversionFrench()
        {
            // Arrange
            IConverter converter = new DateTimeConverter();
            var value = "01/10/2012";
            var expectedValue = new DateTime(2012, 10, 01);

            // Act
            using (new LangageSwitcher("fr-fr"))
            {
                var actualValue = converter.Convert(value, converter.TargetType);

                // Assert
                Assert.NotNull(actualValue);
                Assert.IsType<DateTime>(actualValue);
                Assert.Equal(expectedValue, (DateTime) actualValue);
            }
        }

        [Fact]
        public void ConversionAmerican()
        {
            // Arrange
            IConverter converter = new DateTimeConverter();
            var value = "2012/10/01";
            var expectedValue = new DateTime(2012, 10, 01);

            // Act
            using (new LangageSwitcher("en-us"))
            {
                var actualValue = converter.Convert(value, converter.TargetType);

                // Assert
                Assert.NotNull(actualValue);
                Assert.IsType<DateTime>(actualValue);
                Assert.Equal(expectedValue, (DateTime) actualValue);
            }
        }

        [Fact]
        public void BadValueConversion()
        {
            // Arrange
            IConverter converter = new DateTimeConverter();
            var value = "Hello";
            var expectedExceptionMessage = Constants.ExceptionMessages.FormatConverterUnableConvert(value, typeof(DateTime));
            var expectedInnerExceptionMessage =
                "The string was not recognized as a valid DateTime. There is an unknown word starting at index 0.";

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