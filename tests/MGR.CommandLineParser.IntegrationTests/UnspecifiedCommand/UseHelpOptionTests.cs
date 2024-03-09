using MGR.CommandLineParser.Extensibility.ClassBased;
using MGR.CommandLineParser.Tests.Commands;
using MGR.CommandLineParser.UnitTests;
using Xunit;

namespace MGR.CommandLineParser.IntegrationTests.UnspecifiedCommand;

public class UseHelpOptionTests : ConsoleLoggingTestsBase
{
    [Fact]
    public async Task ShowHelpForTheListCommand()
    {
        // Arrange
        var parserOptions = new ParserOptions {
            Logo = "Display help for list command",
            CommandLineName = "myListTest.exe"
        };
        IEnumerable<string> args = ["list", "--help"];
        var expectedReturnCode = CommandParsingResultCode.Success;
        var expectedResult = 0;
        var expected = @"Display help for list command
Usage: myListTest.exe <command> [options] [args]
Type 'myListTest.exe help <command>' for help on a specific command.

Usage: myListTest.exe List ListCommandUsageDescription
ListCommandDescription

Options:
 --source+            ListCommandSourceDescription
 --verbose            ListCommandVerboseListDescription
 --all-versions       ListCommandAllVersionsDescription
 --prerelease         ListCommandPrerelease
 --help          (?)  Help

Samples:
List sample 1
List sample number 2
";

        // Act
        using (new LangageSwitcher("en-us"))
        {
            var actual = await CallParse(parserOptions, args);
            var actualResult = await actual.ExecuteAsync(TestContext.Current.CancellationToken);

            // Assert
            Assert.True(actual.IsValid);
            Assert.Equal(expectedReturnCode, actual.ParsingResultCode);
            var classBasedCommandObject = Assert.IsAssignableFrom<IClassBasedCommandObject<ListCommand, ListCommand.ListCommandData>>(actual.CommandObject);
            Assert.Equal(expectedResult, actualResult);
            AssertOneMessageLoggedToConsole<FakeConsole.InformationMessage>(expected);
        }
    }

    [Fact]
    public async Task ShowHelpForThePackCommand()
    {
        // Arrange
        var parserOptions = new ParserOptions {
            Logo = "Display help for pack command",
            CommandLineName = "myPackTest.exe"
        };
        IEnumerable<string> args = ["pack", "--help"];
        var expectedReturnCode = CommandParsingResultCode.Success;
        var expectedResult = 0;
        var expected = @"Display help for pack command
Usage: myPackTest.exe <command> [options] [args]
Type 'myPackTest.exe help <command>' for help on a specific command.

Usage: myPackTest.exe Pack PackageCommandUsageSummary
PackageCommandDescription

Options:
 --output-directory          PackageCommandOutputDirDescription
 --base-path                 PackageCommandBasePathDescription
 --verbose              (v)  PackageCommandVerboseDescription
 --version                   PackageCommandVersionDescription
 --exclude+                  PackageCommandExcludeDescription
 --symbols                   PackageCommandSymbolsDescription
 --tool                 (t)  PackageCommandToolDescription
 --build                (b)  PackageCommandBuildDescription
 --msbuild-version           CommandMSBuildVersion
 --no-default-excludes       PackageCommandNoDefaultExcludes
 --no-package-analysis       PackageCommandNoRunAnalysis
 --properties#               PackageCommandPropertiesDescription
 --help                 (?)  Help
";

        // Act
        using (new LangageSwitcher("en-us"))
        {
            var actual = await CallParse(parserOptions, args);
            var actualResult = await actual.ExecuteAsync(TestContext.Current.CancellationToken);

            // Assert
            Assert.True(actual.IsValid);
            Assert.Equal(expectedReturnCode, actual.ParsingResultCode);
            var classBasedCommandObject = Assert.IsAssignableFrom<IClassBasedCommandObject<PackCommand, PackCommand.PackCommandData>>(actual.CommandObject);
            Assert.Equal(expectedResult, actualResult);
            AssertOneMessageLoggedToConsole<FakeConsole.InformationMessage>(expected);
        }
    }
}