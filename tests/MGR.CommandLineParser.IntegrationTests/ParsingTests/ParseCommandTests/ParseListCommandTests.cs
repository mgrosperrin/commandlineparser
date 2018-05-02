using System.Collections.Generic;
using MGR.CommandLineParser.Tests.Commands;
using MGR.CommandLineParser.UnitTests;
using Xunit;

namespace MGR.CommandLineParser.IntegrationTests.ParsingTests.ParseCommandTests
{
    public class ParseListCommandTests : ConsoleLoggingTestsBase
    {
        [Fact]
        public void ShowHelpForThePackCommand()
        {
            // Arrange
            var parserBuild = new ParserBuilder()
                .Logo("Display help for list command")
                .CommandLineName("myListTest.exe");
            var parser = parserBuild.BuildParser();
            IEnumerable<string> args = new[] { "list", "/help" };
            var expectedReturnCode = CommandResultCode.Ok;
            var expectedResult = 0;
            var expectedHelp = @"Display help for list command
Usage: myListTest.exe <command> [options] [args]
Type 'myListTest.exe help <command>' for help on a specific command.

Usage: myListTest.exe List ListCommandUsageDescription
ListCommandDescription

Options:
 -Source+                        ListCommandSourceDescription
 -Verbose                        ListCommandVerboseListDescription
 -AllVersions|all-versions       ListCommandAllVersionsDescription
 -Prerelease                     ListCommandPrerelease
 -Help                      (?)  Help

Samples:
List sample 1
List sample number 2
";

            // Act
            using (new LangageSwitcher("en-us"))
            {
                var actual = parser.Parse(args);
                var actualResult = actual.Execute();

                // Assert
                Assert.True(actual.IsValid);
                Assert.Equal(expectedReturnCode, actual.ReturnCode);
                Assert.IsType<ListCommand>(actual.Command);
                var actualHelp = StringConsole.Current.OutAsString();
                Assert.Equal(expectedHelp, actualHelp, ignoreLineEndingDifferences: true);
                Assert.Equal(expectedResult, actualResult);
            }
        }
    }
}