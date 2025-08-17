using MGR.CommandLineParser.Extensibility.ClassBased;
using MGR.CommandLineParser.Tests.Commands;
using Xunit;

namespace MGR.CommandLineParser.IntegrationTests.UnspecifiedCommand;

public class OptionsHasDefaultValueTests : ConsoleLoggingTestsBase
{
    [Fact]
    public async Task ParseWithValidArgsAndDefaultValue()
    {
        // Arrange
        IEnumerable<string> args = ["SetApiKey"];
        var expectedReturnCode = CommandParsingResultCode.Success;
        var expectedSource = "DefaultSource";
        var expectedNbOfArguments = 0;

        // Act
        var actual = await CallParse(args);

        // Assert
        Assert.True(actual.IsValid);
        Assert.Equal(expectedReturnCode, actual.ParsingResultCode);
        var classBasedCommandObject = Assert.IsAssignableFrom<IClassBasedCommandObject<SetApiKeyCommand, SetApiKeyCommand.SetApiKeyCommandData>>(actual.CommandObject);
        var rawCommandData = classBasedCommandObject.CommandData;
        Assert.Equal(expectedSource, rawCommandData.Source);
        Assert.Equal(expectedNbOfArguments, rawCommandData.Arguments.Count);
    }
}