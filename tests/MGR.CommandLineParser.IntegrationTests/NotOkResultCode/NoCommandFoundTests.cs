using System.Collections.Generic;
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
            var expectedReturnCode = CommandResultCode.NoCommandFound;

            // Act
            var actual = parser.Parse(args);

            // Assert
            Assert.False(actual.IsValid);
            Assert.Equal(expectedReturnCode, actual.ReturnCode);
            Assert.Null(actual.Command);
        }
    }
}