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
        IEnumerable<string> args = [""];
        var expectedReturnCode = CommandParsingResultCode.Success;

        // Act
        var actual = await CallParseWithDefaultCommand<Test.InternalCommand, Test.InternalCommand.InternalCommandData>(args);

        // Assert
        Assert.True(actual.IsValid);
        Assert.Equal(expectedReturnCode, actual.ParsingResultCode);
        var classBasedCommandObject = Assert.IsAssignableFrom<IClassBasedCommandObject<Test.InternalCommand, Test.InternalCommand.InternalCommandData>>(actual.CommandObject);
        var rawCommand = classBasedCommandObject.Command;
        Assert.Equal(123, await rawCommand.ExecuteAsync(classBasedCommandObject.CommandData, TestContext.Current.CancellationToken));
    }
}
internal class Test
{
    internal class InternalCommand : CommandBase<Test.InternalCommand.InternalCommandData>
    {
        internal class InternalCommandData : HelpedCommandData
        {
        }
        public InternalCommand(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        protected override Task<int> ExecuteCommandAsync(InternalCommandData commandData, CancellationToken cancellationToken) => Task.FromResult(123);
    }
}