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
            IEnumerable<string> args = new List<string>();
            var expectedReturnCode = CommandParsingResultCode.NoCommandNameProvided;

            // Act
            var actual = CallParse(args);

            // Assert
            Assert.False(actual.IsValid);
            Assert.Equal(expectedReturnCode, actual.ParsingResultCode);
            Assert.Null(actual.CommandObject);
        }
    }
}