using System.IO;
using System.Threading.Tasks;
using MGR.CommandLineParser.Extensibility.ClassBased;
using MGR.CommandLineParser.Tests.Commands;
using Xunit;

namespace MGR.CommandLineParser.IntegrationTests.UnspecifiedCommand
{
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
            File.WriteAllLines(tempResponseFile, new[]
            {
                "install",
                "--version:12.34",
                "--exclude-version"
            });

            // Act
            var actual = await CallParse(new[] { "@" + tempResponseFile });

            // Assert
            Assert.True(actual.IsValid);
            Assert.Empty(actual.ValidationResults);
            Assert.IsAssignableFrom<IClassBasedCommandObject>(actual.CommandObject);
            Assert.IsType<InstallCommand>(((IClassBasedCommandObject)actual.CommandObject).Command);
            var installCommand = (InstallCommand)((IClassBasedCommandObject)actual.CommandObject).Command;
            Assert.Empty(installCommand.Source);
            Assert.True(string.IsNullOrEmpty(installCommand.OutputDirectory));
            Assert.Equal("12.34", installCommand.Version);
            Assert.True(installCommand.ExcludeVersion);
            Assert.False(installCommand.Prerelease);
            Assert.False(installCommand.NoCache);
            Assert.Empty(installCommand.Arguments);
        }
    }
}
