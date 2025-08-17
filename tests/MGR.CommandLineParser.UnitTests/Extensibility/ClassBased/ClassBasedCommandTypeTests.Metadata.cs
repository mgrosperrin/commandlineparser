using MGR.CommandLineParser.Command;
using MGR.CommandLineParser.Extensibility.ClassBased;
using MGR.CommandLineParser.Extensibility.Converters;
using Xunit;

namespace MGR.CommandLineParser.UnitTests.Extensibility.ClassBased;

public partial class ClassBasedCommandTypeTests
{
    public class Metadata
    {
        [Fact]
        public void TestCommandMetadataExtraction()
        {
            // Arrange
            var testCommandType =
                new ClassBasedCommandType(typeof(TestCommand),
                    new List<IConverter>(), new List<IPropertyOptionAlternateNameGenerator>());
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
        private class TestCommand : CommandData, ICommandHandler<TestCommand>
        {
            public Task<int> ExecuteAsync(TestCommand commandData, CancellationToken cancellationToken) => throw new NotImplementedException();
        }
    }
}