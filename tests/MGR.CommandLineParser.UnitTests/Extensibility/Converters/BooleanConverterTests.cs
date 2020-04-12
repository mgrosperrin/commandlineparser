using System;
using MGR.CommandLineParser.Extensibility.Converters;
using Xunit;

namespace MGR.CommandLineParser.UnitTests.Extensibility.Converters
{
    public class BooleanConverterTests
    {
        [Fact]
        public void TargetType()
        {
            // Arrange
            IConverter converter = new BooleanConverter();
            var expectedType = typeof(bool);

            // Act
            var actual = converter.TargetType;

            // Assert
            Assert.Equal(expectedType, actual);
        }

        [Fact]
        public void StringEmptyConversion()
        {
            // Arrange
            IConverter converter = new BooleanConverter();
            var value = string.Empty;

            // Act
            var actual = converter.Convert(value, converter.TargetType);

            // Assert
            Assert.NotNull(actual);
            Assert.IsType<bool>(actual);
            Assert.True((bool)actual);
        }

        [Fact]
        public void LowerTrueConversion()
        {
            // Arrange
            IConverter converter = new BooleanConverter();
            var value = "true";

            // Act
            var actual = converter.Convert(value, converter.TargetType);

            // Assert
            Assert.NotNull(actual);
            Assert.IsType<bool>(actual);
            Assert.True((bool)actual);
        }

        [Fact]
        public void UpperTrueConversion()
        {
            // Arrange
            IConverter converter = new BooleanConverter();
            var value = "True";

            // Act
            var actual = converter.Convert(value, converter.TargetType);

            // Assert
            Assert.NotNull(actual);
            Assert.IsType<bool>(actual);
            Assert.True((bool)actual);
        }

        [Fact]
        public void LowerFalseConversion()
        {
            // Arrange
            IConverter converter = new BooleanConverter();
            var value = "false";

            // Act
            var actual = converter.Convert(value, converter.TargetType);

            // Assert
            Assert.NotNull(actual);
            Assert.IsType<bool>(actual);
            Assert.False((bool)actual);
        }

        [Fact]
        public void UpperFalseConversion()
        {
            // Arrange
            IConverter converter = new BooleanConverter();
            var value = "False";

            // Act
            var actual = converter.Convert(value, converter.TargetType);

            // Assert
            Assert.NotNull(actual);
            Assert.IsType<bool>(actual);
            Assert.False((bool)actual);
        }

        [Fact]
        public void MinusConversion()
        {
            // Arrange
            IConverter converter = new BooleanConverter();
            var value = "-";

            // Act
            var actual = converter.Convert(value, converter.TargetType);

            // Assert
            Assert.NotNull(actual);
            Assert.IsType<bool>(actual);
            Assert.False((bool)actual);
        }

        [Fact]
        public void PlusConversion()
        {
            // Arrange
            IConverter converter = new BooleanConverter();
            var value = "+";

            // Act
            var actual = converter.Convert(value, converter.TargetType);

            // Assert
            Assert.NotNull(actual);
            Assert.IsType<bool>(actual);
            Assert.True((bool)actual);
        }

        [Fact]
        [Trait(nameof(Exception), nameof(CommandLineParserException))]
        public void BadValueConversion()
        {
            // Arrange
            IConverter converter = new BooleanConverter();
            var value = "Hello";
            var expectedExceptionMessage = Constants.ExceptionMessages.FormatConverterUnableConvert(value, typeof(bool));
#if NET471
            var expectedInnerExceptionMessage = "String was not recognized as a valid Boolean.";
#else
            var expectedInnerExceptionMessage = "String 'Hello' was not recognized as a valid Boolean.";
#endif

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