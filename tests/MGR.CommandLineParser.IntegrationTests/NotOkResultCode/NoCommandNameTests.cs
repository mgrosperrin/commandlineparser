using System.Collections.Generic;
using MGR.CommandLineParser.Command;
using Xunit;

namespace MGR.CommandLineParser.IntegrationTests.NotOkResultCode
{
    public class NoCommandNameTests : ConsoleLoggingTestsBase
    {
        [Fact]
        public void ParseWithEmptyParameter()
        {
            // Arrange
            var parserBuild = new ParserBuilder();
            var parser = parserBuild.BuildParser();
            IEnumerable<string> args = new List<string>();
            var expectedReturnCode = CommandParsingResultCode.NoCommandNameProvided;

            // Act
            var actual = parser.Parse(args);

            // Assert
            Assert.False(actual.IsValid);
            Assert.Equal(expectedReturnCode, actual.ParsingResultCode);
            Assert.Null(actual.CommandObject);
        }
    }
}