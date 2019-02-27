using System;
using MGR.CommandLineParser.Command;
using MGR.CommandLineParser.Extensibility.Converters;
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
                var expected = typeof (Int32Converter);
                var valueConverterType = typeof (GuidConverter);
                var converterAttribute = new ConverterKeyValueAttribute(valueConverterType, expected);

                // Act
                var actual = converterAttribute.BuildKeyConverter();

                // Assert
                Assert.NotNull(actual);
                Assert.IsType<Int32Converter>(actual);
            }

            [Fact]
            public void DefaultStringConverterActivation()
            {
                // Arrange
                var expected = typeof (StringConverter);
                var ValueConverterType = typeof (Int32Converter);
                var converterAttribute = new ConverterKeyValueAttribute(ValueConverterType);

                // Act
                var actual = converterAttribute.BuildKeyConverter();

                // Assert
                Assert.NotNull(actual);
                Assert.IsType<StringConverter>(actual);
            }

            [Fact]
            public void NullConverterAttributeException()
            {
                // Arrange
                ConverterKeyValueAttribute converterAttribute = null;
                var expectedExceptionMessage = SourceParameterName;

                // Act
                // ReSharper disable once ExpressionIsAlwaysNull
                var actualException = Assert.Throws<ArgumentNullException>(() => converterAttribute.BuildKeyConverter());

                // Assert
                Assert.Equal(expectedExceptionMessage, actualException.ParamName);
            }
        }
    }
}