using MGR.CommandLineParser.UnitTests;
using Xunit;

namespace MGR.CommandLineParser.IntegrationTests.InvalidArguments;

public class InexistingShortOptionTests : ConsoleLoggingTestsBase
{
    [Fact]
    public async Task ParseWithInvalidArgs()
    {
        // Arrange
        IEnumerable<string> args = ["delete", "--source:custom value", "-pn", "ApiKey", "MyApiKey", "Custom argument value", "b"];
        var expected = @"There is no option 'pn' for the command 'Delete'.
";
        // Act
        var parsingResult = await CallParse(args);

        // Assert
        Assert.NotNull(parsingResult);
        Assert.Equal(CommandParsingResultCode.CommandParametersNotValid, parsingResult.ParsingResultCode);
        AssertOneMessageLoggedToConsole<FakeConsole.ErrorMessage>(expected);
    }
}