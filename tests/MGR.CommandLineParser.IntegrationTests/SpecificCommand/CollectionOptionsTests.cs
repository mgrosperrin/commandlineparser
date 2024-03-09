using MGR.CommandLineParser.Extensibility.ClassBased;
using MGR.CommandLineParser.Tests.Commands;
using Xunit;

namespace MGR.CommandLineParser.IntegrationTests.SpecificCommand;

public class CollectionOptionsTests : ConsoleLoggingTestsBase
{
    [Fact]
    public async Task ParseWithValidListArgs()
    {
        // Arrange
        IEnumerable<string> args = ["--str-value:custom value", "-i", "42", "-il", "42", "Custom argument value", "-b"];
        var expectedReturnCode = CommandParsingResultCode.Success;
        var expectedStrValue = "custom value";
        var expectedNbOfArguments = 1;
        var expectedArgumentsValue = "Custom argument value";
        var expectedIntValue = 42;

        // Act
        var actual = await CallParse<IntTestCommand, IntTestCommand.IntTestCommandData>(args);

        // Assert
        Assert.True(actual.IsValid);
        Assert.Equal(expectedReturnCode, actual.ParsingResultCode);
        var classBasedCommandObject = Assert.IsAssignableFrom<IClassBasedCommandObject<IntTestCommand, IntTestCommand.IntTestCommandData>>(actual.CommandObject);
        var rawCommand = classBasedCommandObject.CommandData;
        Assert.Equal(expectedStrValue, rawCommand.StrValue);
        Assert.Equal(expectedIntValue, rawCommand.IntValue);
        Assert.NotNull(rawCommand.IntListValue);
        Assert.Equal(expectedNbOfArguments, rawCommand.Arguments.Count);
        Assert.Equal(expectedArgumentsValue, rawCommand.Arguments.Single());
        Assert.Equal(expectedNbOfArguments, rawCommand.IntListValue.Count);
        Assert.Equal(expectedIntValue, rawCommand.IntListValue.Single());
        Assert.True(rawCommand.BoolValue);
    }
}