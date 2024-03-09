using Xunit;

namespace MGR.CommandLineParser.IntegrationTests.NotOkResultCode;

public class NoCommandFoundTests : ConsoleLoggingTestsBase
{
    [Fact]
    public async Task ParseWithBadCommandName()
    {
        // Arrange
        IEnumerable<string> args = ["NotValid", "-option:true"];
        var expectedReturnCode = CommandParsingResultCode.NoCommandFound;

        // Act
        var actual = await CallParse(args);

        // Assert
        Assert.False(actual.IsValid);
        Assert.Equal(expectedReturnCode, actual.ParsingResultCode);
        Assert.Null(actual.CommandObject);
    }
}