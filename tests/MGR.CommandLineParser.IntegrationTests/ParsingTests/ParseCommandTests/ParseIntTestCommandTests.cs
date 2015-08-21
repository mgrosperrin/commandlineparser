using System.Collections.Generic;
using System.Linq;
using MGR.CommandLineParser.Command;
using MGR.CommandLineParser.Tests.Commands;
using Xunit;

namespace MGR.CommandLineParser.IntegrationTests.ParsingTests.ParseCommandTests
{
    public class ParseIntTestCommandTests
    {
        [Fact]
        public void ParseWithValidArgs()
        {
            // Arrange
            IParser parser = Parser.Create();
            IEnumerable<string> args = new[]
                {"IntTest", "-Strvalue:custom value", "-i", "42", "Custom argument value", "-b"};
            var expectedReturnCode = CommandResultCode.Ok;
            string expectedStrValue = "custom value";
            int expectedNbOfArguments = 1;
            string expectedArgumentsValue = "Custom argument value";
            int expectedIntValue = 42;

            // Act
            CommandResult<ICommand> actual = parser.Parse(args);

            // Assert
            Assert.True(actual.IsValid);
            Assert.Equal(expectedReturnCode, actual.ReturnCode);
            Assert.IsType<IntTestCommand>(actual.Command);
            Assert.Equal(expectedStrValue, ((IntTestCommand) actual.Command).StrValue);
            Assert.Equal(expectedIntValue, ((IntTestCommand) actual.Command).IntValue);
            Assert.Null(((IntTestCommand) actual.Command).IntListValue);
            Assert.Equal(expectedNbOfArguments, ((IntTestCommand) actual.Command).Arguments.Count);
            Assert.Equal(expectedArgumentsValue, ((IntTestCommand) actual.Command).Arguments.Single());
            Assert.True(((IntTestCommand) actual.Command).BoolValue);
        }

        [Fact]
        public void ParseWithValidListArgs()
        {
            // Arrange
            IParser parser = Parser.Create();
            IEnumerable<string> args = new[]
                {"IntTest", "-Strvalue:custom value", "-i", "42", "-il", "42", "Custom argument value", "-b"};
            var expectedReturnCode = CommandResultCode.Ok;
            string expectedStrValue = "custom value";
            int expectedNbOfArguments = 1;
            string expectedArgumentsValue = "Custom argument value";
            int expectedIntValue = 42;

            // Act
            CommandResult<ICommand> actual = parser.Parse(args);

            // Assert
            Assert.True(actual.IsValid);
            Assert.Equal(expectedReturnCode, actual.ReturnCode);
            Assert.IsType<IntTestCommand>(actual.Command);
            Assert.Equal(expectedStrValue, ((IntTestCommand) actual.Command).StrValue);
            Assert.Equal(expectedIntValue, ((IntTestCommand) actual.Command).IntValue);
            Assert.NotNull(((IntTestCommand) actual.Command).IntListValue);
            Assert.Equal(expectedNbOfArguments, ((IntTestCommand) actual.Command).Arguments.Count);
            Assert.Equal(expectedArgumentsValue, ((IntTestCommand) actual.Command).Arguments.Single());
            Assert.Equal(expectedNbOfArguments, ((IntTestCommand) actual.Command).IntListValue.Count);
            Assert.Equal(expectedIntValue, ((IntTestCommand) actual.Command).IntListValue.Single());
            Assert.True(((IntTestCommand) actual.Command).BoolValue);
        }

        [Fact]
        public void ParseWithInvalidArgs()
        {
            // Arrange
            IParser parser = Parser.Create();
            IEnumerable<string> args = new[] {"IntTest", "-i", "42", "Custom argument value", "-b"};
            var expectedReturnCode = CommandResultCode.CommandParameterNotValid;
            int expectedNbOfArguments = 1;
            string expectedArgumentsValue = "Custom argument value";
            int expectedIntValue = 42;

            // Act
            CommandResult<ICommand> actual = parser.Parse(args);

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
    }
}