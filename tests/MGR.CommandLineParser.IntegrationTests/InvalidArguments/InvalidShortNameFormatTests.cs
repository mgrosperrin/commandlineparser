using System.Collections.Generic;
using MGR.CommandLineParser.UnitTests;
using Xunit;

namespace MGR.CommandLineParser.IntegrationTests.InvalidArguments
{
    public class InvalidShortNameFormatTests : ConsoleLoggingTestsBase
    {
        [Fact]
        public void ParseWithInvalidShortOption()
        {
            // Arrange
            var parserBuild = new ParserBuilder();
            var parser = parserBuild.BuildParser();
            IEnumerable<string> args = new[] { "import", "--p:50" };
            var expectedMessage = "There is no option 'p' for the command 'Import'.";

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