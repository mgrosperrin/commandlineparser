using Xunit;

namespace MGR.CommandLineParser.IntegrationTests.NotOkResultCode
{
    public class NoArgsTests : ConsoleLoggingTestsBase
    {
        [Fact]
        public void ParseWithoutParameter()
        {
            // Arrange
            var parserBuild = new ParserBuilder();
            var parser = parserBuild.BuildParser();
            var expectedReturnCode = CommandParsingResultCode.NoArgumentsProvided;

            // Act
            var actual = parser.Parse(null);

            // Assert
            Assert.False(actual.IsValid);
            Assert.Equal(expectedReturnCode, actual.ParsingResultCode);
            Assert.Null(actual.Command);
        }
    }
}