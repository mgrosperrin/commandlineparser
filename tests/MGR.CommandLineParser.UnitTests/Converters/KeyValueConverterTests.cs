using System;
using System.Collections.Generic;
using MGR.CommandLineParser.Converters;
using Xunit;

namespace MGR.CommandLineParser.UnitTests.Converters
{
    public class KeyValueConverterTests
    {
        [Fact]
        public void TargetType()
        {
            // Arrange
            IConverter converter = new KeyValueConverter(new StringConverter(), new Int32Converter());
            var expectedType = typeof (KeyValuePair<string, int>);

            // Act
            var actualType = converter.TargetType;

            // Assert
            Assert.Equal(expectedType, actualType);
        }

        [Fact]
        public void Conversion()
        {
            // Arrange
            IConverter converter = new KeyValueConverter(new StringConverter(), new Int32Converter());
            var value = "Test=42";
            var expectedKeyValue = "Test";
            var expectedValueValue = 42;

            // Act
            var actualValue = converter.Convert(value, converter.TargetType);

            // Assert
            Assert.NotNull(actualValue);
            var actualTuple = Assert.IsAssignableFrom<Tuple<object, object>>(actualValue);
            Assert.Equal(expectedKeyValue, (string) actualTuple.Item1);
            Assert.Equal(expectedValueValue, (int) actualTuple.Item2);
        }

        [Fact]
        public void ConversionWithNullValue()
        {
            // Arrange
            IConverter converter = new KeyValueConverter(new StringConverter(), new Int32Converter());
            var value = "Test";
            var expectedKeyValue = "Test";

            // Act
            var actualValue = converter.Convert(value, converter.TargetType);

            // Assert
            Assert.NotNull(actualValue);
            var actualTuple = Assert.IsAssignableFrom<Tuple<object, object>>(actualValue);
            Assert.Equal(expectedKeyValue, (string) actualTuple.Item1);
            Assert.Null(actualTuple.Item2);
        }

        [Fact]
        public void BadValueConversion()
        {
            // Arrange
            IConverter converter = new KeyValueConverter(new StringConverter(), new Int32Converter());
            var value = "Hello=Hello";
            var expectedExceptionMessage = Constants.ExceptionMessages.FormatConverterUnableConvert("Hello", typeof(int));
            var expectedInnerExceptionMessage = "Input string was not in a correct format.";

            // Act
            using (new LangageSwitcher("en-us"))
            {
                var actualException = Assert.Throws<CommandLineParserException>(() => converter.Convert(value, converter.TargetType));

                // Assert
                Assert.Equal(expectedExceptionMessage, actualException.Message);
                Assert.NotNull(actualException.InnerException);
                var actualInnerExecption = Assert.IsAssignableFrom<FormatException>(actualException.InnerException);
                Assert.Equal(expectedInnerExceptionMessage, actualInnerExecption.Message);
            }
        }

        [Fact]
        public void ArgumentNullExceptionForNullKeyConverter()
        {
            // Arrange
            var expectedInnerExceptionMessage = @"keyConverter";

            // Act
            var actualException = Assert.Throws<ArgumentNullException>(() => new KeyValueConverter(null, new Int32Converter()));

            // Assert
            Assert.Equal(expectedInnerExceptionMessage, actualException.ParamName);
        }

        [Fact]
        public void ArgumentNullExceptionForNullValueConverter()
        {
            // Arrange
            var expectedInnerExceptionMessage = @"valueConverter";

            // Act
            var actualException = Assert.Throws<ArgumentNullException>(() => new KeyValueConverter(new StringConverter(), null));

            // Assert
            Assert.Equal(expectedInnerExceptionMessage, actualException.ParamName);
        }

        [Fact]
        public void EmptyValueConverter()
        {
            // Arrange
            IConverter converter = new KeyValueConverter(new StringConverter(), new Int32Converter());
            var expectedInnerExceptionMessage = @"value";
            var value = string.Empty;

            // Act
            var actualException = Assert.Throws<ArgumentNullException>(() => converter.Convert(value, converter.TargetType));

            // Assert
            Assert.Equal(expectedInnerExceptionMessage, actualException.ParamName);
        }
    }
}