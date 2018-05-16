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
            var expectedReturnCode = CommandResultCode.NoArgs;

            // Act
            var actual = parser.Parse(null);

            // Assert
            Assert.False(actual.IsValid);
            Assert.Equal(expectedReturnCode, actual.ReturnCode);
            Assert.Null(actual.Command);
        }
    }
}