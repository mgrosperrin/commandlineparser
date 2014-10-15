using System.Collections.Generic;
using System.Linq;
using MGR.CommandLineParser.Tests.Commands;
using Xunit;

namespace MGR.CommandLineParser.IntegrationTests.ParsingTests.ParseUniqueCommandTests
{
    public class ParseUniqueIntTestCommandTests
    {
        [Fact]
        public void ParseWithValidArgs()
        {
            // Arrange
            Parser parser = Parser.Create();
            IEnumerable<string> args = new[] {"-Strvalue:custom value", "-i", "42", "Custom argument value", "-b"};
            var expectedReturnCode = CommandResultCode.Ok;
            string expectedStrValue = "custom value";
            int expectedNbOfArguments = 1;
            string expectedArgumentsValue = "Custom argument value";
            int expectedIntValue = 42;

            // Act
            CommandResult<IntTestCommand> actual = parser.Parse<IntTestCommand>(args);

            // Assert
            Assert.True(actual.IsValid);
            Assert.Equal(expectedReturnCode, actual.ReturnCode);
            Assert.IsType<IntTestCommand>(actual.Command);
            Assert.Equal(expectedStrValue, actual.Command.StrValue);
            Assert.Equal(expectedIntValue, actual.Command.IntValue);
            Assert.Null(actual.Command.IntListValue);
            Assert.Equal(expectedNbOfArguments, actual.Command.Arguments.Count);
            Assert.Equal(expectedArgumentsValue, actual.Command.Arguments.Single());
            Assert.True(actual.Command.BoolValue);
        }

        [Fact]
        public void ParseWithValidListArgs()
        {
            // Arrange
            Parser parser = Parser.Create();
            IEnumerable<string> args = new[]
                {"-Strvalue:custom value", "-i", "42", "-il", "42", "Custom argument value", "-b"};
            var expectedReturnCode = CommandResultCode.Ok;
            string expectedStrValue = "custom value";
            int expectedNbOfArguments = 1;
            string expectedArgumentsValue = "Custom argument value";
            int expectedIntValue = 42;

            // Act
            CommandResult<IntTestCommand> actual = parser.Parse<IntTestCommand>(args);

            // Assert
            Assert.True(actual.IsValid);
            Assert.Equal(expectedReturnCode, actual.ReturnCode);
            Assert.IsType<IntTestCommand>(actual.Command);
            Assert.Equal(expectedStrValue, (actual.Command).StrValue);
            Assert.Equal(expectedIntValue, (actual.Command).IntValue);
            Assert.NotNull(actual.Command.IntListValue);
            Assert.Equal(expectedNbOfArguments, actual.Command.Arguments.Count);
            Assert.Equal(expectedArgumentsValue, actual.Command.Arguments.Single());
            Assert.Equal(expectedNbOfArguments, actual.Command.IntListValue.Count);
            Assert.Equal(expectedIntValue, actual.Command.IntListValue.Single());
            Assert.True(actual.Command.BoolValue);
        }

        [Fact]
        public void ParseWithInvalidArgs()
        {
            // Arrange
            Parser parser = Parser.Create();
            IEnumerable<string> args = new[] {"-i", "42", "Custom argument value", "-b"};
            var expectedReturnCode = CommandResultCode.CommandParameterNotValid;
            int expectedNbOfArguments = 1;
            string expectedArgumentsValue = "Custom argument value";
            int expectedIntValue = 42;

            // Act
            CommandResult<IntTestCommand> actual = parser.Parse<IntTestCommand>(args);

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