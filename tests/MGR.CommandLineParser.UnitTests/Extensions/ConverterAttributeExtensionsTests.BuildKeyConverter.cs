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
#pragma warning disable CS0618 // Type or member is obsolete
                var converterAttribute = new ConverterKeyValueAttribute(valueConverterType, expected);
#pragma warning restore CS0618 // Type or member is obsolete

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
                var valueConverterType = typeof (Int32Converter);
#pragma warning disable CS0618 // Type or member is obsolete
                var converterAttribute = new ConverterKeyValueAttribute(valueConverterType);
#pragma warning restore CS0618 // Type or member is obsolete

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
#pragma warning disable CS0618 // Type or member is obsolete
                ConverterKeyValueAttribute converterAttribute = null;
#pragma warning restore CS0618 // Type or member is obsolete
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