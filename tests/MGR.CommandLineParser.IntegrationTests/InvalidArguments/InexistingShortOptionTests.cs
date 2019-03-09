using System.Collections.Generic;
using MGR.CommandLineParser.UnitTests;
using Xunit;

namespace MGR.CommandLineParser.IntegrationTests.InvalidArguments
{
    public class InexistingShortOptionTests : ConsoleLoggingTestsBase
    {
        [Fact]
        public void ParseWithInvalidArgs()
        {
            // Arrange
            var parserBuild = new ParserBuilder();
            var parser = parserBuild.BuildParser();
            IEnumerable<string> args = new[] {"delete", "--source:custom value", "-pn", "ApiKey", "MyApiKey", "Custom argument value", "b"};
            var expectedMessage = @"There is no option 'pn' for the command 'Delete'.";

            // Act
            var parsingResult = parser.Parse(args, CreateServiceProvider());

            // Assert
            Assert.NotNull(parsingResult);
            Assert.Equal(CommandParsingResultCode.CommandParametersNotValid, parsingResult.ParsingResultCode);
            var actualHelp = StringConsole.Current.ErrorAsString();
            Assert.Equal(expectedMessage, actualHelp);
        }
    }
}