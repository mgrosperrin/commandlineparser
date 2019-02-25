using System.Collections.Generic;
using MGR.CommandLineParser.Extensibility.ClassBased;
using MGR.CommandLineParser.Tests.Commands;
using Xunit;

namespace MGR.CommandLineParser.IntegrationTests.UnspecifiedCommand
{
    public class SimpleOptionsTests : ConsoleLoggingTestsBase
    {
        [Fact]
        public void ParseWithValidArgs()
        {
            // Arrange
            var parserBuild = new ParserBuilder();
            var parser = parserBuild.BuildParser();
            IEnumerable<string> args = new[] {"delete", "--source:custom value", "-np", "--api-key", "MyApiKey", "Custom argument value", "b"};
            var expectedReturnCode = CommandParsingResultCode.Success;
            var expectedSource = "custom value";
            var expectedApiKey = "MyApiKey";
            var expectedNbOfArguments = 2;
            var expectedArgumentsValue = new List<string> {"Custom argument value", "b"};

            // Act
            var actual = parser.Parse(args);

            // Assert
            Assert.True(actual.IsValid);
            Assert.Equal(expectedReturnCode, actual.ParsingResultCode);
            Assert.IsAssignableFrom<IClassBasedCommandObject>(actual.CommandObject);
            Assert.IsType<DeleteCommand>(((IClassBasedCommandObject)actual.CommandObject).Command);
            var rawCommand = (DeleteCommand)((IClassBasedCommandObject)actual.CommandObject).Command;
            Assert.Equal(expectedSource, rawCommand.Source);
            Assert.Equal(expectedApiKey, rawCommand.ApiKey);
            Assert.True(rawCommand.NoPrompt);
            Assert.Null(rawCommand.SourceProvider);
            Assert.Null(rawCommand.Settings);
            Assert.Equal(expectedNbOfArguments, rawCommand.Arguments.Count);
            for (var i = 0; i < expectedNbOfArguments; i++)
            {
                Assert.Equal(expectedArgumentsValue[i], ((IClassBasedCommandObject)actual.CommandObject).Command.Arguments[i]);
            }
        }
    }
}