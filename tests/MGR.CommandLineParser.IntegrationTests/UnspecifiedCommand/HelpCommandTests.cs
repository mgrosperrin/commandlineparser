using MGR.CommandLineParser.Command;
using MGR.CommandLineParser.Extensibility.ClassBased;
using MGR.CommandLineParser.UnitTests;
using Xunit;

namespace MGR.CommandLineParser.IntegrationTests.UnspecifiedCommand;

public class HelpCommandTests : ConsoleLoggingTestsBase
{
    [Fact]
    public async Task ShowGenericHelpForAllCommand()
    {
        // Arrange
        var parserOptions = new ParserOptions {
            Logo = "Display generic help",
            CommandLineName = "myHelpTest.exe"
        };
        IEnumerable<string> args = ["help"];
        var expectedReturnCode = CommandParsingResultCode.Success;
        var expectedResult = 0;
        var expected = @"Display generic help
Usage: myHelpTest.exe <command> [options] [args]
Type 'myHelpTest.exe help <command>' for help on a specific command.

Available commands:
 Delete      DeleteCommandDescription
 Enum        
 Help        
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
            var actual = await CallParse(parserOptions, args);
            var actualResult = await actual.ExecuteAsync(TestContext.Current.CancellationToken);

            // Assert
            Assert.True(actual.IsValid);
            Assert.Equal(expectedReturnCode, actual.ParsingResultCode);
            Assert.IsAssignableFrom<IClassBasedCommandObject<HelpCommand, HelpCommandData>>(actual.CommandObject);
            AssertOneMessageLoggedToConsole<FakeConsole.InformationMessage>(expected);
            Assert.Equal(expectedResult, actualResult);
        }
    }
}