using MGR.CommandLineParser.Extensibility.ClassBased;
using MGR.CommandLineParser.Tests.Commands;
using Xunit;

namespace MGR.CommandLineParser.IntegrationTests.UnspecifiedCommand;

public class SimpleOptionsTests : ConsoleLoggingTestsBase
{
    [Fact]
    public async Task ParseWithValidArgs()
    {
        // Arrange
        IEnumerable<string> args = ["delete", "--source:custom value", "-np", "--api-key", "MyApiKey", "Custom argument value", "b"];
        var expectedReturnCode = CommandParsingResultCode.Success;
        var expectedSource = "custom value";
        var expectedApiKey = "MyApiKey";
        var expectedNbOfArguments = 2;
        var expectedArgumentsValue = new List<string> { "Custom argument value", "b" };

        // Act
        var actual = await CallParse(args);

        // Assert
        Assert.True(actual.IsValid);
        Assert.Equal(expectedReturnCode, actual.ParsingResultCode);
        var classBasedCommandObject = Assert.IsAssignableFrom<IClassBasedCommandObject<DeleteCommand, DeleteCommand.DeleteCommandData>>(actual.CommandObject);
        var rawCommandData = classBasedCommandObject.CommandData;
        Assert.Equal(expectedSource, rawCommandData.Source);
        Assert.Equal(expectedApiKey, rawCommandData.ApiKey);
        Assert.True(rawCommandData.NoPrompt);
        Assert.Null(rawCommandData.SourceProvider);
        Assert.Null(rawCommandData.Settings);
        Assert.Equal(expectedNbOfArguments, rawCommandData.Arguments.Count);
        for (var i = 0; i < expectedNbOfArguments; i++)
        {
            Assert.Equal(expectedArgumentsValue[i], rawCommandData.Arguments[i]);
        }
    }
}