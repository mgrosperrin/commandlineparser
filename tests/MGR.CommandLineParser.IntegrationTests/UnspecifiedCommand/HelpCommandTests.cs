using System.Collections.Generic;
using System.Threading.Tasks;
using MGR.CommandLineParser.Command;
using MGR.CommandLineParser.Extensibility;
using MGR.CommandLineParser.Extensibility.ClassBased;
using MGR.CommandLineParser.UnitTests;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace MGR.CommandLineParser.IntegrationTests.UnspecifiedCommand
{
    public class HelpCommandTests : ConsoleLoggingTestsBase
    {
        [Fact]

        public async Task ShowGenericHelpForAllCommand()
        {
            // Arrange
            var parserBuild = new ParserBuilder()
                .Logo("Display generic help")
                .CommandLineName("myHelpTest.exe");
            var parser = parserBuild.BuildParser();
            IEnumerable<string> args = new[] { "help" };
            var serviceProvider = CreateServiceProvider();
            var console = (FakeConsole)serviceProvider.GetRequiredService<IConsole>();
            var expectedReturnCode = CommandParsingResultCode.Success;
            var expectedResult = 0;
            var expected = @"Display generic help
Usage: myHelpTest.exe <command> [options] [args]
Type 'myHelpTest.exe help <command>' for help on a specific command.

Available commands:
 Help        
 I           
 Delete      DeleteCommandDescription
 Import      
 Install     InstallCommandDescription
 IntTest     
 List        ListCommandDescription
 Pack        PackageCommandDescription
 Publish     PublishCommandDescription
 Push        PushCommandDescription
 SetApiKey   SetApiKeyCommandDescription
 Sources     SourcesCommandDescription
 Spec        SpecCommandDescription
 Update      UpdateCommandDescription
";

            // Act
            using (new LangageSwitcher("en-us"))
            {
                var actual = parser.Parse(args, serviceProvider);
                var actualResult = await actual.ExecuteAsync();

                // Assert
                Assert.True(actual.IsValid);
                Assert.Equal(expectedReturnCode, actual.ParsingResultCode);
                Assert.IsAssignableFrom<IClassBasedCommandObject>(actual.CommandObject);
                Assert.IsType<HelpCommand>(((IClassBasedCommandObject)actual.CommandObject).Command);
                var messages = console.Messages;
                Assert.Single(messages);
                Assert.IsType<FakeConsole.InformationMessage>(messages[0]);
                Assert.Equal(expected, messages[0].ToString(), ignoreLineEndingDifferences: true);
                Assert.Equal(expectedResult, actualResult);
            }
        }
    }
}