using System;
using MGR.CommandLineParser.Converters;
using Xunit;

namespace MGR.CommandLineParser.UnitTests.Converters
{
    public class DateTimeConverterTests
    {
        [Fact]
        public void TargetType()
        {
            // Arrange
            IConverter converter = new DateTimeConverter();
            Type expectedType = typeof (DateTime);

            // Act
            Type actualType = converter.TargetType;

            // Assert
            Assert.Equal(expectedType, actualType);
        }

        [Fact]
        public void ConversionFrench()
        {
            // Arrange
            IConverter converter = new DateTimeConverter();
            string value = "01/10/2012";
            var expectedValue = new DateTime(2012, 10, 01);

            // Act
            using (new LangageSwitcher("fr-fr"))
            {
                object actualValue = converter.Convert(value, converter.TargetType);

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
            string value = "2012/10/01";
            var expectedValue = new DateTime(2012, 10, 01);

            // Act
            using (new LangageSwitcher("en-us"))
            {
                object actualValue = converter.Convert(value, converter.TargetType);

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
            string value = "Hello";
            string expectedExceptionMessage = "Unable to parse 'Hello' to DateTime.";
            string expectedInnerExceptionMessage =
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