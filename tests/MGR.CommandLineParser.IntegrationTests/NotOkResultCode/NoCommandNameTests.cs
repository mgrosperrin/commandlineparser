using System.Collections.Generic;
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
            var expectedReturnCode = CommandResultCode.NoCommandName;

            // Act
            var actual = parser.Parse(args);

            // Assert
            Assert.False(actual.IsValid);
            Assert.Equal(expectedReturnCode, actual.ReturnCode);
            Assert.Null(actual.Command);
        }
    }
}