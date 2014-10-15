using System;
using MGR.CommandLineParser.Converters;
using Xunit;

namespace MGR.CommandLineParser.UnitTests.Converters
{
    public class UriConverterTests
    {
        [Fact]
        public void TargetType()
        {
            // Arrange
            IConverter converter = new UriConverter();
            Type expectedType = typeof (Uri);

            // Act
            Type actualType = converter.TargetType;

            // Assert
            Assert.Equal(expectedType, actualType);
        }

        [Fact]
        public void AbsoluteConversion()
        {
            // Arrange
            IConverter converter = new UriConverter();
            string value = "http://mgrcommandlineparser.codeplex.com";
            var expectedValue = new Uri("http://mgrcommandlineparser.codeplex.com");

            // Act
            object actualValue = converter.Convert(value, converter.TargetType);

            // Assert
            Assert.NotNull(actualValue);
            Assert.IsType<Uri>(actualValue);
            Assert.Equal(expectedValue, (Uri) actualValue);
        }

        [Fact]
        public void RelativeConversion()
        {
            // Arrange
            IConverter converter = new UriConverter();
            string value = "hello";
            var expectedValue = new Uri("hello", UriKind.Relative);

            // Act
            object actualValue = converter.Convert(value, converter.TargetType);

            // Assert
            Assert.NotNull(actualValue);
            Assert.IsType<Uri>(actualValue);
            Assert.Equal(expectedValue, (Uri) actualValue);
        }
    }
}