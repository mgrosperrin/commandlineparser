using System.Collections.Generic;
using System.Threading.Tasks;
using MGR.CommandLineParser.UnitTests;
using Xunit;

namespace MGR.CommandLineParser.IntegrationTests.InvalidArguments
{
    public class InvalidShortNameFormatTests : ConsoleLoggingTestsBase
    {
        [Fact]
        public async Task ParseWithInvalidShortOption()
        {
            // Arrange
            IEnumerable<string> args = new[] { "import", "--p:50" };
            var expected = @"There is no option 'p' for the command 'Import'.
";

            // Act
            var parsingResult = await CallParse(args);

            // Assert
            Assert.NotNull(parsingResult);
            Assert.Equal(CommandParsingResultCode.CommandParametersNotValid, parsingResult.ParsingResultCode);
            AssertOneMessageLoggedToConsole<FakeConsole.ErrorMessage>(expected);
        }
    }
}