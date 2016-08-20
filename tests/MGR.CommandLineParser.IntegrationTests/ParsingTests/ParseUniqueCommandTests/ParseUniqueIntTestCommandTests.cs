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
            var parserBuild = new ParserBuilder();
            var parser = parserBuild.BuildParser();
            IEnumerable<string> args = new[] {"-Strvalue:custom value", "-i", "42", "Custom argument value", "-b"};
            var expectedReturnCode = CommandResultCode.Ok;
            var expectedStrValue = "custom value";
            var expectedNbOfArguments = 1;
            var expectedArgumentsValue = "Custom argument value";
            var expectedIntValue = 42;

            // Act
            var actual = parser.Parse<IntTestCommand>(args);

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
            var parserBuild = new ParserBuilder();
            var parser = parserBuild.BuildParser();
            IEnumerable<string> args = new[]
                {"-Strvalue:custom value", "-i", "42", "-il", "42", "Custom argument value", "-b"};
            var expectedReturnCode = CommandResultCode.Ok;
            var expectedStrValue = "custom value";
            var expectedNbOfArguments = 1;
            var expectedArgumentsValue = "Custom argument value";
            var expectedIntValue = 42;

            // Act
            var actual = parser.Parse<IntTestCommand>(args);

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
            var parserBuild = new ParserBuilder();
            var parser = parserBuild.BuildParser();
            IEnumerable<string> args = new[] {"-i", "42", "Custom argument value", "-b"};
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
        [Fact]
        public void ParseWithValidArgsAnDoubleDash()
        {
            // Arrange
            var parserBuild = new ParserBuilder();
            var parser = parserBuild.BuildParser();
            IEnumerable<string> args = new[] { "-Strvalue:custom value", "-i", "42", "Custom argument value", "-b", "--", "firstArg", "-i", "32" };
            var expectedReturnCode = CommandResultCode.Ok;
            var expectedStrValue = "custom value";
            var expectedNbOfArguments = 4;
            var expectedArgumentsValue = "Custom argument value";
            var expectedIntValue = 42;

            // Act
            var actual = parser.Parse<IntTestCommand>(args);

            // Assert
            Assert.True(actual.IsValid);
            Assert.Equal(expectedReturnCode, actual.ReturnCode);
            Assert.IsType<IntTestCommand>(actual.Command);
            Assert.Equal(expectedStrValue, actual.Command.StrValue);
            Assert.Equal(expectedIntValue, actual.Command.IntValue);
            Assert.Null(actual.Command.IntListValue);
            Assert.Equal(expectedNbOfArguments, actual.Command.Arguments.Count);
            Assert.Equal(new List<string> { expectedArgumentsValue , "firstArg", "-i", "32"}, actual.Command.Arguments);
            Assert.True(actual.Command.BoolValue);
        }
    }
}