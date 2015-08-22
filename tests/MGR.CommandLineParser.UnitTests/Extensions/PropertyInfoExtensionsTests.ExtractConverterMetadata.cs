using System;
using System.Collections.Generic;
using System.Reflection;
using MGR.CommandLineParser.Command;
using MGR.CommandLineParser.Converters;
using Moq;
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
                string expectedName = TypeHelpers.ExtractPropertyName(() => OriginalProperty);
                PropertyInfo propertyInfo = GetType().GetProperty(expectedName);
                var commandMetadata = new CommandMetadataTemplate { Name = "MyCommand" };
                var optionMetadata = new OptionMetadataTemplate(propertyInfo, commandMetadata) { Name = expectedName };
                DefaultServiceResolver.RegisterServices(() => new List<IConverter> { new Int32Converter() });

                // Act
                OptionMetadataTemplate actual = propertyInfo.ExtractConverterMetadata(optionMetadata);

                // Assert
                Assert.NotNull(actual.Converter);
                Assert.IsType<Int32Converter>(actual.Converter);
            }

            [Fact]
            public void OriginalNoConverterTest()
            {
                // Arrange
                string expectedName = TypeHelpers.ExtractPropertyName(() => OriginalProperty);
                PropertyInfo propertyInfo = GetType().GetProperty(expectedName);
                var commandMetadata = new CommandMetadataTemplate { Name = "MyCommand" };
                var optionMetadata = new OptionMetadataTemplate(propertyInfo, commandMetadata) { Name = expectedName };
                DefaultServiceResolver.RegisterServices(() => new List<IConverter> { new StringConverter() });
                string expectedExceptionMessage =
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
                string expectedName = TypeHelpers.ExtractPropertyName(() => CustomConverterProperty);
                PropertyInfo propertyInfo = GetType().GetProperty(expectedName);
                var commandMetadata = new CommandMetadataTemplate { Name = "MyCommand" };
                var optionMetadata = new OptionMetadataTemplate(propertyInfo, commandMetadata) { Name = expectedName };
                DefaultServiceResolver.RegisterServices(() => new List<IConverter> { new StringConverter() });

                // Act
                OptionMetadataTemplate actual = propertyInfo.ExtractConverterMetadata(optionMetadata);

                // Assert
                Assert.NotNull(actual.Converter);
                Assert.IsType<Int32Converter>(actual.Converter);
            }

            [Fact]
            public void WrongCustomConverterTest()
            {
                // Arrange
                string expectedName = TypeHelpers.ExtractPropertyName(() => WrongCustomConverterProperty);
                PropertyInfo propertyInfo = GetType().GetProperty(expectedName);
                var commandMetadata = new CommandMetadataTemplate { Name = "MyCommand" };
                var optionMetadata = new OptionMetadataTemplate(propertyInfo, commandMetadata) { Name = expectedName };
                DefaultServiceResolver.RegisterServices(() => new List<IConverter> { new StringConverter() });
                string expectedExceptionMessage =
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
                string expectedName = TypeHelpers.ExtractPropertyName(() => OriginalListProperty);
                PropertyInfo propertyInfo = GetType().GetProperty(expectedName);
                var commandMetadata = new CommandMetadataTemplate { Name = "MyCommand" };
                var optionMetadata = new OptionMetadataTemplate(propertyInfo, commandMetadata) { Name = expectedName };
                DefaultServiceResolver.RegisterServices(() => new List<IConverter> { new Int32Converter() });

                // Act
                OptionMetadataTemplate actual = propertyInfo.ExtractConverterMetadata(optionMetadata);

                // Assert
                Assert.NotNull(actual.Converter);
                Assert.IsType<Int32Converter>(actual.Converter);
            }

            [Fact]
            public void CustomListConverterTest()
            {
                // Arrange
                string expectedName = TypeHelpers.ExtractPropertyName(() => CustomListConverterProperty);
                PropertyInfo propertyInfo = GetType().GetProperty(expectedName);
                var commandMetadata = new CommandMetadataTemplate { Name = "MyCommand" };
                var optionMetadata = new OptionMetadataTemplate(propertyInfo, commandMetadata) { Name = expectedName };
                DefaultServiceResolver.RegisterServices(() => new List<IConverter> { new StringConverter() });

                // Act
                OptionMetadataTemplate actual = propertyInfo.ExtractConverterMetadata(optionMetadata);

                // Assert
                Assert.NotNull(actual.Converter);
                Assert.IsType<Int32Converter>(actual.Converter);
            }

            [Fact]
            public void OriginalDictionaryTest()
            {
                // Arrange
                string expectedName = TypeHelpers.ExtractPropertyName(() => OriginalDictionaryProperty);
                PropertyInfo propertyInfo = GetType().GetProperty(expectedName);
                var commandMetadata = new CommandMetadataTemplate { Name = "MyCommand" };
                var optionMetadata = new OptionMetadataTemplate(propertyInfo, commandMetadata) { Name = expectedName };
                DefaultServiceResolver.RegisterServices(() => new List<IConverter> { new Int32Converter(), new GuidConverter() });

                // Act
                OptionMetadataTemplate actual = propertyInfo.ExtractConverterMetadata(optionMetadata);

                // Assert
                Assert.NotNull(actual.Converter);
                Assert.IsType<KeyValueConverter>(actual.Converter);
            }

            [Fact]
            public void CustomDictionaryConverterTest()
            {
                // Arrange
                string expectedName = TypeHelpers.ExtractPropertyName(() => CustomDictionaryConverterProperty);
                PropertyInfo propertyInfo = GetType().GetProperty(expectedName);
                var commandMetadata = new CommandMetadataTemplate { Name = "MyCommand" };
                var optionMetadata = new OptionMetadataTemplate(propertyInfo, commandMetadata) { Name = expectedName };
                DefaultServiceResolver.RegisterServices(() => new List<IConverter> { new StringConverter(), new GuidConverter() });

                // Act
                OptionMetadataTemplate actual = propertyInfo.ExtractConverterMetadata(optionMetadata);

                // Assert
                Assert.NotNull(actual.Converter);
                Assert.IsType<KeyValueConverter>(actual.Converter);
            }

            [Fact]
            public void CustomValueOnlyDictionaryConverterTest()
            {
                // Arrange
                string expectedName = TypeHelpers.ExtractPropertyName(() => CustomValueOnlyDictionaryConverterProperty);
                PropertyInfo propertyInfo = GetType().GetProperty(expectedName);
                var commandMetadata = new CommandMetadataTemplate { Name = "MyCommand" };
                var optionMetadata = new OptionMetadataTemplate(propertyInfo, commandMetadata) { Name = expectedName };
                DefaultServiceResolver.RegisterServices(() => new List<IConverter> { new StringConverter(), new Int32Converter() });

                // Act
                OptionMetadataTemplate actual = propertyInfo.ExtractConverterMetadata(optionMetadata);

                // Assert
                Assert.NotNull(actual.Converter);
                Assert.IsType<KeyValueConverter>(actual.Converter);
            }

            [Fact]
            public void CustomDictionaryConverterWithListException()
            {
                // Arrange
                string expectedName = TypeHelpers.ExtractPropertyName(() => CustomDictionaryConverterWithListProperty);
                PropertyInfo propertyInfo = GetType().GetProperty(expectedName);
                var commandMetadata = new CommandMetadataTemplate { Name = "MyCommand" };
                var optionMetadata = new OptionMetadataTemplate(propertyInfo, commandMetadata) { Name = expectedName };
                DefaultServiceResolver.RegisterServices(() => new List<IConverter> { new StringConverter(), new GuidConverter() });
                string expectedExceptionMessage =
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
                string expectedName =
                    TypeHelpers.ExtractPropertyName(() => CustomDictionaryWithWrongKeyConverterProperty);
                PropertyInfo propertyInfo = GetType().GetProperty(expectedName);
                var commandMetadata = new CommandMetadataTemplate { Name = "MyCommand" };
                var optionMetadata = new OptionMetadataTemplate(propertyInfo, commandMetadata) { Name = expectedName };
                DefaultServiceResolver.RegisterServices(() => new List<IConverter> { new Int32Converter(), new GuidConverter() });
                string expectedExceptionMessage =
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
                string expectedName =
                    TypeHelpers.ExtractPropertyName(() => CustomDictionaryWithWrongValueConverterProperty);
                PropertyInfo propertyInfo = GetType().GetProperty(expectedName);
                var commandMetadata = new CommandMetadataTemplate { Name = "MyCommand" };
                var optionMetadata = new OptionMetadataTemplate(propertyInfo, commandMetadata) { Name = expectedName };
                DefaultServiceResolver.RegisterServices(() => new List<IConverter> { new Int32Converter(), new GuidConverter() });
                string expectedExceptionMessage =
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
                string expectedName =
                    TypeHelpers.ExtractPropertyName(() => OriginalDictionaryWithWrongKeyConverterProperty);
                PropertyInfo propertyInfo = GetType().GetProperty(expectedName);
                var commandMetadata = new CommandMetadataTemplate { Name = "MyCommand" };
                var optionMetadata = new OptionMetadataTemplate(propertyInfo, commandMetadata) { Name = expectedName };
                DefaultServiceResolver.RegisterServices(() => new List<IConverter> { new Int32Converter(), new GuidConverter() });
                string expectedExceptionMessage =
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
                string expectedName =
                    TypeHelpers.ExtractPropertyName(() => OriginalDictionaryWithWrongValueConverterProperty);
                PropertyInfo propertyInfo = GetType().GetProperty(expectedName);
                var commandMetadata = new CommandMetadataTemplate { Name = "MyCommand" };
                var optionMetadata = new OptionMetadataTemplate(propertyInfo, commandMetadata) { Name = expectedName };
                DefaultServiceResolver.RegisterServices(() => new List<IConverter> { new Int32Converter(), new GuidConverter() });
                string expectedExceptionMessage =
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
                string expectedExceptionMessage = @"propertySource";

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
                PropertyInfo propertyInfo =
                    GetType().GetProperty(TypeHelpers.ExtractPropertyName(() => OriginalProperty));
                OptionMetadataTemplate optionMetadata = null;
                string expectedExceptionMessage = @"metadata";

                // Act
                var actualException =
                    Assert.Throws<ArgumentNullException>(
                        () => propertyInfo.ExtractConverterMetadata(optionMetadata));

                // Assert
                Assert.Equal(expectedExceptionMessage, actualException.ParamName);
            }
        }
    }
}