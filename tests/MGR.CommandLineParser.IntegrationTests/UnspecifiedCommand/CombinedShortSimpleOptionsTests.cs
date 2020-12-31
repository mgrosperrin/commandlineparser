using System.Collections.Generic;
using System.Threading.Tasks;
using MGR.CommandLineParser.Extensibility.ClassBased;
using MGR.CommandLineParser.Tests.Commands;
using Xunit;

namespace MGR.CommandLineParser.IntegrationTests.UnspecifiedCommand
{
    public class CombinedShortSimpleOptionsTests : ConsoleLoggingTestsBase
    {
        [Fact]
        public async Task ParseWithValidArgs()
        {
            // Arrange
            IEnumerable<string> args = new[] { "pack", "--version:abc", "-vt" };
            var expectedReturnCode = CommandParsingResultCode.Success;
            var expectedVersion = "abc";

            // Act
            var actual = await CallParse(args);

            // Assert
            Assert.True(actual.IsValid);
            Assert.Equal(expectedReturnCode, actual.ParsingResultCode);
            Assert.IsAssignableFrom<IClassBasedCommandObject>(actual.CommandObject);
            Assert.IsType<PackCommand>(((IClassBasedCommandObject)actual.CommandObject).Command);
            var rawCommand = (PackCommand)((IClassBasedCommandObject)actual.CommandObject).Command;
            Assert.True(rawCommand.Verbose);
            Assert.True(rawCommand.Tool);
            Assert.False(rawCommand.Build);
            Assert.Equal(expectedVersion, rawCommand.Version);
        }
        [Fact]
        public async Task ParseWithValidArgsWithFalse()
        {
            // Arrange
            IEnumerable<string> args = new[] { "pack", "--version:abc", "-vt:-", "-b" };
            var expectedReturnCode = CommandParsingResultCode.Success;
            var expectedVersion = "abc";

            // Act
            var actual = await CallParse(args);

            // Assert
            Assert.True(actual.IsValid);
            Assert.Equal(expectedReturnCode, actual.ParsingResultCode);
            Assert.IsAssignableFrom<IClassBasedCommandObject>(actual.CommandObject);
            Assert.IsType<PackCommand>(((IClassBasedCommandObject)actual.CommandObject).Command);
            var rawCommand = (PackCommand)((IClassBasedCommandObject)actual.CommandObject).Command;
            Assert.False(rawCommand.Verbose);
            Assert.False(rawCommand.Tool);
            Assert.True(rawCommand.Build);
            Assert.Equal(expectedVersion, rawCommand.Version);
        }
    }
}