using MGR.CommandLineParser.Extensibility.ClassBased;
using MGR.CommandLineParser.Tests.Commands;
using Xunit;

namespace MGR.CommandLineParser.IntegrationTests.SpecificCommand;

public class EscapedTraillingArgumentTests : ConsoleLoggingTestsBase
{
    [Fact]
    public async Task ParseWithValidArgsAnDoubleDash()
    {
        // Arrange
        IEnumerable<string> args = ["--str-value:custom value", "-i", "42", "Custom argument value", "-b", "--", "firstArg", "-i", "32"];
        var expectedReturnCode = CommandParsingResultCode.Success;
        var expectedStrValue = "custom value";
        var expectedNbOfArguments = 4;
        var expectedArgumentsValue = "Custom argument value";
        var expectedIntValue = 42;

        // Act
        var actual = await CallParse<IntTestCommand, IntTestCommand.IntTestCommandData>(args);

        // Assert
        Assert.True(actual.IsValid);
        Assert.Equal(expectedReturnCode, actual.ParsingResultCode);
        var classBasedCommandObject = Assert.IsAssignableFrom<IClassBasedCommandObject<IntTestCommand, IntTestCommand.IntTestCommandData>>(actual.CommandObject);
        var rawCommandData = classBasedCommandObject.CommandData;
        Assert.Equal(expectedStrValue, rawCommandData.StrValue);
        Assert.Equal(expectedIntValue, rawCommandData.IntValue);
        Assert.Null(rawCommandData.IntListValue);
        Assert.Equal(expectedNbOfArguments, rawCommandData.Arguments.Count);
        Assert.Equal(new List<string> { expectedArgumentsValue, "firstArg", "-i", "32" }, rawCommandData.Arguments);
        Assert.True(rawCommandData.BoolValue);
    }
}