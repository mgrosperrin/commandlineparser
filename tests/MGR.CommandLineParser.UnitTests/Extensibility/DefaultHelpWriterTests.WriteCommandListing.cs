using System.Linq;
using MGR.CommandLineParser.Extensibility;
using MGR.CommandLineParser.Extensibility.Command;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

namespace MGR.CommandLineParser.UnitTests.Extensibility
{
    public partial class DefaultHelpWriterTests
    {
        public class WriteCommandListing
        {
            [Fact]
            public void NoCommandTypesDisplayOnlyGeneralInformation()
            {
                var console = new FakeConsole();
                var parserOptions = new ParserOptions {
                    Logo = "Logo Unit Test",
                    CommandLineName = "tool.exe"
                };
                var parserOptionsAccessorMock = new Mock<IOptions<ParserOptions>>();
                parserOptionsAccessorMock.SetupGet(_ => _.Value).Returns(parserOptions);
                var commandTypeProviderMock = new Mock<ICommandTypeProvider>();
                commandTypeProviderMock.Setup(_ => _.GetAllCommandTypes()).ReturnsAsync(Enumerable.Empty<ICommandType>);
                var helpWriter = new DefaultHelpWriter(console, new[] { commandTypeProviderMock.Object }, parserOptionsAccessorMock.Object);
                var expected = @"Logo Unit Test
Usage: tool.exe <command> [options] [args]
Type 'tool.exe help <command>' for help on a specific command.

Available commands:
No commands found.
";

                using (new LangageSwitcher("en-us"))
                {
                    helpWriter.WriteCommandListing();
                }
                var messages = console.Messages;

                Assert.Single(messages);
                Assert.IsType<FakeConsole.InformationMessage>(messages[0]);
                Assert.Equal(expected, messages[0].ToString(), ignoreLineEndingDifferences: true);
            }
        }

        [Fact]
        public void OneCommandTypeDisplayOnlyGeneralInformationThenHelpForTheCommand()
        {
            var console = new FakeConsole();
            var parserOptions = new ParserOptions {
                Logo = "Logo Unit Test",
                CommandLineName = "tool.exe"
            };
            var parserOptionsAccessorMock = new Mock<IOptions<ParserOptions>>();
            parserOptionsAccessorMock.SetupGet(_ => _.Value).Returns(parserOptions);
            var commandTypeProviderMock = new Mock<ICommandTypeProvider>();
            var commandMetadataMock = new Mock<ICommandMetadata>();
            commandMetadataMock.SetupGet(_ => _.HideFromHelpListing).Returns(false);
            commandMetadataMock.SetupGet(_ => _.Name).Returns("test");
            commandMetadataMock.SetupGet(_ => _.Description).Returns("test command");
            var commandTypeMock = new Mock<ICommandType>();
            commandTypeMock.SetupGet(_ => _.Metadata).Returns(commandMetadataMock.Object);
            commandTypeProviderMock.Setup(_ => _.GetAllCommandTypes()).ReturnsAsync(new[] { commandTypeMock.Object });
            var helpWriter = new DefaultHelpWriter(console, new[] { commandTypeProviderMock.Object }, parserOptionsAccessorMock.Object);
            var expected = @"Logo Unit Test
Usage: tool.exe <command> [options] [args]
Type 'tool.exe help <command>' for help on a specific command.

Available commands:
 test   test command
";

            using (new LangageSwitcher("en-us"))
            {
                helpWriter.WriteCommandListing();
            }
            var messages = console.Messages;

            Assert.Single(messages);
            Assert.IsType<FakeConsole.InformationMessage>(messages[0]);
            Assert.Equal(expected, messages[0].ToString(), ignoreLineEndingDifferences: true);
        }

        [Fact]
        public void TwoCommandTypesDisplayOnlyGeneralInformationThenHelpForTheCommand()
        {
            var console = new FakeConsole();
            var parserOptions = new ParserOptions {
                Logo = "Logo Unit Test",
                CommandLineName = "tool.exe"
            };
            var parserOptionsAccessorMock = new Mock<IOptions<ParserOptions>>();
            parserOptionsAccessorMock.SetupGet(_ => _.Value).Returns(parserOptions);
            var commandTypeProviderMock = new Mock<ICommandTypeProvider>();
            var commandMetadata1Mock = new Mock<ICommandMetadata>();
            commandMetadata1Mock.SetupGet(_ => _.HideFromHelpListing).Returns(false);
            commandMetadata1Mock.SetupGet(_ => _.Name).Returns("build");
            commandMetadata1Mock.SetupGet(_ => _.Description).Returns("description for build command");
            var commandType1Mock = new Mock<ICommandType>();
            commandType1Mock.SetupGet(_ => _.Metadata).Returns(commandMetadata1Mock.Object);
            var commandMetadata2Mock = new Mock<ICommandMetadata>();
            commandMetadata2Mock.SetupGet(_ => _.HideFromHelpListing).Returns(false);
            commandMetadata2Mock.SetupGet(_ => _.Name).Returns("test");
            commandMetadata2Mock.SetupGet(_ => _.Description).Returns("description for test command");
            var commandType2Mock = new Mock<ICommandType>();
            commandType2Mock.SetupGet(_ => _.Metadata).Returns(commandMetadata2Mock.Object);
            commandTypeProviderMock.Setup(_ => _.GetAllCommandTypes()).ReturnsAsync(new[] { commandType1Mock.Object, commandType2Mock.Object });
            var helpWriter = new DefaultHelpWriter(console, new[] { commandTypeProviderMock.Object }, parserOptionsAccessorMock.Object);
            var expected = @"Logo Unit Test
Usage: tool.exe <command> [options] [args]
Type 'tool.exe help <command>' for help on a specific command.

Available commands:
 build   description for build command
 test    description for test command
";

            using (new LangageSwitcher("en-us"))
            {
                helpWriter.WriteCommandListing();
            }
            var messages = console.Messages;

            Assert.Single(messages);
            Assert.IsType<FakeConsole.InformationMessage>(messages[0]);
            Assert.Equal(expected, messages[0].ToString(), ignoreLineEndingDifferences: true);
        }
    }
}
