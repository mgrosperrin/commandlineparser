using System.Collections.Generic;
using System.Linq;
using MGR.CommandLineParser.Tests.Commands;
using Xunit;

namespace MGR.CommandLineParser.IntegrationTests.UnspecifiedCommand
{
    public class SimpleOptionsWithArgumentsTests : ConsoleLoggingTestsBase
    {
        [Fact]
        public void ParseWithValidArgs()
        {
            // Arrange
            var parserBuild = new ParserBuilder();
            var parser = parserBuild.BuildParser();
            var fileArgument = @"C:\temp\file.txt";
            var expectedOutputDirectory = @"C:\Temp";
            var expectedOutputFile = @"C:\Temp\otherfile.txt";
            IEnumerable<string> args = new[] {"import", fileArgument, "-p:50", @"-o:" + expectedOutputDirectory, @"-of:" + expectedOutputFile};
            var expectedReturnCode = CommandParsingResultCode.Success;
            var expectedMaxParallel = 50;
            var expectedNbOfArguments = 1;
            var expectedArgumentsValue = new List<string> {fileArgument};

            // Act
            var actual = parser.Parse(args);

            // Assert
            Assert.True(actual.IsValid);
            Assert.Equal(expectedReturnCode, actual.ParsingResultCode);
            Assert.IsType<ImportCommand>(actual.Command);
            var importCommand = (ImportCommand) actual.Command;
            Assert.Equal(expectedOutputDirectory, importCommand.OutputDirectory.FullName);
            Assert.Equal(expectedOutputFile, importCommand.OutputFile.FullName);
            Assert.Equal(expectedMaxParallel, importCommand.MaxItemInParallel);
            Assert.Equal(expectedNbOfArguments, importCommand.Arguments.Count);
            for (var i = 0; i < expectedNbOfArguments; i++)
            {
                Assert.Equal(expectedArgumentsValue[i], actual.Command.Arguments[i]);
            }
        }
        [Fact]
        public void ParseWithValidArgs2()
        {
            // Arrange
            var parserBuild = new ParserBuilder();
            var parser = parserBuild.BuildParser();
            IEnumerable<string> args = new[]
                {"IntTest", "--Strvalue:custom value", "-i", "42", "Custom argument value", "-b"};
            var expectedReturnCode = CommandParsingResultCode.Success;
            var expectedStrValue = "custom value";
            var expectedNbOfArguments = 1;
            var expectedArgumentsValue = "Custom argument value";
            var expectedIntValue = 42;

            // Act
            var actual = parser.Parse(args);

            // Assert
            Assert.True(actual.IsValid);
            Assert.Equal(expectedReturnCode, actual.ParsingResultCode);
            Assert.IsType<IntTestCommand>(actual.Command);
            Assert.Equal(expectedStrValue, ((IntTestCommand)actual.Command).StrValue);
            Assert.Equal(expectedIntValue, ((IntTestCommand)actual.Command).IntValue);
            Assert.Null(((IntTestCommand)actual.Command).IntListValue);
            Assert.Equal(expectedNbOfArguments, ((IntTestCommand)actual.Command).Arguments.Count);
            Assert.Equal(expectedArgumentsValue, ((IntTestCommand)actual.Command).Arguments.Single());
            Assert.True(((IntTestCommand)actual.Command).BoolValue);
        }
    }
}