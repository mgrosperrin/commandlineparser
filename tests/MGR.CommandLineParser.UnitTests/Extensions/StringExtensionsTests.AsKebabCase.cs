using Xunit;

namespace MGR.CommandLineParser.UnitTests.Extensions;

public partial class StringExtensionsTests
{
    public class AsKebabCase
    {
        [Theory]
        [InlineData("", "")]
        [InlineData("Word", "word")]
        [InlineData("SingleWord", "single-word")]
        [InlineData("KC", "kc")]
        [InlineData("MSBuild", "msbuild")]
        [InlineData("RunMSBuild", "run-msbuild")]
        public void AsKebabCaseReturnsTheRightValue(string source, string expected)
        {
            // Arrange

            // Act
            var actual = source.AsKebabCase();

            // Assert
            Assert.Equal(expected, actual);
        }
    }
}