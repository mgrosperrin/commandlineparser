using System;
using MGR.CommandLineParser.Command;
using MGR.CommandLineParser.Converters;
using Xunit;

namespace MGR.CommandLineParser.UnitTests.Extensions
{
    public partial class ConverterAttributeExtensionsTests
    {
        public class BuildKeyConverter
        {
            [Fact]
            public void Int32ConverterActivation()
            {
                // Arrange
                Type expected = typeof (Int32Converter);
                Type valueConverterType = typeof (GuidConverter);
                var converterAttribute = new ConverterKeyValueAttribute(valueConverterType, expected);

                // Act
                IConverter actual = converterAttribute.BuildKeyConverter();

                // Assert
                Assert.NotNull(actual);
                Assert.IsType<Int32Converter>(actual);
            }

            [Fact]
            public void DefaultStringConverterActivation()
            {
                // Arrange
                Type expected = typeof (StringConverter);
                Type ValueConverterType = typeof (Int32Converter);
                var converterAttribute = new ConverterKeyValueAttribute(ValueConverterType);

                // Act
                IConverter actual = converterAttribute.BuildKeyConverter();

                // Assert
                Assert.NotNull(actual);
                Assert.IsType<StringConverter>(actual);
            }

            [Fact]
            public void NullConverterAttributeException()
            {
                // Arrange
                ConverterKeyValueAttribute converterAttribute = null;
                string expectedExceptionMessage = SourceParameterName;

                // Act
                var actualException = Assert.Throws<ArgumentNullException>(() => converterAttribute.BuildKeyConverter());

                // Assert
                Assert.Equal(expectedExceptionMessage, actualException.ParamName);
            }
        }
    }
}