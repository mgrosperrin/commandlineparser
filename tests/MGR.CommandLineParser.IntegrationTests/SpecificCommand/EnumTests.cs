using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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
        IEnumerable<string> args = new[] { "--target:Assembly" };
        var expectedReturnCode = CommandParsingResultCode.Success;
        var expectedTargets = AttributeTargets.Assembly;

        // Act
        var actual = await CallParse<EnumCommand>(args);

        // Assert
        Assert.True(actual.IsValid);
        Assert.Equal(expectedReturnCode, actual.ParsingResultCode);
        var classBasedCommandObject = Assert.IsAssignableFrom<IClassBasedCommandObject>(actual.CommandObject);
        var rawCommand = Assert.IsType<EnumCommand>(classBasedCommandObject.Command);
        Assert.Equal(expectedTargets, rawCommand.Target);
    }
}
