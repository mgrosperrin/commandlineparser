using MGR.CommandLineParser.Extensibility.ClassBased;
using MGR.CommandLineParser.Tests.Commands;
using Xunit;

namespace MGR.CommandLineParser.IntegrationTests.UnspecifiedCommand;

public class SimpleOptionsWithArgumentsTests : ConsoleLoggingTestsBase
{
    [Fact]
    public async Task ParseWithValidArgs()
    {
        // Arrange
        var fileArgument = @"C:\temp\file.txt";
        var expectedOutputDirectory = @"C:\Temp";
        var expectedOutputFile = @"C:\Temp\otherfile.txt";
        IEnumerable<string> args = ["import", fileArgument, "-p:50", @"-o:" + expectedOutputDirectory, @"-of:" + expectedOutputFile];
        var expectedReturnCode = CommandParsingResultCode.Success;
        var expectedMaxParallel = 50;
        var expectedNbOfArguments = 1;
        var expectedArgumentsValue = new List<string> { fileArgument };

        // Act
        var actual = await CallParse(args);

        // Assert
        Assert.True(actual.IsValid);
        Assert.Equal(expectedReturnCode, actual.ParsingResultCode);
        var classBasedCommandObject = Assert.IsAssignableFrom<IClassBasedCommandObject<ImportCommand, ImportCommand.ImportCommandData>>(actual.CommandObject);
        var importCommandData = classBasedCommandObject.CommandData;
        Assert.Equal(expectedOutputDirectory, importCommandData.OutputDirectory.FullName);
        Assert.Equal(expectedOutputFile, importCommandData.OutputFile.FullName);
        Assert.Equal(expectedMaxParallel, importCommandData.MaxItemInParallel);
        Assert.Equal(expectedNbOfArguments, importCommandData.Arguments.Count);
        for (var i = 0; i < expectedNbOfArguments; i++)
        {
            Assert.Equal(expectedArgumentsValue[i], importCommandData.Arguments[i]);
        }
    }
    [Fact]
    public async Task ParseWithValidArgs2()
    {
        // Arrange
        IEnumerable<string> args = ["IntTest", "--str-value:custom value", "-i", "42", "Custom argument value", "-b"];
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
        var classBasedCommandObject = Assert.IsAssignableFrom<IClassBasedCommandObject<IntTestCommand, IntTestCommand.IntTestCommandData>>(actual.CommandObject);
        var rawCommandData = classBasedCommandObject.CommandData;
        Assert.Equal(expectedStrValue, rawCommandData.StrValue);
        Assert.Equal(expectedIntValue, rawCommandData.IntValue);
        Assert.Null(rawCommandData.IntListValue);
        Assert.Equal(expectedNbOfArguments, rawCommandData.Arguments.Count);
        Assert.Equal(expectedArgumentsValue, rawCommandData.Arguments.Single());
        Assert.True(rawCommandData.BoolValue);
    }
}