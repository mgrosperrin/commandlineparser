using System.Collections.Generic;
using System.Linq;
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
            var parserBuild = new ParserBuilder();
            var parser = parserBuild.BuildParser();
            IEnumerable<string> args = new[] {"IntTest", "-i", "42", "Custom argument value", "-b"};
            var expectedReturnCode = CommandResultCode.CommandParameterNotValid;
            var expectedNbOfArguments = 1;
            var expectedArgumentsValue = "Custom argument value";
            var expectedIntValue = 42;

            // Act
            var actual = parser.Parse(args);

            // Assert
            Assert.False(actual.IsValid);
            Assert.Equal(expectedReturnCode, actual.ReturnCode);
            Assert.IsType<IntTestCommand>(actual.Command);
            Assert.Equal(expectedIntValue, ((IntTestCommand) actual.Command).IntValue);
            Assert.Null(((IntTestCommand) actual.Command).IntListValue);
            Assert.Equal(expectedNbOfArguments, ((IntTestCommand) actual.Command).Arguments.Count);
            Assert.Equal(expectedArgumentsValue, ((IntTestCommand) actual.Command).Arguments.Single());
            Assert.True(((IntTestCommand) actual.Command).BoolValue);
        }

        [Fact]
        public void ParseWithSpecifiedCommandAndInvalidArgs()
        {
            // Arrange
            var parserBuild = new ParserBuilder();
            var parser = parserBuild.BuildParser();
            IEnumerable<string> args = new[] { "-i", "42", "Custom argument value", "-b" };
            var expectedReturnCode = CommandResultCode.CommandParameterNotValid;
            var expectedNbOfArguments = 1;
            var expectedArgumentsValue = "Custom argument value";
            var expectedIntValue = 42;

            // Act
            var actual = parser.Parse<IntTestCommand>(args);

            // Assert
            Assert.False(actual.IsValid);
            Assert.Equal(expectedReturnCode, actual.ReturnCode);
            Assert.IsType<IntTestCommand>(actual.Command);
            Assert.Equal(expectedIntValue, actual.Command.IntValue);
            Assert.Null(actual.Command.IntListValue);
            Assert.Equal(expectedNbOfArguments, actual.Command.Arguments.Count);
            Assert.Equal(expectedArgumentsValue, actual.Command.Arguments.Single());
            Assert.True(actual.Command.BoolValue);
        }
    }
}