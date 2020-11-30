using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MGR.CommandLineParser.Extensibility.ClassBased;
using MGR.CommandLineParser.Tests.Commands;
using Xunit;

namespace MGR.CommandLineParser.IntegrationTests.UnspecifiedCommand
{
    public class SimpleOptionsWithArgumentsTests : ConsoleLoggingTestsBase
    {
        [Fact]
        public async Task ParseWithValidArgs()
        {
            // Arrange
            var fileArgument = @"C:\temp\file.txt";
            var expectedOutputDirectory = @"C:\Temp";
            var expectedOutputFile = @"C:\Temp\otherfile.txt";
            IEnumerable<string> args = new[] {"import", fileArgument, "-p:50", @"-o:" + expectedOutputDirectory, @"-of:" + expectedOutputFile};
            var expectedReturnCode = CommandParsingResultCode.Success;
            var expectedMaxParallel = 50;
            var expectedNbOfArguments = 1;
            var expectedArgumentsValue = new List<string> {fileArgument};

            // Act
            var actual = await CallParse(args);

            // Assert
            Assert.True(actual.IsValid);
            Assert.Equal(expectedReturnCode, actual.ParsingResultCode);
            Assert.IsAssignableFrom<IClassBasedCommandObject>(actual.CommandObject);
            Assert.IsType<ImportCommand>(((IClassBasedCommandObject)actual.CommandObject).Command);
            var importCommand = (ImportCommand) ((IClassBasedCommandObject)actual.CommandObject).Command;
            Assert.Equal(expectedOutputDirectory, importCommand.OutputDirectory.FullName);
            Assert.Equal(expectedOutputFile, importCommand.OutputFile.FullName);
            Assert.Equal(expectedMaxParallel, importCommand.MaxItemInParallel);
            Assert.Equal(expectedNbOfArguments, importCommand.Arguments.Count);
            for (var i = 0; i < expectedNbOfArguments; i++)
            {
                Assert.Equal(expectedArgumentsValue[i], importCommand.Arguments[i]);
            }
        }
        [Fact]
        public async Task ParseWithValidArgs2()
        {
            // Arrange
            IEnumerable<string> args = new[]
                {"IntTest", "--str-value:custom value", "-i", "42", "Custom argument value", "-b"};
            var expectedReturnCode = CommandParsingResultCode.Success;
            var expectedStrValue = "custom value";
            var expectedNbOfArguments = 1;
            var expectedArgumentsValue = "Custom argument value";
            var expectedIntValue = 42;

            // Act
            var actual = await CallParse(args);

            // Assert
            Assert.True(actual.IsValid);
            Assert.Equal(expectedReturnCode, actual.ParsingResultCode);
            Assert.IsAssignableFrom<IClassBasedCommandObject>(actual.CommandObject);
            Assert.IsType<IntTestCommand>(((IClassBasedCommandObject)actual.CommandObject).Command);
            var rawCommand = (IntTestCommand)((IClassBasedCommandObject)actual.CommandObject).Command;
            Assert.Equal(expectedStrValue, rawCommand.StrValue);
            Assert.Equal(expectedIntValue, rawCommand.IntValue);
            Assert.Null(rawCommand.IntListValue);
            Assert.Equal(expectedNbOfArguments, rawCommand.Arguments.Count);
            Assert.Equal(expectedArgumentsValue, rawCommand.Arguments.Single());
            Assert.True(rawCommand.BoolValue);
        }
    }
}