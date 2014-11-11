using System;
using MGR.CommandLineParser.Converters;
using Xunit;

namespace MGR.CommandLineParser.UnitTests.Converters
{
    public class EnumConverterTests
    {
        [Fact]
        public void TargetType()
        {
            // Arrange
            IConverter converter = new EnumConverter();
            Type expectedType = typeof (Enum);

            // Act
            Type actualType = converter.TargetType;

            // Assert
            Assert.Equal(expectedType, actualType);
        }

        [Fact]
        public void ParseSimpleEnumValue()
        {
            // Arrange
            IConverter converter = new EnumConverter();
            string value = "1";
            var expectedValue = AttributeTargets.Assembly;

            // Act
            object actualValue = converter.Convert(value, typeof (AttributeTargets));

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
            string value = "Assembly";
            var expectedValue = AttributeTargets.Assembly;

            // Act
            object actualValue = converter.Convert(value, typeof (AttributeTargets));

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
            string value = "Assembly, Class";
            AttributeTargets expectedValue = AttributeTargets.Assembly | AttributeTargets.Class;

            // Act
            object actualValue = converter.Convert(value, typeof (AttributeTargets));

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
            string value = "10";
            string expectedExceptionMessage = "The specified value '10' is not correct the type 'ConsoleModifiers'.";

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
            string value = "Hello";
            string expectedExceptionMessage = "Unable to parse 'Hello' to Enum.";
            string expectedInnerExceptionMessage = "enumType";

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
            string value = "Hello";
            string expectedExceptionMessage = @"concreteTargetType";

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
            string value = "Hello";
            string expectedExceptionMessage = "The specified concrete target type (Exception) is not an enum type.";

            // Act
            var actualException = Assert.Throws<CommandLineParserException>(() => converter.Convert(value, typeof (Exception)));

            // Assert
            Assert.Equal(expectedExceptionMessage, actualException.Message);
        }
    }
}