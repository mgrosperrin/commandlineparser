using System.Collections.Generic;
using MGR.CommandLineParser.Command;
using Xunit;

namespace MGR.CommandLineParser.IntegrationTests.NotOkResultCode
{
    public class NoCommandFoundTests : ConsoleLoggingTestsBase
    {
        [Fact]
        public void ParseWithBadCommandName()
        {
            // Arrange
            var parserBuild = new ParserBuilder();
            var parser = parserBuild.BuildParser();
            IEnumerable<string> args = new[] {"NotValid", "-option:true"};
            var expectedReturnCode = CommandParsingResultCode.NoCommandFound;

            // Act
            var actual = parser.Parse(args);

            // Assert
            Assert.False(actual.IsValid);
            Assert.Equal(expectedReturnCode, actual.ParsingResultCode);
            Assert.Null(actual.CommandObject);
        }
    }
}