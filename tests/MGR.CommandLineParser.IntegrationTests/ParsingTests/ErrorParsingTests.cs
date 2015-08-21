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
            CommandResultCode expectedReturnCode = CommandResultCode.NoArgs;

            // Act
            CommandResult<ICommand> actual = parser.Parse(args);

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
            CommandResultCode expectedReturnCode = CommandResultCode.NoCommandName;

            // Act
            CommandResult<ICommand> actual = parser.Parse(args);

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
            CommandResultCode expectedReturnCode = CommandResultCode.NoCommandFound;

            // Act
            CommandResult<ICommand> actual = parser.Parse(args);

            // Assert
            Assert.False(actual.IsValid);
            Assert.Equal(expectedReturnCode, actual.ReturnCode);
            Assert.Null(actual.Command);
        }
    }
}