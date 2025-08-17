using MGR.CommandLineParser.Extensibility;
using MGR.CommandLineParser.Extensibility.Command;
using Microsoft.Extensions.Options;
using Xunit;

namespace MGR.CommandLineParser.UnitTests.Extensibility;

public partial class DefaultHelpWriterTests
{
    public class WriteCommandListing
    {
        [Fact]
        public async Task NoCommandTypesDisplayOnlyGeneralInformation()
        {
            var console = new FakeConsole();
            var parserOptions = new ParserOptions {
                Logo = "Logo Unit Test",
                CommandLineName = "tool.exe"
            };
            var parserOptionsAccessorSubstitute = Substitute.For<IOptions<ParserOptions>>();
            parserOptionsAccessorSubstitute.Value.Returns(parserOptions);
            var commandTypeProviderSubstitute = Substitute.For<ICommandTypeProvider>();
            commandTypeProviderSubstitute.GetAllCommandTypes().Returns(Enumerable.Empty<ICommandType>());
            var helpWriter = new DefaultHelpWriter(console, [commandTypeProviderSubstitute], parserOptionsAccessorSubstitute);
            var expected = @"Logo Unit Test
Usage: tool.exe <command> [options] [args]
Type 'tool.exe help <command>' for help on a specific command.

Available commands:
No commands found.
";

            using (new LangageSwitcher("en-us"))
            {
                await helpWriter.WriteCommandListing();
            }
            var messages = console.Messages;

            Assert.Single(messages);
            Assert.IsType<FakeConsole.InformationMessage>(messages[0]);
            Assert.Equal(expected, messages[0].ToString(), ignoreLineEndingDifferences: true);
        }
    }

    [Fact]
    public async Task OneCommandTypeDisplayOnlyGeneralInformationThenHelpForTheCommand()
    {
        var console = new FakeConsole();
        var parserOptions = new ParserOptions {
            Logo = "Logo Unit Test",
            CommandLineName = "tool.exe"
        };
        var parserOptionsAccessorSubstitute = Substitute.For<IOptions<ParserOptions>>();
        parserOptionsAccessorSubstitute.Value.Returns(parserOptions);
        var commandTypeProviderSubstitute = Substitute.For<ICommandTypeProvider>();
        var commandMetadataSubstitute = Substitute.For<ICommandMetadata>();
        commandMetadataSubstitute.HideFromHelpListing.Returns(false);
        commandMetadataSubstitute.Name.Returns("test");
        commandMetadataSubstitute.Description.Returns("test command");
        var commandTypeSubstitute = Substitute.For<ICommandType>();
        commandTypeSubstitute.Metadata.Returns(commandMetadataSubstitute);
        commandTypeProviderSubstitute.GetAllCommandTypes().Returns([commandTypeSubstitute]);
        var helpWriter = new DefaultHelpWriter(console, [commandTypeProviderSubstitute], parserOptionsAccessorSubstitute);
        var expected = @"Logo Unit Test
Usage: tool.exe <command> [options] [args]
Type 'tool.exe help <command>' for help on a specific command.

Available commands:
 test   test command
";

        using (new LangageSwitcher("en-us"))
        {
            await helpWriter.WriteCommandListing();
        }
        var messages = console.Messages;

        Assert.Single(messages);
        Assert.IsType<FakeConsole.InformationMessage>(messages[0]);
        Assert.Equal(expected, messages[0].ToString(), ignoreLineEndingDifferences: true);
    }

    [Fact]
    public async Task TwoCommandTypesDisplayOnlyGeneralInformationThenHelpForTheCommand()
    {
        var console = new FakeConsole();
        var parserOptions = new ParserOptions {
            Logo = "Logo Unit Test",
            CommandLineName = "tool.exe"
        };
        var parserOptionsAccessorSubstitute = Substitute.For<IOptions<ParserOptions>>();
        parserOptionsAccessorSubstitute.Value.Returns(parserOptions);
        var commandTypeProviderSubstitute = Substitute.For<ICommandTypeProvider>();
        var commandMetadata1Substitute = Substitute.For<ICommandMetadata>();
        commandMetadata1Substitute.HideFromHelpListing.Returns(false);
        commandMetadata1Substitute.Name.Returns("build");
        commandMetadata1Substitute.Description.Returns("description for build command");
        var commandType1Substitute = Substitute.For<ICommandType>();
        commandType1Substitute.Metadata.Returns(commandMetadata1Substitute);
        var commandMetadata2Substitute = Substitute.For<ICommandMetadata>();
        commandMetadata2Substitute.HideFromHelpListing.Returns(false);
        commandMetadata2Substitute.Name.Returns("test");
        commandMetadata2Substitute.Description.Returns("description for test command");
        var commandType2Substitute = Substitute.For<ICommandType>();
        commandType2Substitute.Metadata.Returns(commandMetadata2Substitute);
        commandTypeProviderSubstitute.GetAllCommandTypes().Returns([commandType1Substitute, commandType2Substitute]);
        var helpWriter = new DefaultHelpWriter(console, [commandTypeProviderSubstitute], parserOptionsAccessorSubstitute);
        var expected = @"Logo Unit Test
Usage: tool.exe <command> [options] [args]
Type 'tool.exe help <command>' for help on a specific command.

Available commands:
 build   description for build command
 test    description for test command
";

        using (new LangageSwitcher("en-us"))
        {
            await helpWriter.WriteCommandListing();
        }
        var messages = console.Messages;

        Assert.Single(messages);
        Assert.IsType<FakeConsole.InformationMessage>(messages[0]);
        Assert.Equal(expected, messages[0].ToString(), ignoreLineEndingDifferences: true);
    }
}
