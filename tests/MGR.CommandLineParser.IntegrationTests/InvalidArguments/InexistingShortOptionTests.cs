using System.Collections.Generic;
using MGR.CommandLineParser.Extensibility;
using MGR.CommandLineParser.UnitTests;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace MGR.CommandLineParser.IntegrationTests.InvalidArguments
{
    public class InexistingShortOptionTests : ConsoleLoggingTestsBase
    {
        [Fact]
        public void ParseWithInvalidArgs()
        {
            // Arrange
            var parserBuild = new ParserBuilder();
            var parser = parserBuild.BuildParser();
            IEnumerable<string> args = new[] { "delete", "--source:custom value", "-pn", "ApiKey", "MyApiKey", "Custom argument value", "b" };
            var expected = @"There is no option 'pn' for the command 'Delete'.
";
            var serviceProvider = CreateServiceProvider();
            var console = (FakeConsole)serviceProvider.GetRequiredService<IConsole>();

            // Act
            var parsingResult = parser.Parse(args, serviceProvider);

            // Assert
            Assert.NotNull(parsingResult);
            Assert.Equal(CommandParsingResultCode.CommandParametersNotValid, parsingResult.ParsingResultCode);
            var messages = console.Messages;
            Assert.Single(messages);
            Assert.IsType<FakeConsole.ErrorMessage>(messages[0]);
            Assert.Equal(expected, messages[0].ToString(), ignoreLineEndingDifferences: true);
        }
    }
}