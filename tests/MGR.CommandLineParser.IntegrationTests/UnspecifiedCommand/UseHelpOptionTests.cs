﻿using System.Collections.Generic;
using System.Threading.Tasks;
using MGR.CommandLineParser.Extensibility.ClassBased;
using MGR.CommandLineParser.Tests.Commands;
using MGR.CommandLineParser.UnitTests;
using Xunit;

namespace MGR.CommandLineParser.IntegrationTests.UnspecifiedCommand
{
    public class UseHelpOptionTests : ConsoleLoggingTestsBase
    {
        [Fact]
        public async Task ShowHelpForTheListCommand()
        {
            // Arrange
            var parserBuild = new ParserBuilder()
                .Logo("Display help for list command")
                .CommandLineName("myListTest.exe");
            IEnumerable<string> args = new[] { "list", "--help" };
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
                var actual = CallParse(parserBuild, args);
                var actualResult = await actual.ExecuteAsync();

                // Assert
                Assert.True(actual.IsValid);
                Assert.Equal(expectedReturnCode, actual.ParsingResultCode);
                Assert.IsType<ListCommand>(((IClassBasedCommandObject)actual.CommandObject).Command);
                Assert.Equal(expectedResult, actualResult);
                AssertOneMessageLoggedToConsole<FakeConsole.InformationMessage>(expected);
            }
        }

        [Fact]
        public async Task ShowHelpForThePackCommand()
        {
            // Arrange
            var parserBuild = new ParserBuilder()
                .Logo("Display help for pack command")
                .CommandLineName("myPackTest.exe");
            IEnumerable<string> args = new[] { "pack", "--help" };
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
                var actual = CallParse(parserBuild, args);
                var actualResult = await actual.ExecuteAsync();

                // Assert
                Assert.True(actual.IsValid);
                Assert.Equal(expectedReturnCode, actual.ParsingResultCode);
                Assert.IsAssignableFrom<IClassBasedCommandObject>(actual.CommandObject);
                Assert.IsType<PackCommand>(((IClassBasedCommandObject)actual.CommandObject).Command);
                Assert.Equal(expectedResult, actualResult);
                AssertOneMessageLoggedToConsole<FakeConsole.InformationMessage>(expected);
            }
        }
    }
}