using System.Collections.Generic;
using MGR.CommandLineParser.Extensibility.ClassBased;
using MGR.CommandLineParser.Tests.Commands;
using Xunit;

namespace MGR.CommandLineParser.IntegrationTests.SpecificCommand
{
    public class EscapedTraillingArgumentTests : ConsoleLoggingTestsBase
    {
        [Fact]
        public void ParseWithValidArgsAnDoubleDash()
        {
            // Arrange
            IEnumerable<string> args = new[] {"--str-value:custom value", "-i", "42", "Custom argument value", "-b", "--", "firstArg", "-i", "32"};
            var expectedReturnCode = CommandParsingResultCode.Success;
            var expectedStrValue = "custom value";
            var expectedNbOfArguments = 4;
            var expectedArgumentsValue = "Custom argument value";
            var expectedIntValue = 42;

            // Act
            var actual = CallParse<IntTestCommand>(args);

            // Assert
            Assert.True(actual.IsValid);
            Assert.Equal(expectedReturnCode, actual.ParsingResultCode);
            Assert.IsAssignableFrom<IClassBasedCommandObject>(actual.CommandObject);
            Assert.IsType<IntTestCommand>(((IClassBasedCommandObject)actual.CommandObject).Command);
            var rawCommand = (IntTestCommand)((IClassBasedCommandObject)actual.CommandObject).Command;
            Assert.Equal(expectedStrValue, rawCommand.StrValue);
            Assert.Equal(expectedIntValue, rawCommand.IntValue);
            Assert.Null(rawCommand.IntListValue);
            Assert.Equal(expectedNbOfArguments, rawCommand.Arguments.Count);
            Assert.Equal(new List<string> {expectedArgumentsValue, "firstArg", "-i", "32"}, rawCommand.Arguments);
            Assert.True(rawCommand.BoolValue);
        }
    }
}