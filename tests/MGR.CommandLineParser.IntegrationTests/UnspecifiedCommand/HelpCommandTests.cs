using System.Collections.Generic;
using MGR.CommandLineParser.Command;
using MGR.CommandLineParser.UnitTests;
using Xunit;

namespace MGR.CommandLineParser.IntegrationTests.UnspecifiedCommand
{
    public class HelpCommandTests : ConsoleLoggingTestsBase
    {
        [Fact]
        public void ShowGenericHelpForAllCommand()
        {
            // Arrange
            var parserBuild = new ParserBuilder()
                .Logo("Display generic help")
                .CommandLineName("myHelpTest.exe");
            var parser = parserBuild.BuildParser(CreateServiceProvider());
            IEnumerable<string> args = new[] {"help"};
            var expectedReturnCode = CommandResultCode.Ok;
            var expectedResult = 0;
            var expectedHelp = @"Display generic help
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
                var actual = parser.Parse(args);
                var actualResult = actual.Execute();

                // Assert
                Assert.True(actual.IsValid);
                Assert.Equal(expectedReturnCode, actual.ReturnCode);
                Assert.IsType<HelpCommand>(actual.Command);
                var actualHelp = StringConsole.Current.OutAsString();
                Assert.Equal(expectedHelp, actualHelp, ignoreLineEndingDifferences: true);
                Assert.Equal(expectedResult, actualResult);
            }
        }
    }
}