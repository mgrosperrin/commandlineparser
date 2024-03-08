using System.Collections.Generic;
using System.Threading.Tasks;
using MGR.CommandLineParser.Extensibility.ClassBased;
using MGR.CommandLineParser.Tests.Commands;
using MGR.CommandLineParser.UnitTests;
using Xunit;

namespace MGR.CommandLineParser.IntegrationTests.Help
{
    public class AsOptionsTests : ConsoleLoggingTestsBase
    {
        [Fact]
        public async Task ParseWithValidArgs()
        {
            // Arrange
            IEnumerable<string> args = new[] { "--help" };
            var expectedReturnCode = CommandParsingResultCode.Success;
            var expectedHelpMessage = @"Integration Tests
Usage: test.exe <command> [options] [args]
Type 'test.exe help <command>' for help on a specific command.

Usage: test.exe IntTest 


Options:
 --str-value       (s)   A simple string value
 --int-value       (i)   A simple integer value
 --bool-value      (b)   A boolean value
 --int-list-value+ (il)  A list of integer value
 --help            (?)   Help
";
            //var expectedNbOfArguments = 1;
            //var expectedArgumentsValue = "Custom argument value";
            //var expectedIntValue = 42;

            // Act
            var actual = await CallParseWithDefaultCommand<IntTestCommand>(args);

            // Assert
            Assert.True(actual.IsValid);
            Assert.Equal(expectedReturnCode, actual.ParsingResultCode);
            Assert.IsAssignableFrom<IClassBasedCommandObject>(actual.CommandObject);
            Assert.IsType<IntTestCommand>(((IClassBasedCommandObject)actual.CommandObject).Command);
            var rawCommand = (IntTestCommand)((IClassBasedCommandObject)actual.CommandObject).Command;

            Assert.Equal(0, await rawCommand.ExecuteAsync());
            var message = Assert.Single(Console.Messages);

            Assert.IsType< FakeConsole.InformationMessage>(message);
            Assert.Equal(expectedHelpMessage, message.ToString());
        }
    }
}