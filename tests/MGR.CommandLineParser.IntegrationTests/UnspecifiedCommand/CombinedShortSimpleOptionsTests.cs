using MGR.CommandLineParser.Extensibility.ClassBased;
using MGR.CommandLineParser.Tests.Commands;
using Xunit;

namespace MGR.CommandLineParser.IntegrationTests.UnspecifiedCommand;

public class CombinedShortSimpleOptionsTests : ConsoleLoggingTestsBase
{
    [Fact]
    public async Task ParseWithValidArgs()
    {
        // Arrange
        IEnumerable<string> args = ["pack", "--version:abc", "-vt"];
        var expectedReturnCode = CommandParsingResultCode.Success;
        var expectedVersion = "abc";

        // Act
        var actual = await CallParse(args);

        // Assert
        Assert.True(actual.IsValid);
        Assert.Equal(expectedReturnCode, actual.ParsingResultCode);
        Assert.IsAssignableFrom<IClassBasedCommandObject<PackCommand, PackCommand.PackCommandData>>(actual.CommandObject);
        Assert.IsType<PackCommand>(((IClassBasedCommandObject<PackCommand, PackCommand.PackCommandData>)actual.CommandObject).Command);
        var rawCommandData = ((IClassBasedCommandObject<PackCommand, PackCommand.PackCommandData>)actual.CommandObject).CommandData;
        Assert.True(rawCommandData.Verbose);
        Assert.True(rawCommandData.Tool);
        Assert.False(rawCommandData.Build);
        Assert.Equal(expectedVersion, rawCommandData.Version);
    }
    [Fact]
    public async Task ParseWithValidArgsWithFalse()
    {
        // Arrange
        IEnumerable<string> args = ["pack", "--version:abc", "-vt:-", "-b"];
        var expectedReturnCode = CommandParsingResultCode.Success;
        var expectedVersion = "abc";

        // Act
        var actual = await CallParse(args);

        // Assert
        Assert.True(actual.IsValid);
        Assert.Equal(expectedReturnCode, actual.ParsingResultCode);
        var classBasedCommandObject = Assert.IsAssignableFrom<IClassBasedCommandObject<PackCommand, PackCommand.PackCommandData>>(actual.CommandObject);
        var rawCommandData = classBasedCommandObject.CommandData;
        Assert.False(rawCommandData.Verbose);
        Assert.False(rawCommandData.Tool);
        Assert.True(rawCommandData.Build);
        Assert.Equal(expectedVersion, rawCommandData.Version);
    }
}