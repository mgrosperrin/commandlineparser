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
            IEnumerable<string> args = new[] {"NotValid", "-option:true"};
            var expectedReturnCode = CommandParsingResultCode.NoCommandFound;

            // Act
            var actual = CallParse(args);

            // Assert
            Assert.False(actual.IsValid);
            Assert.Equal(expectedReturnCode, actual.ParsingResultCode);
            Assert.Null(actual.CommandObject);
        }
    }
}