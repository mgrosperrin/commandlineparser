using System.Collections.Generic;
using System.Reflection;
using MGR.CommandLineParser.Command;
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
                var commandMetadata = new CommandMetadataTemplate {Name = "MyCommand"};
                var optionMetadata = new OptionMetadataTemplate(propertyInfo, commandMetadata)
                {
                    Name = propertyInfo.Name
                };
                var expected = string.Empty;

                // Act
                var actual = HelpCommand.GetMultiValueIndicator(optionMetadata);

                // Assert
                Assert.Equal(expected, actual);
            }

            [Fact]
            public void TestListProperty()
            {
                // Arrange
                var propertyInfo = GetType()
                    .GetProperty(TypeHelpers.ExtractPropertyName(() => ListIntProperty));
                var commandMetadata = new CommandMetadataTemplate {Name = "MyCommand"};
                var optionMetadata = new OptionMetadataTemplate(propertyInfo, commandMetadata)
                {
                    Name = propertyInfo.Name
                };
                var expected = "+";

                // Act
                var actual = HelpCommand.GetMultiValueIndicator(optionMetadata);

                // Assert
                Assert.Equal(expected, actual);
            }

            [Fact]
            public void TestDictionaryProperty()
            {
                // Arrange
                var propertyInfo =
                    GetType().GetProperty(TypeHelpers.ExtractPropertyName(() => DictionaryProperty));
                var commandMetadata = new CommandMetadataTemplate {Name = "MyCommand"};
                var optionMetadata = new OptionMetadataTemplate(propertyInfo, commandMetadata)
                {
                    Name = propertyInfo.Name
                };
                var expected = "#";

                // Act
                var actual = HelpCommand.GetMultiValueIndicator(optionMetadata);

                // Assert
                Assert.Equal(expected, actual);
            }
        }
    }
}