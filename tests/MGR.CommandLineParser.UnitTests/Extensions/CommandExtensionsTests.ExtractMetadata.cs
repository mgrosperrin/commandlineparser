using System;
using MGR.CommandLineParser.Command;
using Xunit;

namespace MGR.CommandLineParser.UnitTests.Extensions
{
    public partial class CommandExtensionsTests
    {
        public class ExtractMetadata
        {
            [Fact]
            public void ExtractNullCommandException()
            {
                // Arrange
                ICommand myCommand = null;
                string expectedExceptionMessage = @"command";

                // Act
                var actualException = Assert.Throws<ArgumentNullException>(() => myCommand.ExtractMetadata(null));

                // Assert
                Assert.Equal(expectedExceptionMessage, actualException.ParamName);
            }
        }
    }
}