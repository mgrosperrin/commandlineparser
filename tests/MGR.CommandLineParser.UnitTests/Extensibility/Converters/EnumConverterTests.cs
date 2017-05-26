using System;
using MGR.CommandLineParser.Extensibility.Converters;
using Xunit;

namespace MGR.CommandLineParser.UnitTests.Extensibility.Converters
{
    public class EnumConverterTests
    {
        [Fact]
        public void TargetType()
        {
            // Arrange
            IConverter converter = new EnumConverter();
            var expectedType = typeof (Enum);

            // Act
            var actualType = converter.TargetType;

            // Assert
            Assert.Equal(expectedType, actualType);
        }

        [Fact]
        public void ParseSimpleEnumValue()
        {
            // Arrange
            IConverter converter = new EnumConverter();
            var value = "1";
            var expectedValue = AttributeTargets.Assembly;

            // Act
            var actualValue = converter.Convert(value, typeof (AttributeTargets));

            // Assert
            Assert.NotNull(actualValue);
            Assert.IsType<AttributeTargets>(actualValue);
            Assert.Equal(expectedValue, (AttributeTargets) actualValue);
        }

        [Fact]
        public void ParseSimpleEnumNameValue()
        {
            // Arrange
            IConverter converter = new EnumConverter();
            var value = "Assembly";
            var expectedValue = AttributeTargets.Assembly;

            // Act
            var actualValue = converter.Convert(value, typeof (AttributeTargets));

            // Assert
            Assert.NotNull(actualValue);
            Assert.IsType<AttributeTargets>(actualValue);
            Assert.Equal(expectedValue, (AttributeTargets) actualValue);
        }

        [Fact]
        public void ParseMultipleEnumNameValue()
        {
            // Arrange
            IConverter converter = new EnumConverter();
            var value = "Assembly, Class";
            var expectedValue = AttributeTargets.Assembly | AttributeTargets.Class;

            // Act
            var actualValue = converter.Convert(value, typeof (AttributeTargets));

            // Assert
            Assert.NotNull(actualValue);
            Assert.IsType<AttributeTargets>(actualValue);
            Assert.Equal(expectedValue, (AttributeTargets) actualValue);
        }

        [Fact]
        public void BadValueException()
        {
            // Arrange
            IConverter converter = new EnumConverter();
            var value = "10";
            var expectedExceptionMessage = "The specified value '10' is not correct the type 'System.ConsoleModifiers'.";

            // Act
            var actualException = Assert.Throws<CommandLineParserException>(() => converter.Convert(value, typeof (ConsoleModifiers)));

            // Assert
            Assert.Equal(expectedExceptionMessage, actualException.Message);
        }

        [Fact]
        public void BadNameValueException()
        {
            // Arrange
            IConverter converter = new EnumConverter();
            var value = "Hello";
            var expectedExceptionMessage = Constants.ExceptionMessages.FormatConverterUnableConvert(value, typeof(Enum));
            var expectedInnerExceptionMessage = "enumType";

            // Act
            var actualException = Assert.Throws<CommandLineParserException>(() => converter.Convert(value, converter.TargetType));

            // Assert
            Assert.Equal(expectedExceptionMessage, actualException.Message);
            Assert.NotNull(actualException.InnerException);
            var actualInnerExecption = Assert.IsAssignableFrom<ArgumentException>(actualException.InnerException);
            Assert.Equal(expectedInnerExceptionMessage, actualInnerExecption.ParamName);
        }

        [Fact]
        public void NullConcreteTargetTypeException()
        {
            // Arrange
            IConverter converter = new EnumConverter();
            var value = "Hello";
            var expectedExceptionMessage = @"concreteTargetType";

            // Act
            var actualException = Assert.Throws<ArgumentNullException>(() => converter.Convert(value, null));

            // Assert
            Assert.Equal(expectedExceptionMessage, actualException.ParamName);
        }

        [Fact]
        public void NotEnumConcreteTargetTypeException()
        {
            // Arrange
            IConverter converter = new EnumConverter();
            var value = "Hello";
            var expectedExceptionMessage = "The specified concrete target type (System.Exception) is not an enum type.";

            // Act
            var actualException = Assert.Throws<CommandLineParserException>(() => converter.Convert(value, typeof (Exception)));

            // Assert
            Assert.Equal(expectedExceptionMessage, actualException.Message);
        }
    }
}