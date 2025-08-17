using MGR.CommandLineParser.Extensibility.Converters;
using Xunit;

namespace MGR.CommandLineParser.UnitTests.Extensibility.Converters;

public class Int64ConverterTests
{
    [Fact]
    public void TargetType()
    {
        // Arrange
        IConverter converter = new Int64Converter();
        var expectedType = typeof(long);

        // Act
        var actualType = converter.TargetType;

        // Assert
        Assert.Equal(expectedType, actualType);
    }

    [Fact]
    public void Conversion()
    {
        // Arrange
        IConverter converter = new Int64Converter();
        var value = "42";
        long expectedValue = 42;

        // Act
        var actualValue = converter.Convert(value, converter.TargetType);

        // Assert
        Assert.NotNull(actualValue);
        Assert.IsType<long>(actualValue);
        Assert.Equal(expectedValue, (long)actualValue);
    }

    [Fact]
    public void BadValueConversion()
    {
        // Arrange
        IConverter converter = new Int64Converter();
        var value = "Hello";
        var expectedExceptionMessage = Constants.ExceptionMessages.FormatConverterUnableConvert(value, typeof(long));
        var expectedInnerExceptionMessage =
#if NETFRAMEWORK
            "Input string was not in a correct format."
#else
            "The input string 'Hello' was not in a correct format."
#endif
            ;

        // Act
        using (new LangageSwitcher("en-us"))
        {
            var actualException =
                Assert.Throws<CommandLineParserException>(() => converter.Convert(value, converter.TargetType));

            // Assert
            Assert.Equal(expectedExceptionMessage, actualException.Message);
            Assert.NotNull(actualException.InnerException);
            var actualInnerExecption = Assert.IsAssignableFrom<FormatException>(actualException.InnerException);
            Assert.Equal(expectedInnerExceptionMessage, actualInnerExecption.Message);
        }
    }
}