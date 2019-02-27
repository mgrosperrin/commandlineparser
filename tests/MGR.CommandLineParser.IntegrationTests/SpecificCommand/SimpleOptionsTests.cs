using System.Collections.Generic;
using System.Linq;
using MGR.CommandLineParser.Extensibility.ClassBased;
using MGR.CommandLineParser.Tests.Commands;
using Xunit;

namespace MGR.CommandLineParser.IntegrationTests.SpecificCommand
{
    public class SimpleOptionsTests : ConsoleLoggingTestsBase
    {
        [Fact]
        public void ParseWithValidArgs()
        {
            // Arrange
            var parserBuild = new ParserBuilder();
            var parser = parserBuild.BuildParser();
            IEnumerable<string> args = new[] {"-Strvalue:custom value", "-i", "42", "Custom argument value", "-b"};
            var expectedReturnCode = CommandParsingResultCode.Success;
            var expectedStrValue = "custom value";
            var expectedNbOfArguments = 1;
            var expectedArgumentsValue = "Custom argument value";
            var expectedIntValue = 42;

            // Act
            var actual = parser.Parse<IntTestCommand>(args);

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
            Assert.Equal(expectedArgumentsValue, rawCommand.Arguments.Single());
            Assert.True(rawCommand.BoolValue);
        }
    }
}