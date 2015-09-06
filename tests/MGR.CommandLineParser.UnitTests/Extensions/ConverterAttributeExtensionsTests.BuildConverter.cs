using System;
using MGR.CommandLineParser.Command;
using MGR.CommandLineParser.Converters;
using Xunit;

namespace MGR.CommandLineParser.UnitTests.Extensions
{
    public partial class ConverterAttributeExtensionsTests
    {
        public class BuildConverter
        {
            [Fact]
            public void Int32ConverterActivation()
            {
                // Arrange
                Type expected = typeof (Int32Converter);
                var converterAttribute = new ConverterAttribute(expected);

                // Act
                IConverter actual = converterAttribute.BuildConverter();

                // Assert
                Assert.NotNull(actual);
                Assert.IsType<Int32Converter>(actual);
            }

            [Fact]
            public void NullConverterAttributeException()
            {
                // Arrange
                ConverterAttribute converterAttribute = null;
                string expectedExceptionMessage = SourceParameterName;

                // Act
                var actualException = Assert.Throws<ArgumentNullException>(() => converterAttribute.BuildConverter());

                // Assert
                Assert.Equal(expectedExceptionMessage, actualException.ParamName);
            }
        }
    }
}