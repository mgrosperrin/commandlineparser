using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MGR.CommandLineParser.Command;
using MGR.CommandLineParser.Extensibility.ClassBased;
using Xunit;

namespace MGR.CommandLineParser.IntegrationTests.SpecificCommand;
public class InternalTypesTests : ConsoleLoggingTestsBase
{
    [Fact]
    public async Task ParseInternalCommand()
    {
        // Arrange
        IEnumerable<string> args = new[] { "" };
        var expectedReturnCode = CommandParsingResultCode.Success;

        // Act
        var actual = await CallParseWithDefaultCommand<Test.InternalCommand>(args);

        // Assert
        Assert.True(actual.IsValid);
        Assert.Equal(expectedReturnCode, actual.ParsingResultCode);
        var classBasedCommandObject = Assert.IsAssignableFrom<IClassBasedCommandObject>(actual.CommandObject);
        var rawCommand = Assert.IsType<Test.InternalCommand>(classBasedCommandObject.Command);
        Assert.Equal(123, await rawCommand.ExecuteAsync());
    }
}
internal class Test
{
    internal class InternalCommand : CommandBase
    {
        public InternalCommand(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        protected override Task<int> ExecuteCommandAsync() => Task.FromResult(123);
    }
}