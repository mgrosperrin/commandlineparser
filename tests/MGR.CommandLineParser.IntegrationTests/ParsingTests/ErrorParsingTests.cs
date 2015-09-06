using System.Collections.Generic;
using MGR.CommandLineParser.Command;
using Xunit;

namespace MGR.CommandLineParser.IntegrationTests.ParsingTests
{
    public class ErrorParsingTests
    {
        [Fact]
        public void ParseWithoutParameter()
        {
            // Arrange
            var parserBuild = new ParserBuilder();
            var parser = parserBuild.BuildParser();
            IEnumerable<string> args = null;
            var expectedReturnCode = CommandResultCode.NoArgs;

            // Act
            var actual = parser.Parse(args);

            // Assert
            Assert.False(actual.IsValid);
            Assert.Equal(expectedReturnCode, actual.ReturnCode);
            Assert.Null(actual.Command);
        }

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

        [Fact]
        public void ParseWithBadCommandName()
        {
            // Arrange
            var parserBuild = new ParserBuilder();
            var parser = parserBuild.BuildParser();
            IEnumerable<string> args = new[] { "NotValid", "-option:true" };
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