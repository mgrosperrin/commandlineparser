using MGR.CommandLineParser.Extensibility.ClassBased;
using MGR.CommandLineParser.Tests.Commands;
using Xunit;

namespace MGR.CommandLineParser.IntegrationTests.NotOkResultCode;

public class CommandParameterNotValidTests : ConsoleLoggingTestsBase
{
    [Fact]
    public async Task ParseWithCommandNameAndInvalidArgs()
    {
        // Arrange
        IEnumerable<string> args = ["IntTest", "-i", "42", "Custom argument value", "-b"];
        var expectedReturnCode = CommandParsingResultCode.CommandParametersNotValid;
        var expectedNbOfArguments = 1;
        var expectedArgumentsValue = "Custom argument value";
        var expectedIntValue = 42;

        // Act
        var actual = await CallParse(args);

        // Assert
        Assert.False(actual.IsValid);
        Assert.Equal(expectedReturnCode, actual.ParsingResultCode);
        var classBasedCommandObject = Assert.IsAssignableFrom<IClassBasedCommandObject<IntTestCommand, IntTestCommand.IntTestCommandData>>(actual.CommandObject);
        var rawCommandData = classBasedCommandObject.CommandData;
        Assert.Equal(expectedIntValue, rawCommandData.IntValue);
        Assert.Null(rawCommandData.IntListValue);
        Assert.Equal(expectedNbOfArguments, rawCommandData.Arguments.Count);
        Assert.Equal(expectedArgumentsValue, rawCommandData.Arguments.Single());
        Assert.True(rawCommandData.BoolValue);
    }

    [Fact]
    public async Task ParseWithSpecifiedCommandAndInvalidArgs()
    {
        // Arrange
        IEnumerable<string> args = ["-i", "42", "Custom argument value", "-b"];
        var expectedReturnCode = CommandParsingResultCode.CommandParametersNotValid;
        var expectedNbOfArguments = 1;
        var expectedArgumentsValue = "Custom argument value";
        var expectedIntValue = 42;

        // Act
        var actual = await CallParse<IntTestCommand, IntTestCommand.IntTestCommandData>(args);

        // Assert
        Assert.False(actual.IsValid);
        Assert.Equal(expectedReturnCode, actual.ParsingResultCode);
        var classBasedCommandObject = Assert.IsAssignableFrom<IClassBasedCommandObject<IntTestCommand, IntTestCommand.IntTestCommandData>>(actual.CommandObject);
        var rawCommandData = classBasedCommandObject.CommandData;
        Assert.Equal(expectedIntValue, rawCommandData.IntValue);
        Assert.Null(rawCommandData.IntListValue);
        Assert.Equal(expectedNbOfArguments, rawCommandData.Arguments.Count);
        Assert.Equal(expectedArgumentsValue, rawCommandData.Arguments.Single());
        Assert.True(rawCommandData.BoolValue);
    }
}