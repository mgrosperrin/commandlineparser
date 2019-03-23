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
            IEnumerable<string> args = new[] { "delete", "--source:custom value", "-pn", "ApiKey", "MyApiKey", "Custom argument value", "b" };
            var expected = @"There is no option 'pn' for the command 'Delete'.
";
            // Act
            var parsingResult = CallParse(args);

            // Assert
            Assert.NotNull(parsingResult);
            Assert.Equal(CommandParsingResultCode.CommandParametersNotValid, parsingResult.ParsingResultCode);
            AssertOneMessageLoggedToConsole<FakeConsole.ErrorMessage>(expected);
        }
    }
}