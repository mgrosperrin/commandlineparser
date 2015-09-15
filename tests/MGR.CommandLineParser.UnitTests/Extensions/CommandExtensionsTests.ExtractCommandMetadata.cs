using System;
using System.Collections.Generic;
using MGR.CommandLineParser.Command;
using Xunit;

namespace MGR.CommandLineParser.UnitTests.Extensions
{
    public partial class CommandExtensionsTests
    {
        public class ExtractCommandMetadataTemplate
        {
            [Fact]
            public void TestCommandMetadataExtraction()
            {
                // Arrange
                ICommand myCommand = new TestCommand();
                var expectedName = "Test";
                var expectedDescription = "My great description";
                var expectedUsage = "test arg [option]";

                // Act
                var metadata = myCommand.GetType().ExtractCommandMetadataTemplate();

                // Assert
                Assert.Equal(expectedName, metadata.Name);
                Assert.Equal(expectedDescription, metadata.Description);
                Assert.Equal(expectedUsage, metadata.Usage);
            }

            [CommandDisplay(Description = "My great description", Usage = "test arg [option]")]
            private class TestCommand : ICommand
            {
                public int Execute()
                {
                    throw new NotImplementedException();
                }

                public IList<string> Arguments
                {
                    get { throw new NotImplementedException(); }
                }
            }
        }
    }
}