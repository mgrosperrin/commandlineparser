using System.Collections.Generic;
using MGR.CommandLineParser.Extensibility;
using MGR.CommandLineParser.UnitTests;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace MGR.CommandLineParser.IntegrationTests.InvalidArguments
{
    public class InvalidShortNameFormatTests : ConsoleLoggingTestsBase
    {
        [Fact]
        public void ParseWithInvalidShortOption()
        {
            // Arrange
            var parserBuild = new ParserBuilder();
            var parser = parserBuild.BuildParser();
            IEnumerable<string> args = new[] { "import", "--p:50" };
            var expected = @"There is no option 'p' for the command 'Import'.
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