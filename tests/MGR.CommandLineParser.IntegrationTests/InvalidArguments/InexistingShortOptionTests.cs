using System.Collections.Generic;
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
            IEnumerable<string> args = new[] {"delete", "--Source:custom value", "-pn", "ApiKey", "MyApiKey", "Custom argument value", "b"};
            var expectedMessageException = @"There is no option 'pn' for the command 'Delete'.";

            // Act
            var actual = Assert.Throws<CommandLineParserException>(() => parser.Parse(args));

            // Assert
            Assert.Equal(expectedMessageException, actual.Message);
        }
    }
}