using System.Collections.Generic;
using MGR.CommandLineParser.Tests.Commands;
using Xunit;

namespace MGR.CommandLineParser.IntegrationTests.UnspecifiedCommand
{
    public class CombinedShortSimpleOptionsTests : ConsoleLoggingTestsBase
    {
        [Fact]
        public void ParseWithValidArgs()
        {
            // Arrange
            var parserBuild = new ParserBuilder();
            var parser = parserBuild.BuildParser();
            IEnumerable<string> args = new[] { "pack", "--Version:abc", "-vt" };
            var expectedReturnCode = CommandParsingResultCode.Success;
            var expectedVersion = "abc";

            // Act
            var actual = parser.Parse(args);

            // Assert
            Assert.True(actual.IsValid);
            Assert.Equal(expectedReturnCode, actual.ParsingResultCode);
            Assert.IsType<PackCommand>(actual.Command);
            Assert.True(((PackCommand)actual.Command).Verbose);
            Assert.True(((PackCommand)actual.Command).Tool);
            Assert.False(((PackCommand)actual.Command).Build);
            Assert.Equal(expectedVersion, ((PackCommand)actual.Command).Version);
        }
        [Fact]
        public void ParseWithValidArgsWithFalse()
        {
            // Arrange
            var parserBuild = new ParserBuilder();
            var parser = parserBuild.BuildParser();
            IEnumerable<string> args = new[] { "pack", "--Version:abc", "-vt:-", "-b" };
            var expectedReturnCode = CommandParsingResultCode.Success;
            var expectedVersion = "abc";

            // Act
            var actual = parser.Parse(args);

            // Assert
            Assert.True(actual.IsValid);
            Assert.Equal(expectedReturnCode, actual.ParsingResultCode);
            Assert.IsType<PackCommand>(actual.Command);
            Assert.False(((PackCommand)actual.Command).Verbose);
            Assert.False(((PackCommand)actual.Command).Tool);
            Assert.True(((PackCommand)actual.Command).Build);
            Assert.Equal(expectedVersion, ((PackCommand)actual.Command).Version);
        }
    }
}