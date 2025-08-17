using MGR.CommandLineParser.Extensibility.ClassBased;
using MGR.CommandLineParser.Tests.Commands;
using Xunit;

namespace MGR.CommandLineParser.IntegrationTests.SpecificCommand;
public class EnumTests : ConsoleLoggingTestsBase
{
    [Fact]
    public async Task ParseWithValidArgs()
    {
        // Arrange
        IEnumerable<string> args = ["--target:Assembly"];
        var expectedReturnCode = CommandParsingResultCode.Success;
        var expectedTargets = AttributeTargets.Assembly;

        // Act
        var actual = await CallParse<EnumCommand, EnumCommand.EnumCommandData>(args);

        // Assert
        Assert.True(actual.IsValid);
        Assert.Equal(expectedReturnCode, actual.ParsingResultCode);
        var classBasedCommandObject = Assert.IsAssignableFrom<IClassBasedCommandObject<EnumCommand, EnumCommand.EnumCommandData>>(actual.CommandObject);
        var rawCommandData = classBasedCommandObject.CommandData;
        Assert.Equal(expectedTargets, rawCommandData.Target);
    }
}
