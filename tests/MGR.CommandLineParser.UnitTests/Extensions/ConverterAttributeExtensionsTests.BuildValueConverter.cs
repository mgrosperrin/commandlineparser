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
                Type keyConverterType = typeof (GuidConverter);
                Type expected = typeof (Int32Converter);
                var converterAttribute = new ConverterKeyValueAttribute(expected, keyConverterType);

                // Act
                IConverter actual = converterAttribute.BuildValueConverter();

                // Assert
                Assert.NotNull(actual);
                Assert.IsType<Int32Converter>(actual);
            }

            [Fact]
            public void DefaultStringConverterActivation()
            {
                // Arrange
                Type expected = typeof (Int32Converter);
                var converterAttribute = new ConverterKeyValueAttribute(expected);

                // Act
                IConverter actual = converterAttribute.BuildValueConverter();

                // Assert
                Assert.NotNull(actual);
                Assert.IsType<Int32Converter>(actual);
            }

            [Fact]
            public void NullConverterAttributeException()
            {
                // Arrange
                ConverterKeyValueAttribute converterAttribute = null;
                string expectedExceptionMessage = SourceParameterName;

                // Act
                var actualException =
                    Assert.Throws<ArgumentNullException>(() => converterAttribute.BuildValueConverter());

                // Assert
                Assert.Equal(expectedExceptionMessage, actualException.ParamName);
            }
        }
    }
}