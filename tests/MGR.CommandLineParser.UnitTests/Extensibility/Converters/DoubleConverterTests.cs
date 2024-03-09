using MGR.CommandLineParser.Extensibility.Converters;
using Xunit;

namespace MGR.CommandLineParser.UnitTests.Extensibility.Converters;

public class DoubleConverterTests
{
    [Fact]
    public void TargetType()
    {
        // Arrange
        IConverter converter = new DoubleConverter();
        var expectedType = typeof(double);

        // Act
        var actualType = converter.TargetType;

        // Assert
        Assert.Equal(expectedType, actualType);
    }

    [Fact]
    public void Conversion()
    {
        // Arrange
        IConverter converter = new DoubleConverter();
        var value = "42,35";
        var expectedValue = 42.35;

        // Act
        using (new LangageSwitcher("fr-fr"))
        {
            var actualValue = converter.Convert(value, converter.TargetType);

            // Assert
            Assert.NotNull(actualValue);
            Assert.IsType<double>(actualValue);
            Assert.Equal(expectedValue, (double)actualValue);
        }
    }

    [Fact]
    public void BadValueConversion()
    {
        // Arrange
        IConverter converter = new DoubleConverter();
        var value = "Hello";
        var expectedExceptionMessage = Constants.ExceptionMessages.FormatConverterUnableConvert(value, typeof(double));
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
            var actualException = Assert.Throws<CommandLineParserException>(() => converter.Convert(value, converter.TargetType));

            // Assert
            Assert.Equal(expectedExceptionMessage, actualException.Message);
            Assert.NotNull(actualException.InnerException);
            var actualInnerExecption = Assert.IsAssignableFrom<FormatException>(actualException.InnerException);
            Assert.Equal(expectedInnerExceptionMessage, actualInnerExecption.Message);
        }
    }
}