using System;
using MGR.CommandLineParser.Command;
using MGR.CommandLineParser.Converters;
using Xunit;

namespace MGR.CommandLineParser.UnitTests.Extensions
{
    public partial class ConverterAttributeExtensionsTests
    {
        public class BuildValueConverter
        {
            [Fact]
            public void Int32ConverterActivation()
            {
                // Arrange
                var keyConverterType = typeof (GuidConverter);
                var expected = typeof (Int32Converter);
                var converterAttribute = new ConverterKeyValueAttribute(expected, keyConverterType);

                // Act
                var actual = converterAttribute.BuildValueConverter();

                // Assert
                Assert.NotNull(actual);
                Assert.IsType<Int32Converter>(actual);
            }

            [Fact]
            public void DefaultStringConverterActivation()
            {
                // Arrange
                var expected = typeof (Int32Converter);
                var converterAttribute = new ConverterKeyValueAttribute(expected);

                // Act
                var actual = converterAttribute.BuildValueConverter();

                // Assert
                Assert.NotNull(actual);
                Assert.IsType<Int32Converter>(actual);
            }

            [Fact]
            public void NullConverterAttributeException()
            {
                // Arrange
                ConverterKeyValueAttribute converterAttribute = null;
                var expectedExceptionMessage = SourceParameterName;

                // Act
                var actualException =
                    Assert.Throws<ArgumentNullException>(() => converterAttribute.BuildValueConverter());

                // Assert
                Assert.Equal(expectedExceptionMessage, actualException.ParamName);
            }
        }
    }
}