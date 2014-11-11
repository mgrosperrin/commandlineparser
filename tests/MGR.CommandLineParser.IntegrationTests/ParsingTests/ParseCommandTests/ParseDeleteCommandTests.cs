using System;
using System.Collections.Generic;
using MGR.CommandLineParser.Command;
using MGR.CommandLineParser.Tests.Commands;
using Xunit;

namespace MGR.CommandLineParser.IntegrationTests.ParsingTests.ParseCommandTests
{
    public class ParseDeleteCommandTests
    {
        [Fact]
        public void ParseWithValidArgs()
        {
            // Arrange
            Parser parser = Parser.Create();
            IEnumerable<string> args = new[] { "delete", "-Source:custom value", "-np", "-ApiKey", "MyApiKey", "Custom argument value", "b" };
            CommandResultCode expectedReturnCode = CommandResultCode.Ok;
            string expectedSource = "custom value";
            string expectedApiKey = "MyApiKey";
            int expectedNbOfArguments = 2;
            var expectedArgumentsValue = new List<string> { "Custom argument value", "b" };

            // Act
            CommandResult<ICommand> actual = parser.Parse(args);

            // Assert
            Assert.True(actual.IsValid);
            Assert.Equal(expectedReturnCode, actual.ReturnCode);
            Assert.IsType<DeleteCommand>(actual.Command);
            Assert.Equal(expectedSource, ((DeleteCommand)actual.Command).Source);
            Assert.Equal(expectedApiKey, ((DeleteCommand)actual.Command).ApiKey);
            Assert.True(((DeleteCommand)actual.Command).NoPrompt);
            Assert.Null(((DeleteCommand)actual.Command).SourceProvider);
            Assert.Null(((DeleteCommand)actual.Command).Settings);
            Assert.Equal(expectedNbOfArguments, ((DeleteCommand)actual.Command).Arguments.Count);
            for (int i = 0; i < expectedNbOfArguments; i++)
            {
                Assert.Equal(expectedArgumentsValue[i], actual.Command.Arguments[i]);
            }
        }
        [Fact]
        public void ParseWithInValidArgs()
        {
            // Arrange
            Parser parser = Parser.Create();
            IEnumerable<string> args = new[] { "delete", "-Source:custom value", "-pn", "ApiKey", "MyApiKey", "Custom argument value", "b" };
            string expectedMessageException = @"There is no option 'pn' for the command 'Delete'.";

            // Act
            Exception actual = null;
            try
            {
                parser.Parse(args);
            }
            catch (Exception exception)
            {
                actual = exception;
            }

            // Assert
            Assert.NotNull(actual);
            Assert.IsType<CommandLineParserException>(actual);
            Assert.Equal(expectedMessageException, actual.Message);
        }
    }
}