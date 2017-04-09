using System;
using System.Collections.Generic;
using MGR.CommandLineParser.Command;
using MGR.CommandLineParser.Converters;
using Xunit;

namespace MGR.CommandLineParser.UnitTests.Command
{
    public partial class CommandTypeTests
    {
        public class Metadata
        {
            [Fact]
            public void TestCommandMetadataExtraction()
            {
                // Arrange
                var testCommandType =
                    new CommandType(typeof (TestCommand),
                        new List<IConverter>());
                var expectedName = "Test";
                var expectedDescription = "My great description";
                var expectedUsage = "test arg [option]";

                // Act
                var metadata = testCommandType.Metadata;

                // Assert
                Assert.Equal(expectedName, metadata.Name);
                Assert.Equal(expectedDescription, metadata.Description);
                Assert.Equal(expectedUsage, metadata.Usage);
            }

            [Command(Description = "My great description", Usage = "test arg [option]")]
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