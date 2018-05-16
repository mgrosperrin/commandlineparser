using System.Collections.Generic;
using System.Linq;
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
    }
}