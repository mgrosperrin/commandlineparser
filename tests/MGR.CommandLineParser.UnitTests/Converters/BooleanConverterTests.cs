using System;
using MGR.CommandLineParser.Converters;
using Xunit;

namespace MGR.CommandLineParser.UnitTests.Converters
{
    public class BooleanConverterTests
    {
        [Fact]
        public void TargetType()
        {
            // Arrange
            IConverter converter = new BooleanConverter();
            Type expectedType = typeof (Boolean);

            // Act
            Type actualType = converter.TargetType;

            // Assert
            Assert.Equal(expectedType, actualType);
        }

        [Fact]
        public void StringEmptyConversion()
        {
            // Arrange
            IConverter converter = new BooleanConverter();
            string value = string.Empty;
            bool expectedValue = true;

            // Act
            object actualValue = converter.Convert(value, converter.TargetType);

            // Assert
            Assert.NotNull(actualValue);
            Assert.IsType<bool>(actualValue);
            Assert.Equal(expectedValue, (Boolean) actualValue);
        }

        [Fact]
        public void trueConversion()
        {
            // Arrange
            IConverter converter = new BooleanConverter();
            string value = "true";
            bool expectedValue = true;

            // Act
            object actualValue = converter.Convert(value, converter.TargetType);

            // Assert
            Assert.NotNull(actualValue);
            Assert.IsType<bool>(actualValue);
            Assert.Equal(expectedValue, (Boolean) actualValue);
        }

        [Fact]
        public void TrueConversion()
        {
            // Arrange
            IConverter converter = new BooleanConverter();
            string value = "True";
            bool expectedValue = true;

            // Act
            object actualValue = converter.Convert(value, converter.TargetType);

            // Assert
            Assert.NotNull(actualValue);
            Assert.IsType<bool>(actualValue);
            Assert.Equal(expectedValue, (Boolean) actualValue);
        }

        [Fact]
        public void falseConversion()
        {
            // Arrange
            IConverter converter = new BooleanConverter();
            string value = "false";
            bool expectedValue = false;

            // Act
            object actualValue = converter.Convert(value, converter.TargetType);

            // Assert
            Assert.NotNull(actualValue);
            Assert.IsType<bool>(actualValue);
            Assert.Equal(expectedValue, (Boolean) actualValue);
        }

        [Fact]
        public void FalseConversion()
        {
            // Arrange
            IConverter converter = new BooleanConverter();
            string value = "False";
            bool expectedValue = false;

            // Act
            object actualValue = converter.Convert(value, converter.TargetType);

            // Assert
            Assert.NotNull(actualValue);
            Assert.IsType<bool>(actualValue);
            Assert.Equal(expectedValue, (Boolean) actualValue);
        }

        [Fact]
        public void MinusConversion()
        {
            // Arrange
            IConverter converter = new BooleanConverter();
            string value = "-";
            bool expectedValue = false;

            // Act
            object actualValue = converter.Convert(value, converter.TargetType);

            // Assert
            Assert.NotNull(actualValue);
            Assert.IsType<bool>(actualValue);
            Assert.Equal(expectedValue, (Boolean) actualValue);
        }

        [Fact]
        public void PlusConversion()
        {
            // Arrange
            IConverter converter = new BooleanConverter();
            string value = "+";
            bool expectedValue = true;

            // Act
            object actualValue = converter.Convert(value, converter.TargetType);

            // Assert
            Assert.NotNull(actualValue);
            Assert.IsType<bool>(actualValue);
            Assert.Equal(expectedValue, (Boolean) actualValue);
        }

        [Fact]
        [Trait("Exception", "CommandLineParserException")]
        public void BadValueConversion()
        {
            // Arrange
            IConverter converter = new BooleanConverter();
            string value = "Hello";
            string expectedExceptionMessage = "Unable to parse 'Hello' to Boolean.";
            string expectedInnerExceptionMessage = "String was not recognized as a valid Boolean.";

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
    }
}