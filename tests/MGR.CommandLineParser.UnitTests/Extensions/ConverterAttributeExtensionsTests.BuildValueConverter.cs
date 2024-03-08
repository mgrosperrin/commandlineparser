using System;
using MGR.CommandLineParser.Command;
using MGR.CommandLineParser.Extensibility.Converters;
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
#pragma warning disable CS0618 // Type or member is obsolete
                var converterAttribute = new ConverterKeyValueAttribute(expected, keyConverterType);
#pragma warning restore CS0618 // Type or member is obsolete

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
#pragma warning disable CS0618 // Type or member is obsolete
                var converterAttribute = new ConverterKeyValueAttribute(expected);
#pragma warning restore CS0618 // Type or member is obsolete

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
#pragma warning disable CS0618 // Type or member is obsolete
                ConverterKeyValueAttribute converterAttribute = null;
#pragma warning restore CS0618 // Type or member is obsolete
                var expectedExceptionMessage = SourceParameterName;

                // Act
                var actualException =
                    // ReSharper disable once ExpressionIsAlwaysNull
                    Assert.Throws<ArgumentNullException>(() => converterAttribute.BuildValueConverter());

                // Assert
                Assert.Equal(expectedExceptionMessage, actualException.ParamName);
            }
        }
    }
}