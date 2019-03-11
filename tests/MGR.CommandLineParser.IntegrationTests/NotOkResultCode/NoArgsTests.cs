using Xunit;

namespace MGR.CommandLineParser.IntegrationTests.NotOkResultCode
{
    public class NoArgsTests : ConsoleLoggingTestsBase
    {
        [Fact]
        public void ParseWithoutParameter()
        {
            // Arrange
            var expectedReturnCode = CommandParsingResultCode.NoArgumentsProvided;

            // Act
            var actual = CallParse(null);

            // Assert
            Assert.False(actual.IsValid);
            Assert.Equal(expectedReturnCode, actual.ParsingResultCode);
            Assert.Null(actual.CommandObject);
        }
    }
}