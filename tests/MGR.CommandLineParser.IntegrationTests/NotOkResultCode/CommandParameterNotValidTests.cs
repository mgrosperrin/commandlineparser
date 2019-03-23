using System.Collections.Generic;
using System.Linq;
using MGR.CommandLineParser.Extensibility.ClassBased;
using MGR.CommandLineParser.Tests.Commands;
using Xunit;

namespace MGR.CommandLineParser.IntegrationTests.NotOkResultCode
{
    public class CommandParameterNotValidTests : ConsoleLoggingTestsBase
    {
        [Fact]
        public void ParseWithCommandNameAndInvalidArgs()
        {
            // Arrange
            IEnumerable<string> args = new[] {"IntTest", "-i", "42", "Custom argument value", "-b"};
            var expectedReturnCode = CommandParsingResultCode.CommandParametersNotValid;
            var expectedNbOfArguments = 1;
            var expectedArgumentsValue = "Custom argument value";
            var expectedIntValue = 42;

            // Act
            var actual = CallParse(args);

            // Assert
            Assert.False(actual.IsValid);
            Assert.Equal(expectedReturnCode, actual.ParsingResultCode);
            Assert.IsAssignableFrom<IClassBasedCommandObject>(actual.CommandObject);
            Assert.IsType<IntTestCommand>(((IClassBasedCommandObject)actual.CommandObject).Command);
            var rawCommand = (IntTestCommand)((IClassBasedCommandObject)actual.CommandObject).Command;
            Assert.Equal(expectedIntValue, rawCommand.IntValue);
            Assert.Null(rawCommand.IntListValue);
            Assert.Equal(expectedNbOfArguments, rawCommand.Arguments.Count);
            Assert.Equal(expectedArgumentsValue, rawCommand.Arguments.Single());
            Assert.True(rawCommand.BoolValue);
        }

        [Fact]
        public void ParseWithSpecifiedCommandAndInvalidArgs()
        {
            // Arrange
            IEnumerable<string> args = new[] { "-i", "42", "Custom argument value", "-b" };
            var expectedReturnCode = CommandParsingResultCode.CommandParametersNotValid;
            var expectedNbOfArguments = 1;
            var expectedArgumentsValue = "Custom argument value";
            var expectedIntValue = 42;

            // Act
            var actual = CallParse<IntTestCommand>(args);

            // Assert
            Assert.False(actual.IsValid);
            Assert.Equal(expectedReturnCode, actual.ParsingResultCode);
            Assert.IsAssignableFrom<IClassBasedCommandObject>(actual.CommandObject);
            Assert.IsType<IntTestCommand>(((IClassBasedCommandObject)actual.CommandObject).Command);
            var rawCommand = (IntTestCommand)((IClassBasedCommandObject)actual.CommandObject).Command;
            Assert.Equal(expectedIntValue, rawCommand.IntValue);
            Assert.Null(rawCommand.IntListValue);
            Assert.Equal(expectedNbOfArguments, rawCommand.Arguments.Count);
            Assert.Equal(expectedArgumentsValue, rawCommand.Arguments.Single());
            Assert.True(rawCommand.BoolValue);
        }
    }
}