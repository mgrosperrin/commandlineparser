using System;
using System.Linq;
using MGR.CommandLineParser.Extensibility;
using MGR.CommandLineParser.Extensibility.Command;
using Moq;
using Xunit;

namespace MGR.CommandLineParser.UnitTests.Extensibility
{
    public partial class DefaultHelpWriterTests
    {
        public class WriteCommandListing
        {
            [Fact]
            public void NullParserOptionsThrowsException()
            {
                var defaultHelpWriter = new DefaultHelpWriter(null, null);
                var expectedParamNameException = "parserOptions";

                var actualException = Assert.Throws<ArgumentNullException>(() => defaultHelpWriter.WriteCommandListing(null));

                Assert.Equal(expectedParamNameException, actualException.ParamName);
            }

            [Fact]
            public void NoCommandTypesDisplayOnlyGeneralInformation()
            {
                var console = new StringConsole();
                var commandTypeProviderMock = new Mock<ICommandTypeProvider>();
                commandTypeProviderMock.Setup(_ => _.GetAllCommandTypes()).Returns(Enumerable.Empty<ICommandType>);
                var helpWriter = new DefaultHelpWriter(console, new[]{ commandTypeProviderMock.Object});
                var parserOptions = new ParserOptions
                {
                    Logo = "Logo Unit Test",
                    CommandLineName = "tool.exe"
                };
                var expected = @"Logo Unit Test
Usage: tool.exe <command> [options] [args]
Type 'tool.exe help <command>' for help on a specific command.

Available commands:
No commands found.
";

                using (new LangageSwitcher("en-us"))
                {
                    helpWriter.WriteCommandListing(parserOptions);
                }
                var actual = console.OutAsString();

                Assert.Equal(expected, actual, ignoreLineEndingDifferences: true);
            }

            [Fact]
            public void OneCommandTypeDisplayOnlyGeneralInformationThenHelpForTheCommand()
            {
                var console = new StringConsole();
                var commandTypeProviderMock = new Mock<ICommandTypeProvider>();
                var commandMetadataMock = new Mock<ICommandMetadata>();
                commandMetadataMock.SetupGet(_ => _.HideFromHelpListing).Returns(false);
                commandMetadataMock.SetupGet(_ => _.Name).Returns("test");
                commandMetadataMock.SetupGet(_ => _.Description).Returns("test command");
                var commandTypeMock = new Mock<ICommandType>();
                commandTypeMock.SetupGet(_ => _.Metadata).Returns(commandMetadataMock.Object);
                commandTypeProviderMock.Setup(_ => _.GetAllCommandTypes()).Returns(new[] { commandTypeMock.Object });
                var helpWriter = new DefaultHelpWriter(console, new[] { commandTypeProviderMock.Object });
                var parserOptions = new ParserOptions
                {
                    Logo = "Logo Unit Test",
                    CommandLineName = "tool.exe"
                };
                var expected = @"Logo Unit Test
Usage: tool.exe <command> [options] [args]
Type 'tool.exe help <command>' for help on a specific command.

Available commands:
 test   test command
";

                using (new LangageSwitcher("en-us"))
                {
                    helpWriter.WriteCommandListing(parserOptions);
                }
                var actual = console.OutAsString();

                Assert.Equal(expected, actual, ignoreLineEndingDifferences: true);
            }

            [Fact]
            public void TwoCommandTypesDisplayOnlyGeneralInformationThenHelpForTheCommand()
            {
                var console = new StringConsole();
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
                commandTypeProviderMock.Setup(_ => _.GetAllCommandTypes()).Returns(new[] { commandType1Mock.Object, commandType2Mock.Object });
                var helpWriter = new DefaultHelpWriter(console, new[] { commandTypeProviderMock.Object });
                var parserOptions = new ParserOptions
                {
                    Logo = "Logo Unit Test",
                    CommandLineName = "tool.exe"
                };
                var expected = @"Logo Unit Test
Usage: tool.exe <command> [options] [args]
Type 'tool.exe help <command>' for help on a specific command.

Available commands:
 build   description for build command
 test    description for test command
";

                using (new LangageSwitcher("en-us"))
                {
                    helpWriter.WriteCommandListing(parserOptions);
                }
                var actual = console.OutAsString();

                Assert.Equal(expected, actual, ignoreLineEndingDifferences: true);
            }
        }
    }
}