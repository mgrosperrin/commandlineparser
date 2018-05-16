using System.Collections.Generic;
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
            var expectedReturnCode = CommandResultCode.Ok;
            var expectedSource = "DefaultSource";
            var expectedNbOfArguments = 0;

            // Act
            var actual = parser.Parse(args);

            // Assert
            Assert.True(actual.IsValid);
            Assert.Equal(expectedReturnCode, actual.ReturnCode);
            Assert.IsType<SetApiKeyCommand>(actual.Command);
            Assert.Equal(expectedSource, ((SetApiKeyCommand) actual.Command).Source);
            Assert.Equal(expectedNbOfArguments, ((SetApiKeyCommand) actual.Command).Arguments.Count);
        }
    }
}