using System.Collections.Generic;
using MGR.CommandLineParser.Extensibility.ClassBased;
using MGR.CommandLineParser.Tests.Commands;
using Xunit;

namespace MGR.CommandLineParser.IntegrationTests.UnspecifiedCommand
{
    public class OptionsHasDefaultValueTests : ConsoleLoggingTestsBase
    {
        [Fact]
        public void ParseWithValidArgsAndDefaultValue()
        {
            // Arrange
            var parserBuild = new ParserBuilder();
            var parser = parserBuild.BuildParser();
            IEnumerable<string> args = new[]
                {"SetApiKey"};
            var expectedReturnCode = CommandParsingResultCode.Success;
            var expectedSource = "DefaultSource";
            var expectedNbOfArguments = 0;

            // Act
            var actual = parser.Parse(args);

            // Assert
            Assert.True(actual.IsValid);
            Assert.Equal(expectedReturnCode, actual.ParsingResultCode);
            Assert.IsAssignableFrom<IClassBasedCommandObject>(actual.CommandObject);
            Assert.IsType<SetApiKeyCommand>(((IClassBasedCommandObject)actual.CommandObject).Command);
            var rawCommand = (SetApiKeyCommand)((IClassBasedCommandObject)actual.CommandObject).Command;
            Assert.Equal(expectedSource, rawCommand.Source);
            Assert.Equal(expectedNbOfArguments, rawCommand.Arguments.Count);
        }
    }
}