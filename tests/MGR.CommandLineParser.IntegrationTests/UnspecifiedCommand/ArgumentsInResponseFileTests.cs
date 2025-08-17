using MGR.CommandLineParser.Extensibility.ClassBased;
using MGR.CommandLineParser.Tests.Commands;
using Xunit;

namespace MGR.CommandLineParser.IntegrationTests.UnspecifiedCommand;

public class ArgumentsInResponseFileTests : ConsoleLoggingTestsBase
{
    [Fact]
    public async Task ParseWithAResponseFile()
    {
        // Arrange
        var tempFile = Path.GetRandomFileName();
        var tempResponseFileName = Path.ChangeExtension(tempFile, ".rsp");
        var tempFolder = Path.GetTempPath();
        var tempResponseFile = Path.Combine(tempFolder, tempResponseFileName);
        File.WriteAllLines(tempResponseFile,
        [
            "install",
            "--version:12.34",
            "--exclude-version"
        ]);

        // Act
        var actual = await CallParse(["@" + tempResponseFile]);

        // Assert
        Assert.True(actual.IsValid);
        Assert.Empty(actual.ValidationResults);
        var classBasedCommandObject = Assert.IsAssignableFrom<IClassBasedCommandObject<InstallCommand, InstallCommand.InstallCommandData>>(actual.CommandObject);
        var installCommandData = classBasedCommandObject.CommandData;
        Assert.Empty(installCommandData.Source);
        Assert.True(string.IsNullOrEmpty(installCommandData.OutputDirectory));
        Assert.Equal("12.34", installCommandData.Version);
        Assert.True(installCommandData.ExcludeVersion);
        Assert.False(installCommandData.Prerelease);
        Assert.False(installCommandData.NoCache);
        Assert.Empty(installCommandData.Arguments);
    }
}
