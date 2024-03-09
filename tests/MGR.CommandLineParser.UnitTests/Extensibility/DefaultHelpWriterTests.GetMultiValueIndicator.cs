using MGR.CommandLineParser.Command;
using MGR.CommandLineParser.Extensibility;
using MGR.CommandLineParser.Extensibility.ClassBased;
using MGR.CommandLineParser.Extensibility.Converters;
using Xunit;

namespace MGR.CommandLineParser.UnitTests.Extensibility;

public partial class DefaultHelpWriterTests
{
    public class GetMultiValueIndicator : ICommandHandler<GetMultiValueIndicator.GetMultiValueIndicatorData>
    {

        public class GetMultiValueIndicatorData : CommandData
        {
            public int SimpleIntProperty { get; set; }
            public List<int> ListIntProperty { get; set; }
            public Dictionary<string, int> DictionaryProperty { get; set; }
        }
        [Fact]
        public void TestSimpleProperty()
        {
            // Arrange
            var propertyInfo =
                typeof(GetMultiValueIndicatorData).GetProperty(TypeHelpers.ExtractPropertyName<GetMultiValueIndicatorData, int>((o) => o.SimpleIntProperty));
            var commandMetadata = new ClassBasedCommandMetadata<GetMultiValueIndicator, GetMultiValueIndicatorData>();
            var commandOption = ClassBasedCommandOptionMetadata.Create(propertyInfo, commandMetadata, new List<IConverter> { new Int32Converter() }, new List<IPropertyOptionAlternateNameGenerator>());
            var expected = string.Empty;

            // Act
            var actual = DefaultHelpWriter.GetMultiValueIndicator(commandOption);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void TestListProperty()
        {
            // Arrange
            var propertyInfo = typeof(GetMultiValueIndicatorData)
                .GetProperty(TypeHelpers.ExtractPropertyName<GetMultiValueIndicatorData, List<int>>((o) => o.ListIntProperty));
            var commandMetadata = new ClassBasedCommandMetadata<GetMultiValueIndicator, GetMultiValueIndicatorData>();
            var commandOption = ClassBasedCommandOptionMetadata.Create(propertyInfo, commandMetadata, new List<IConverter> { new Int32Converter() }, new List<IPropertyOptionAlternateNameGenerator>());
            var expected = DefaultHelpWriter.CollectionIndicator;

            // Act
            var actual = DefaultHelpWriter.GetMultiValueIndicator(commandOption);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void TestDictionaryProperty()
        {
            // Arrange
            var propertyInfo =
                typeof(GetMultiValueIndicatorData).GetProperty(TypeHelpers.ExtractPropertyName<GetMultiValueIndicatorData, Dictionary<string, int>>((o) => o.DictionaryProperty));
            var commandMetadata = new ClassBasedCommandMetadata<GetMultiValueIndicator, GetMultiValueIndicatorData>();
            var commandOption = ClassBasedCommandOptionMetadata.Create(propertyInfo, commandMetadata, new List<IConverter> { new StringConverter(), new Int32Converter() }, new List<IPropertyOptionAlternateNameGenerator>());
            var expected = DefaultHelpWriter.DictionaryIndicator;

            // Act
            var actual = DefaultHelpWriter.GetMultiValueIndicator(commandOption);

            // Assert
            Assert.Equal(expected, actual);
        }
        public Task<int> ExecuteAsync(GetMultiValueIndicatorData commandData, CancellationToken cancellationToken) => throw new System.NotImplementedException();
    }
}