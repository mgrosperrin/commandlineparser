using System.Collections.Generic;
using MGR.CommandLineParser.Tests.Commands;
using Xunit;

namespace MGR.CommandLineParser.IntegrationTests.UnspecifiedCommand
{
    public class SimpleOptionsTests : ConsoleLoggingTestsBase
    {
        [Fact]
        public void ParseWithValidArgs()
        {
            // Arrange
            var parserBuild = new ParserBuilder();
            var parser = parserBuild.BuildParser();
            IEnumerable<string> args = new[] {"delete", "-Source:custom value", "-np", "-ApiKey", "MyApiKey", "Custom argument value", "b"};
            var expectedReturnCode = CommandResultCode.Ok;
            var expectedSource = "custom value";
            var expectedApiKey = "MyApiKey";
            var expectedNbOfArguments = 2;
            var expectedArgumentsValue = new List<string> {"Custom argument value", "b"};

            // Act
            var actual = parser.Parse(args);

            // Assert
            Assert.True(actual.IsValid);
            Assert.Equal(expectedReturnCode, actual.ReturnCode);
            Assert.IsType<DeleteCommand>(actual.Command);
            Assert.Equal(expectedSource, ((DeleteCommand) actual.Command).Source);
            Assert.Equal(expectedApiKey, ((DeleteCommand) actual.Command).ApiKey);
            Assert.True(((DeleteCommand) actual.Command).NoPrompt);
            Assert.Null(((DeleteCommand) actual.Command).SourceProvider);
            Assert.Null(((DeleteCommand) actual.Command).Settings);
            Assert.Equal(expectedNbOfArguments, ((DeleteCommand) actual.Command).Arguments.Count);
            for (var i = 0; i < expectedNbOfArguments; i++)
            {
                Assert.Equal(expectedArgumentsValue[i], actual.Command.Arguments[i]);
            }
        }
    }
}