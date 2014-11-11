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
            Type expectedType = typeof (KeyValuePair<string, int>);

            // Act
            Type actualType = converter.TargetType;

            // Assert
            Assert.Equal(expectedType, actualType);
        }

        [Fact]
        public void Conversion()
        {
            // Arrange
            IConverter converter = new KeyValueConverter(new StringConverter(), new Int32Converter());
            string value = "Test=42";
            string expectedKeyValue = "Test";
            int expectedValueValue = 42;

            // Act
            object actualValue = converter.Convert(value, converter.TargetType);

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
            string value = "Test";
            string expectedKeyValue = "Test";

            // Act
            object actualValue = converter.Convert(value, converter.TargetType);

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
            string value = "Hello=Hello";
            string expectedExceptionMessage = "Unable to parse 'Hello' to Int32.";
            string expectedInnerExceptionMessage = "Input string was not in a correct format.";

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
            string expectedInnerExceptionMessage = @"keyConverter";

            // Act
            var actualException = Assert.Throws<ArgumentNullException>(() => new KeyValueConverter(null, new Int32Converter()));

            // Assert
            Assert.Equal(expectedInnerExceptionMessage, actualException.ParamName);
        }

        [Fact]
        public void ArgumentNullExceptionForNullValueConverter()
        {
            // Arrange
            string expectedInnerExceptionMessage = @"valueConverter";

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
            string expectedInnerExceptionMessage = @"value";
            string value = string.Empty;

            // Act
            var actualException = Assert.Throws<ArgumentNullException>(() => converter.Convert(value, converter.TargetType));

            // Assert
            Assert.Equal(expectedInnerExceptionMessage, actualException.ParamName);
        }
    }
}