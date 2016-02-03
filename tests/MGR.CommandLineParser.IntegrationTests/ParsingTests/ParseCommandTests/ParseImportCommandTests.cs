using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MGR.CommandLineParser.IntegrationTests.Commands;
using MGR.CommandLineParser.Tests.Commands;
using Xunit;

namespace MGR.CommandLineParser.IntegrationTests.ParsingTests.ParseCommandTests
{
    public class ParseImportCommandTests
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
            IEnumerable<string> args = new[] { "import", fileArgument, "/p:50", @"/o:" + expectedOutputDirectory, @"/of:" + expectedOutputFile };
            var expectedReturnCode = CommandResultCode.Ok;
            var expectedMaxParallel = 50;
            var expectedNbOfArguments = 1;
            var expectedArgumentsValue = new List<string> { fileArgument };

            // Act
            var actual = parser.Parse(args);

            // Assert
            Assert.True(actual.IsValid);
            Assert.Equal(expectedReturnCode, actual.ReturnCode);
            Assert.IsType<ImportCommand>(actual.Command);
            var importCommand = (ImportCommand)actual.Command;
            Assert.Equal(expectedOutputDirectory, importCommand.OutputDirectory.FullName);
            Assert.Equal(expectedOutputFile, importCommand.OutputFile.FullName);
            Assert.Equal(expectedMaxParallel, importCommand.MaxItemInParallel);
            Assert.Equal(expectedNbOfArguments, importCommand.Arguments.Count);
            for (var i = 0; i < expectedNbOfArguments; i++)
            {
                Assert.Equal(expectedArgumentsValue[i], actual.Command.Arguments[i]);
            }
        }
    }
}
