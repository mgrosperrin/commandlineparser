using System.IO;
using MGR.CommandLineParser.Tests.Commands;
using Xunit;

namespace MGR.CommandLineParser.IntegrationTests.UnspecifiedCommand
{
    public class ArgumentsInResponseFileTests : ConsoleLoggingTestsBase
    {
        [Fact]
        public void ParseWithAResponseFile()
        {
            // Arrange
            var parser = new ParserBuilder().BuildParser();
            var tempFile = Path.GetRandomFileName();
            var tempResponseFileName = Path.ChangeExtension(tempFile, ".rsp");
            var tempFolder = Path.GetTempPath();
            var tempResponseFile = Path.Combine(tempFolder, tempResponseFileName);
            File.WriteAllLines(tempResponseFile, new[]
            {
                "install",
                "-version:12.34",
                "-excludeVersion"
            });

            // Act
            var actual = parser.Parse(new[] { "@" + tempResponseFile });

            // Assert
            Assert.True(actual.IsValid);
            Assert.Empty(actual.ValidationResults);
            Assert.IsType<InstallCommand>(actual.Command);
            var installCommand = (InstallCommand)actual.Command;
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
