using System.Collections.Generic;
using MGR.CommandLineParser.Command;
using MGR.CommandLineParser.Converters;
using Xunit;

namespace MGR.CommandLineParser.UnitTests.Command
{
    public partial class HelpCommandTests
    {
        public class GetMultiValueIndicator
        {
            public int SimpleIntProperty { get; set; }
            public List<int> ListIntProperty { get; set; }
            public Dictionary<string, int> DictionaryProperty { get; set; }

            [Fact]
            public void TestSimpleProperty()
            {
                // Arrange
                var propertyInfo =
                    GetType().GetProperty(TypeHelpers.ExtractPropertyName(() => SimpleIntProperty));
                var commandMetadata = new CommandMetadata(typeof (GetMultiValueIndicator));
                var commandOption = CommandOption.Create(propertyInfo, commandMetadata, new List<IConverter> {new Int32Converter()});
                var expected = string.Empty;

                // Act
                var actual = HelpCommand.GetMultiValueIndicator(commandOption);

                // Assert
                Assert.Equal(expected, actual);
            }

            [Fact]
            public void TestListProperty()
            {
                // Arrange
                var propertyInfo = GetType()
                    .GetProperty(TypeHelpers.ExtractPropertyName(() => ListIntProperty));
                var commandMetadata = new CommandMetadata(typeof (GetMultiValueIndicator));
                var commandOption = CommandOption.Create(propertyInfo, commandMetadata, new List<IConverter> { new Int32Converter()});
                var expected = HelpCommand.CollectionIndicator;

                // Act
                var actual = HelpCommand.GetMultiValueIndicator(commandOption);

                // Assert
                Assert.Equal(expected, actual);
            }

            [Fact]
            public void TestDictionaryProperty()
            {
                // Arrange
                var propertyInfo =
                    GetType().GetProperty(TypeHelpers.ExtractPropertyName(() => DictionaryProperty));
                var commandMetadata = new CommandMetadata(typeof (GetMultiValueIndicator));
                var commandOption = CommandOption.Create(propertyInfo, commandMetadata,
                    new List<IConverter> {new StringConverter(), new Int32Converter()});
                var expected = HelpCommand.DictionaryIndicator;

                // Act
                var actual = HelpCommand.GetMultiValueIndicator(commandOption);

                // Assert
                Assert.Equal(expected, actual);
            }
        }
    }
}