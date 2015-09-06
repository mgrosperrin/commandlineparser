using System;
using System.Collections.Generic;
using System.Reflection;
using MGR.CommandLineParser.Command;
using MGR.CommandLineParser.Converters;
using Xunit;

namespace MGR.CommandLineParser.UnitTests.Extensions
{
    public partial class PropertyInfoExtensionsTests
    {
        public class ExtractConverterMetadata
        {
            public int OriginalProperty { get; set; }

            [Converter(typeof(Int32Converter))]
            public int CustomConverterProperty { get; set; }

            [Converter(typeof(GuidConverter))]
            public int WrongCustomConverterProperty { get; set; }

            public List<int> OriginalListProperty { get; set; }

            [Converter(typeof(Int32Converter))]
            public List<int> CustomListConverterProperty { get; set; }

            public Dictionary<int, Guid> OriginalDictionaryProperty { get; set; }

            [ConverterKeyValue(typeof(GuidConverter), typeof(Int32Converter))]
            public Dictionary<int, Guid> CustomDictionaryConverterProperty { get; set; }

            [ConverterKeyValue(typeof(GuidConverter))]
            public Dictionary<string, Guid> CustomValueOnlyDictionaryConverterProperty { get; set; }

            [ConverterKeyValue(typeof(GuidConverter))]
            public List<string> CustomDictionaryConverterWithListProperty { get; set; }

            [ConverterKeyValue(typeof(GuidConverter), typeof(Int32Converter))]
            public Dictionary<string, Guid> CustomDictionaryWithWrongKeyConverterProperty { get; set; }

            [ConverterKeyValue(typeof(GuidConverter), typeof(Int32Converter))]
            public Dictionary<int, string> CustomDictionaryWithWrongValueConverterProperty { get; set; }

            public Dictionary<string, Guid> OriginalDictionaryWithWrongKeyConverterProperty { get; set; }
            public Dictionary<int, string> OriginalDictionaryWithWrongValueConverterProperty { get; set; }

            [Fact]
            public void OriginalTest()
            {
                // Arrange
                var expectedName = TypeHelpers.ExtractPropertyName(() => OriginalProperty);
                var propertyInfo = GetType().GetProperty(expectedName);
                var commandMetadata = new CommandMetadataTemplate { Name = "MyCommand" };
                var optionMetadata = new OptionMetadataTemplate(propertyInfo, commandMetadata) { Name = expectedName };
                DefaultServiceResolver.RegisterServices(() => new List<IConverter> { new Int32Converter() });

                // Act
                var actual = propertyInfo.ExtractConverterMetadata(optionMetadata);

                // Assert
                Assert.NotNull(actual.Converter);
                Assert.IsType<Int32Converter>(actual.Converter);
            }

            [Fact]
            public void OriginalNoConverterTest()
            {
                // Arrange
                var expectedName = TypeHelpers.ExtractPropertyName(() => OriginalProperty);
                var propertyInfo = GetType().GetProperty(expectedName);
                var commandMetadata = new CommandMetadataTemplate { Name = "MyCommand" };
                var optionMetadata = new OptionMetadataTemplate(propertyInfo, commandMetadata) { Name = expectedName };
                DefaultServiceResolver.RegisterServices(() => new List<IConverter> { new StringConverter() });
                var expectedExceptionMessage =
                    string.Format("No converter found for the option '{0}' of the command '{1}' of type '{2}'.",
                        expectedName, commandMetadata.Name, typeof(int).FullName);

                // Act
                var actualException =
                    Assert.Throws<CommandLineParserException>(
                        () => propertyInfo.ExtractConverterMetadata(optionMetadata));

                // Assert
                Assert.Equal(expectedExceptionMessage, actualException.Message);
            }

            [Fact]
            public void CustomConverterTest()
            {
                // Arrange
                var expectedName = TypeHelpers.ExtractPropertyName(() => CustomConverterProperty);
                var propertyInfo = GetType().GetProperty(expectedName);
                var commandMetadata = new CommandMetadataTemplate { Name = "MyCommand" };
                var optionMetadata = new OptionMetadataTemplate(propertyInfo, commandMetadata) { Name = expectedName };
                DefaultServiceResolver.RegisterServices(() => new List<IConverter> { new StringConverter() });

                // Act
                var actual = propertyInfo.ExtractConverterMetadata(optionMetadata);

                // Assert
                Assert.NotNull(actual.Converter);
                Assert.IsType<Int32Converter>(actual.Converter);
            }

            [Fact]
            public void WrongCustomConverterTest()
            {
                // Arrange
                var expectedName = TypeHelpers.ExtractPropertyName(() => WrongCustomConverterProperty);
                var propertyInfo = GetType().GetProperty(expectedName);
                var commandMetadata = new CommandMetadataTemplate { Name = "MyCommand" };
                var optionMetadata = new OptionMetadataTemplate(propertyInfo, commandMetadata) { Name = expectedName };
                DefaultServiceResolver.RegisterServices(() => new List<IConverter> { new StringConverter() });
                var expectedExceptionMessage =
                    string.Format(
                        "The specified converter for the option '{0}' of the command '{1}' is not valid : property type : {2}, converter target type : {3}.",
                        expectedName, commandMetadata.Name, typeof(int).FullName, typeof(Guid).FullName);

                // Act
                var actualException =
                    Assert.Throws<CommandLineParserException>(
                        () => propertyInfo.ExtractConverterMetadata(optionMetadata));

                // Assert
                Assert.Equal(expectedExceptionMessage, actualException.Message);
            }

            [Fact]
            public void OriginalListTest()
            {
                // Arrange
                var expectedName = TypeHelpers.ExtractPropertyName(() => OriginalListProperty);
                var propertyInfo = GetType().GetProperty(expectedName);
                var commandMetadata = new CommandMetadataTemplate { Name = "MyCommand" };
                var optionMetadata = new OptionMetadataTemplate(propertyInfo, commandMetadata) { Name = expectedName };
                DefaultServiceResolver.RegisterServices(() => new List<IConverter> { new Int32Converter() });

                // Act
                var actual = propertyInfo.ExtractConverterMetadata(optionMetadata);

                // Assert
                Assert.NotNull(actual.Converter);
                Assert.IsType<Int32Converter>(actual.Converter);
            }

            [Fact]
            public void CustomListConverterTest()
            {
                // Arrange
                var expectedName = TypeHelpers.ExtractPropertyName(() => CustomListConverterProperty);
                var propertyInfo = GetType().GetProperty(expectedName);
                var commandMetadata = new CommandMetadataTemplate { Name = "MyCommand" };
                var optionMetadata = new OptionMetadataTemplate(propertyInfo, commandMetadata) { Name = expectedName };
                DefaultServiceResolver.RegisterServices(() => new List<IConverter> { new StringConverter() });

                // Act
                var actual = propertyInfo.ExtractConverterMetadata(optionMetadata);

                // Assert
                Assert.NotNull(actual.Converter);
                Assert.IsType<Int32Converter>(actual.Converter);
            }

            [Fact]
            public void OriginalDictionaryTest()
            {
                // Arrange
                var expectedName = TypeHelpers.ExtractPropertyName(() => OriginalDictionaryProperty);
                var propertyInfo = GetType().GetProperty(expectedName);
                var commandMetadata = new CommandMetadataTemplate { Name = "MyCommand" };
                var optionMetadata = new OptionMetadataTemplate(propertyInfo, commandMetadata) { Name = expectedName };
                DefaultServiceResolver.RegisterServices(() => new List<IConverter> { new Int32Converter(), new GuidConverter() });

                // Act
                var actual = propertyInfo.ExtractConverterMetadata(optionMetadata);

                // Assert
                Assert.NotNull(actual.Converter);
                Assert.IsType<KeyValueConverter>(actual.Converter);
            }

            [Fact]
            public void CustomDictionaryConverterTest()
            {
                // Arrange
                var expectedName = TypeHelpers.ExtractPropertyName(() => CustomDictionaryConverterProperty);
                var propertyInfo = GetType().GetProperty(expectedName);
                var commandMetadata = new CommandMetadataTemplate { Name = "MyCommand" };
                var optionMetadata = new OptionMetadataTemplate(propertyInfo, commandMetadata) { Name = expectedName };
                DefaultServiceResolver.RegisterServices(() => new List<IConverter> { new StringConverter(), new GuidConverter() });

                // Act
                var actual = propertyInfo.ExtractConverterMetadata(optionMetadata);

                // Assert
                Assert.NotNull(actual.Converter);
                Assert.IsType<KeyValueConverter>(actual.Converter);
            }

            [Fact]
            public void CustomValueOnlyDictionaryConverterTest()
            {
                // Arrange
                var expectedName = TypeHelpers.ExtractPropertyName(() => CustomValueOnlyDictionaryConverterProperty);
                var propertyInfo = GetType().GetProperty(expectedName);
                var commandMetadata = new CommandMetadataTemplate { Name = "MyCommand" };
                var optionMetadata = new OptionMetadataTemplate(propertyInfo, commandMetadata) { Name = expectedName };
                DefaultServiceResolver.RegisterServices(() => new List<IConverter> { new StringConverter(), new Int32Converter() });

                // Act
                var actual = propertyInfo.ExtractConverterMetadata(optionMetadata);

                // Assert
                Assert.NotNull(actual.Converter);
                Assert.IsType<KeyValueConverter>(actual.Converter);
            }

            [Fact]
            public void CustomDictionaryConverterWithListException()
            {
                // Arrange
                var expectedName = TypeHelpers.ExtractPropertyName(() => CustomDictionaryConverterWithListProperty);
                var propertyInfo = GetType().GetProperty(expectedName);
                var commandMetadata = new CommandMetadataTemplate { Name = "MyCommand" };
                var optionMetadata = new OptionMetadataTemplate(propertyInfo, commandMetadata) { Name = expectedName };
                DefaultServiceResolver.RegisterServices(() => new List<IConverter> { new StringConverter(), new GuidConverter() });
                var expectedExceptionMessage =
                    string.Format(
                        "The option '{0}' of the command 'MyCommand' defined a Key/Value converter but its type is not System.Generic.IDictionary<TKey, TValue>.",
                        expectedName);

                // Act
                var actualException =
                    Assert.Throws<CommandLineParserException>(
                        () => propertyInfo.ExtractConverterMetadata(optionMetadata));

                // Assert
                Assert.Equal(expectedExceptionMessage, actualException.Message);
            }

            [Fact]
            public void CustomDictionaryWithWrongKeyConverterException()
            {
                // Arrange
                var expectedName =
                    TypeHelpers.ExtractPropertyName(() => CustomDictionaryWithWrongKeyConverterProperty);
                var propertyInfo = GetType().GetProperty(expectedName);
                var commandMetadata = new CommandMetadataTemplate { Name = "MyCommand" };
                var optionMetadata = new OptionMetadataTemplate(propertyInfo, commandMetadata) { Name = expectedName };
                DefaultServiceResolver.RegisterServices(() => new List<IConverter> { new Int32Converter(), new GuidConverter() });
                var expectedExceptionMessage =
                    string.Format(
                        "The specified KeyValueConverter for the option '{0}' of the command '{1}' is not valid : key property type : {2}, key converter target type : {3}.",
                        expectedName, commandMetadata.Name, typeof(string).FullName, typeof(int).FullName);

                // Act
                var actualException =
                    Assert.Throws<CommandLineParserException>(
                        () => propertyInfo.ExtractConverterMetadata(optionMetadata));

                // Assert
                Assert.Equal(expectedExceptionMessage, actualException.Message);
            }

            [Fact]
            public void CustomDictionaryWithWrongValueConverterException()
            {
                // Arrange
                var expectedName =
                    TypeHelpers.ExtractPropertyName(() => CustomDictionaryWithWrongValueConverterProperty);
                var propertyInfo = GetType().GetProperty(expectedName);
                var commandMetadata = new CommandMetadataTemplate { Name = "MyCommand" };
                var optionMetadata = new OptionMetadataTemplate(propertyInfo, commandMetadata) { Name = expectedName };
                DefaultServiceResolver.RegisterServices(() => new List<IConverter> { new Int32Converter(), new GuidConverter() });
                var expectedExceptionMessage =
                    string.Format(
                        "The specified KeyValueConverter for the option '{0}' of the command '{1}' is not valid : value property type : {2}, value converter target type : {3}.",
                        expectedName, commandMetadata.Name, typeof(string).FullName, typeof(Guid).FullName);

                // Act
                var actualException =
                    Assert.Throws<CommandLineParserException>(
                        () => propertyInfo.ExtractConverterMetadata(optionMetadata));

                // Assert
                Assert.Equal(expectedExceptionMessage, actualException.Message);
            }

            [Fact]
            public void OriginalDictionaryWithoutKeyConverterException()
            {
                // Arrange
                var expectedName =
                    TypeHelpers.ExtractPropertyName(() => OriginalDictionaryWithWrongKeyConverterProperty);
                var propertyInfo = GetType().GetProperty(expectedName);
                var commandMetadata = new CommandMetadataTemplate { Name = "MyCommand" };
                var optionMetadata = new OptionMetadataTemplate(propertyInfo, commandMetadata) { Name = expectedName };
                DefaultServiceResolver.RegisterServices(() => new List<IConverter> { new Int32Converter(), new GuidConverter() });
                var expectedExceptionMessage =
                    string.Format(
                        "No converter found for the key type ('{2}') of the option '{0}' of the command '{1}'.",
                        expectedName, commandMetadata.Name, typeof(string).FullName);

                // Act
                var actualException =
                    Assert.Throws<CommandLineParserException>(
                        () => propertyInfo.ExtractConverterMetadata(optionMetadata));

                // Assert
                Assert.Equal(expectedExceptionMessage, actualException.Message);
            }

            [Fact]
            public void OriginalDictionaryWithoutValueConverterException()
            {
                // Arrange
                var expectedName =
                    TypeHelpers.ExtractPropertyName(() => OriginalDictionaryWithWrongValueConverterProperty);
                var propertyInfo = GetType().GetProperty(expectedName);
                var commandMetadata = new CommandMetadataTemplate { Name = "MyCommand" };
                var optionMetadata = new OptionMetadataTemplate(propertyInfo, commandMetadata) { Name = expectedName };
                DefaultServiceResolver.RegisterServices(() => new List<IConverter> { new Int32Converter(), new GuidConverter() });
                var expectedExceptionMessage =
                    string.Format(
                        "No converter found for the value type ('{2}') of the option '{0}' of the command '{1}'.",
                        expectedName, commandMetadata.Name, typeof(string).FullName);

                // Act
                var actualException =
                    Assert.Throws<CommandLineParserException>(
                        () => propertyInfo.ExtractConverterMetadata(optionMetadata));

                // Assert
                Assert.Equal(expectedExceptionMessage, actualException.Message);
            }

            [Fact]
            public void NullPropertyInfoException()
            {
                // Arrange
                PropertyInfo propertyInfo = null;
                var optionMetadata = new OptionMetadataTemplate(null, null);
                var expectedExceptionMessage = SourceParameterName;

                // Act
                var actualException =
                    Assert.Throws<ArgumentNullException>(
                        () => propertyInfo.ExtractConverterMetadata(optionMetadata));

                // Assert
                Assert.Equal(expectedExceptionMessage, actualException.ParamName);
            }

            [Fact]
            public void NullMetadataException()
            {
                // Arrange
                var propertyInfo =
                    GetType().GetProperty(TypeHelpers.ExtractPropertyName(() => OriginalProperty));
                OptionMetadataTemplate optionMetadataTemplate = null;
                var expectedExceptionMessage = nameof(optionMetadataTemplate);

                // Act
                var actualException =
                    Assert.Throws<ArgumentNullException>(
                        () => propertyInfo.ExtractConverterMetadata(optionMetadataTemplate));

                // Assert
                Assert.Equal(expectedExceptionMessage, actualException.ParamName);
            }
        }
    }
}