using MGR.CommandLineParser.Extensibility.ClassBased;
using MGR.CommandLineParser.Tests.Commands;
using Xunit;

namespace MGR.CommandLineParser.IntegrationTests.UnspecifiedCommand;

public class CollectionArgumentWithArgumentsTests : ConsoleLoggingTestsBase
{
    [Fact]
    public async Task ParseWithValidListArgs()
    {
        // Arrange
        IEnumerable<string> args = ["IntTest", "--str-value:custom value", "-i", "42", "-il", "42", "Custom argument value", "-b"];
        var expectedReturnCode = CommandParsingResultCode.Success;
        var expectedStrValue = "custom value";
        var expectedNbOfArguments = 1;
        var expectedArgumentsValue = "Custom argument value";
        var expectedIntValue = 42;

        // Act
        var actual = await CallParse(args);

        // Assert
        Assert.True(actual.IsValid);
        Assert.Equal(expectedReturnCode, actual.ParsingResultCode);
        var classBasedCommandObject = Assert.IsAssignableFrom<IClassBasedCommandObject<IntTestCommand, IntTestCommand.IntTestCommandData>>(actual.CommandObject);
        var rawCommandData = classBasedCommandObject.CommandData;
        Assert.Equal(expectedStrValue, rawCommandData.StrValue);
        Assert.Equal(expectedIntValue, rawCommandData.IntValue);
        Assert.NotNull(rawCommandData.IntListValue);
        Assert.Equal(expectedNbOfArguments, rawCommandData.Arguments.Count);
        Assert.Equal(expectedArgumentsValue, rawCommandData.Arguments.Single());
        Assert.Equal(expectedNbOfArguments, rawCommandData.IntListValue.Count);
        Assert.Equal(expectedIntValue, rawCommandData.IntListValue.Single());
        Assert.True(rawCommandData.BoolValue);
    }
}