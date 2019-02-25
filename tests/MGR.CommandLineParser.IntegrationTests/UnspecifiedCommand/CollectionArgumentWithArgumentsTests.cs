using System.Collections.Generic;
using System.Linq;
using MGR.CommandLineParser.Tests.Commands;
using Xunit;

namespace MGR.CommandLineParser.IntegrationTests.UnspecifiedCommand
{
    public class CollectionArgumentWithArgumentsTests : ConsoleLoggingTestsBase
    {
        [Fact]
        public void ParseWithValidListArgs()
        {
            // Arrange
            var parserBuild = new ParserBuilder();
            var parser = parserBuild.BuildParser();
            IEnumerable<string> args = new[]
                {"IntTest", "--Strvalue:custom value", "-i", "42", "-il", "42", "Custom argument value", "-b"};
            var expectedReturnCode = CommandParsingResultCode.Success;
            var expectedStrValue = "custom value";
            var expectedNbOfArguments = 1;
            var expectedArgumentsValue = "Custom argument value";
            var expectedIntValue = 42;

            // Act
            var actual = parser.Parse(args);

            // Assert
            Assert.True(actual.IsValid);
            Assert.Equal(expectedReturnCode, actual.ParsingResultCode);
            Assert.IsType<IntTestCommand>(actual.Command);
            Assert.Equal(expectedStrValue, ((IntTestCommand)actual.Command).StrValue);
            Assert.Equal(expectedIntValue, ((IntTestCommand)actual.Command).IntValue);
            Assert.NotNull(((IntTestCommand)actual.Command).IntListValue);
            Assert.Equal(expectedNbOfArguments, ((IntTestCommand)actual.Command).Arguments.Count);
            Assert.Equal(expectedArgumentsValue, ((IntTestCommand)actual.Command).Arguments.Single());
            Assert.Equal(expectedNbOfArguments, ((IntTestCommand)actual.Command).IntListValue.Count);
            Assert.Equal(expectedIntValue, ((IntTestCommand)actual.Command).IntListValue.Single());
            Assert.True(((IntTestCommand)actual.Command).BoolValue);
        }
    }
}