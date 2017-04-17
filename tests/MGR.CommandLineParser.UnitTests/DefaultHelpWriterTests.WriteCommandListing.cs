using System;
using System.Collections.Generic;
using System.Linq;
using MGR.CommandLineParser.Command;
using Moq;
using Xunit;

namespace MGR.CommandLineParser.UnitTests
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
                var helpWriter = new DefaultHelpWriter(console, commandTypeProviderMock.Object);
                var parserOptions = new ParserOptions
                {
                    Logo = "Logo Unit Test",
                    CommandLineName = "tool.exe"
                };
                var expected = @"Logo Unit Test
Usage: tool.exe <command> [options] [args]
Type 'tool.exe Help <command>' for help on a specific command.

Available commands:
No commands found.
";

                helpWriter.WriteCommandListing(parserOptions);

                var actual = console.AsString();
                Assert.Equal(expected, actual);
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
                commandTypeProviderMock.Setup(_ => _.GetAllCommandTypes()).Returns(new [] {commandTypeMock.Object});
                var helpWriter = new DefaultHelpWriter(console, commandTypeProviderMock.Object);
                var parserOptions = new ParserOptions
                {
                    Logo = "Logo Unit Test",
                    CommandLineName = "tool.exe"
                };
                var expected = @"Logo Unit Test
Usage: tool.exe <command> [options] [args]
Type 'tool.exe Help <command>' for help on a specific command.

Available commands:
 test   test command
";

                helpWriter.WriteCommandListing(parserOptions);

                var actual = console.AsString();
                Assert.Equal(expected, actual);
            }
        }
    }
}