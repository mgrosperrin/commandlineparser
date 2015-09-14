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
                var expectedExceptionMessage = Constants.ExceptionMessages.ParserNoConverterFound(expectedName, commandMetadata.Name, typeof(int));

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
                var expectedExceptionMessage = Constants.ExceptionMessages.ParserSpecifiedConverterNotValid(expectedName, commandMetadata.Name, typeof(int), typeof(Guid));

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
                var expectedExceptionMessage = Constants.ExceptionMessages.ParserExtractConverterKeyValueConverterIsForIDictionaryProperty(expectedName, "MyCommand");

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
                var expectedExceptionMessage =Constants.ExceptionMessages.ParserExtractKeyConverterIsNotValid(expectedName, commandMetadata.Name, typeof(string), typeof(int));

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
                var expectedExceptionMessage = Constants.ExceptionMessages.ParserExtractValueConverterIsNotValid(expectedName, commandMetadata.Name, typeof(string), typeof(Guid));

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
                var expectedExceptionMessage = Constants.ExceptionMessages.ParserNoKeyConverterFound(expectedName, commandMetadata.Name, typeof(string));

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
                var expectedExceptionMessage = Constants.ExceptionMessages.ParserNoValueConverterFound(expectedName, commandMetadata.Name, typeof(string));

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