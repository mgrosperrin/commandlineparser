using System.Reflection;
using MGR.CommandLineParser.Command;
using MGR.CommandLineParser.Extensibility.Converters;
using Xunit;

namespace MGR.CommandLineParser.UnitTests.Extensions;

public partial class PropertyInfoExtensionsTests
{
    public class ExtractConverterMetadata
    {
        public int OriginalProperty { get; set; }

#pragma warning disable CS0618 // Type or member is obsolete
        [Converter(typeof(Int32Converter))]
#pragma warning restore CS0618 // Type or member is obsolete
        public int CustomConverterProperty { get; set; }
#if NET7_0_OR_GREATER
        [Converter<Int32Converter>]
        public int CustomGenericConverterProperty { get; set; }
#endif
#pragma warning disable CS0618 // Type or member is obsolete
        [Converter(typeof(GuidConverter))]
#pragma warning restore CS0618 // Type or member is obsolete
        public int WrongCustomConverterProperty { get; set; }

        public List<int> OriginalListProperty { get; set; }

#pragma warning disable CS0618 // Type or member is obsolete
        [Converter(typeof(Int32Converter))]
#pragma warning restore CS0618 // Type or member is obsolete
        public List<int> CustomListConverterProperty { get; set; }

        public Dictionary<int, Guid> OriginalDictionaryProperty { get; set; }

#pragma warning disable CS0618 // Type or member is obsolete
        [ConverterKeyValue(typeof(GuidConverter), typeof(Int32Converter))]
#pragma warning restore CS0618 // Type or member is obsolete
        public Dictionary<int, Guid> CustomDictionaryConverterProperty { get; set; }

        [ConverterKeyValue<GuidConverter, Int32Converter>]
        public Dictionary<int, Guid> CustomDictionaryConverterGenericProperty { get; set; }

#pragma warning disable CS0618 // Type or member is obsolete
        [ConverterKeyValue(typeof(GuidConverter))]
#pragma warning restore CS0618 // Type or member is obsolete
        public Dictionary<string, Guid> CustomValueOnlyDictionaryConverterProperty { get; set; }

        [ConverterKeyValue<GuidConverter>]
        public Dictionary<string, Guid> CustomValueOnlyDictionaryConverterGenericProperty { get; set; }

#pragma warning disable CS0618 // Type or member is obsolete
        [ConverterKeyValue(typeof(GuidConverter))]
#pragma warning restore CS0618 // Type or member is obsolete
        public List<string> CustomDictionaryConverterWithListProperty { get; set; }

#pragma warning disable CS0618 // Type or member is obsolete
        [ConverterKeyValue(typeof(GuidConverter), typeof(Int32Converter))]
#pragma warning restore CS0618 // Type or member is obsolete
        public Dictionary<string, Guid> CustomDictionaryWithWrongKeyConverterProperty { get; set; }

#pragma warning disable CS0618 // Type or member is obsolete
        [ConverterKeyValue(typeof(GuidConverter), typeof(Int32Converter))]
#pragma warning restore CS0618 // Type or member is obsolete
        public Dictionary<int, string> CustomDictionaryWithWrongValueConverterProperty { get; set; }

        public Dictionary<string, Guid> OriginalDictionaryWithWrongKeyConverterProperty { get; set; }
        public Dictionary<int, string> OriginalDictionaryWithWrongValueConverterProperty { get; set; }

        [Fact]
        public void OriginalTest()
        {
            // Arrange
            var propertyName = TypeHelpers.ExtractPropertyName(() => OriginalProperty);
            var propertyInfo = GetType().GetProperty(propertyName);
            var commandName = "MyCommand";
            var converters = new List<IConverter> { new Int32Converter() };

            // Act
            var actual = propertyInfo.ExtractConverter(converters, propertyName, commandName);

            // Assert
            Assert.NotNull(actual);
            Assert.IsType<Int32Converter>(actual);
        }

        [Fact]
        public void OriginalNoConverterTest()
        {
            // Arrange
            var propertyName = TypeHelpers.ExtractPropertyName(() => OriginalProperty);
            var propertyInfo = GetType().GetProperty(propertyName);
            var commandName = "MyCommand";
            var converters = new List<IConverter> { new StringConverter() };
            var expectedExceptionMessage = Constants.ExceptionMessages.ParserNoConverterFound(propertyName, commandName, typeof(int));

            // Act
            var actualException =
                Assert.Throws<CommandLineParserException>(
                    () => propertyInfo.ExtractConverter(converters, propertyName, commandName));

            // Assert
            Assert.Equal(expectedExceptionMessage, actualException.Message);
        }

        [Fact]
        public void CustomConverterTest()
        {
            // Arrange
            var propertyName = TypeHelpers.ExtractPropertyName(() => CustomConverterProperty);
            var propertyInfo = GetType().GetProperty(propertyName);
            var commandName = "MyCommand";
            var converters = new List<IConverter> { new StringConverter() };

            // Act
            var actual = propertyInfo.ExtractConverter(converters, propertyName, commandName);

            // Assert
            Assert.NotNull(actual);
            Assert.IsType<Int32Converter>(actual);
        }
#if NET7_0_OR_GREATER
        [Fact]
        public void CustomGenericConverterTest()
        {
            // Arrange
            var propertyName = TypeHelpers.ExtractPropertyName(() => CustomGenericConverterProperty);
            var propertyInfo = GetType().GetProperty(propertyName);
            var commandName = "MyCommand";
            var converters = new List<IConverter> { new StringConverter() };

            // Act
            var actual = propertyInfo.ExtractConverter(converters, propertyName, commandName);

            // Assert
            Assert.NotNull(actual);
            Assert.IsType<Int32Converter>(actual);
        }

        [Fact]
        public void CustomDictionaryGenericConverterTest()
        {
            // Arrange
            var propertyName = TypeHelpers.ExtractPropertyName(() => CustomDictionaryConverterGenericProperty);
            var propertyInfo = GetType().GetProperty(propertyName);
            var commandName = "MyCommand";
            var converters = new List<IConverter> { new StringConverter(), new GuidConverter() };

            // Act
            var actual = propertyInfo.ExtractConverter(converters, propertyName, commandName);

            // Assert
            Assert.NotNull(actual);
            Assert.IsType<KeyValueConverter>(actual);
        }

        [Fact]
        public void CustomValueOnlyDictionaryGenericConverterTest()
        {
            // Arrange
            var propertyName = TypeHelpers.ExtractPropertyName(() => CustomValueOnlyDictionaryConverterGenericProperty);
            var propertyInfo = GetType().GetProperty(propertyName);
            var commandName = "MyCommand";
            var converters = new List<IConverter> { new StringConverter(), new Int32Converter() };

            // Act
            var actual = propertyInfo.ExtractConverter(converters, propertyName, commandName);

            // Assert
            Assert.NotNull(actual);
            Assert.IsType<KeyValueConverter>(actual);
        }
#endif

        [Fact]
        public void WrongCustomConverterTest()
        {
            // Arrange
            var propertyName = TypeHelpers.ExtractPropertyName(() => WrongCustomConverterProperty);
            var propertyInfo = GetType().GetProperty(propertyName);
            var commandName = "MyCommand";
            var converters = new List<IConverter> { new StringConverter() };
            var expectedExceptionMessage = Constants.ExceptionMessages.ParserSpecifiedConverterNotValid(propertyName, commandName, typeof(int), typeof(Guid));

            // Act
            var actualException =
                Assert.Throws<CommandLineParserException>(
                    () => propertyInfo.ExtractConverter(converters, propertyName, commandName));

            // Assert
            Assert.Equal(expectedExceptionMessage, actualException.Message);
        }

        [Fact]
        public void OriginalListTest()
        {
            // Arrange
            var propertyName = TypeHelpers.ExtractPropertyName(() => OriginalListProperty);
            var propertyInfo = GetType().GetProperty(propertyName);
            var commandName = "MyCommand";
            var converters = new List<IConverter> { new Int32Converter() };

            // Act
            var actual = propertyInfo.ExtractConverter(converters, propertyName, commandName);

            // Assert
            Assert.NotNull(actual);
            Assert.IsType<Int32Converter>(actual);
        }

        [Fact]
        public void CustomListConverterTest()
        {
            // Arrange
            var propertyName = TypeHelpers.ExtractPropertyName(() => CustomListConverterProperty);
            var propertyInfo = GetType().GetProperty(propertyName);
            var commandName = "MyCommand";
            var converters = new List<IConverter> { new StringConverter() };

            // Act
            var actual = propertyInfo.ExtractConverter(converters, propertyName, commandName);

            // Assert
            Assert.NotNull(actual);
            Assert.IsType<Int32Converter>(actual);
        }

        [Fact]
        public void OriginalDictionaryTest()
        {
            // Arrange
            var propertyName = TypeHelpers.ExtractPropertyName(() => OriginalDictionaryProperty);
            var propertyInfo = GetType().GetProperty(propertyName);
            var commandName = "MyCommand";
            var converters = new List<IConverter> { new Int32Converter(), new GuidConverter() };

            // Act
            var actual = propertyInfo.ExtractConverter(converters, propertyName, commandName);

            // Assert
            Assert.NotNull(actual);
            Assert.IsType<KeyValueConverter>(actual);
        }

        [Fact]
        public void CustomDictionaryConverterTest()
        {
            // Arrange
            var propertyName = TypeHelpers.ExtractPropertyName(() => CustomDictionaryConverterProperty);
            var propertyInfo = GetType().GetProperty(propertyName);
            var commandName = "MyCommand";
            var converters = new List<IConverter> { new StringConverter(), new GuidConverter() };

            // Act
            var actual = propertyInfo.ExtractConverter(converters, propertyName, commandName);

            // Assert
            Assert.NotNull(actual);
            Assert.IsType<KeyValueConverter>(actual);
        }

        [Fact]
        public void CustomValueOnlyDictionaryConverterTest()
        {
            // Arrange
            var propertyName = TypeHelpers.ExtractPropertyName(() => CustomValueOnlyDictionaryConverterProperty);
            var propertyInfo = GetType().GetProperty(propertyName);
            var commandName = "MyCommand";
            var converters = new List<IConverter> { new StringConverter(), new Int32Converter() };

            // Act
            var actual = propertyInfo.ExtractConverter(converters, propertyName, commandName);

            // Assert
            Assert.NotNull(actual);
            Assert.IsType<KeyValueConverter>(actual);
        }

        [Fact]
        public void CustomDictionaryConverterWithListException()
        {
            // Arrange
            var propertyName = TypeHelpers.ExtractPropertyName(() => CustomDictionaryConverterWithListProperty);
            var propertyInfo = GetType().GetProperty(propertyName);
            var commandName = "MyCommand";
            var converters = new List<IConverter> { new StringConverter(), new GuidConverter() };
            var expectedExceptionMessage = Constants.ExceptionMessages.ParserExtractConverterKeyValueConverterIsForIDictionaryProperty(propertyName, "MyCommand");

            // Act
            var actualException =
                Assert.Throws<CommandLineParserException>(
                    () => propertyInfo.ExtractConverter(converters, propertyName, commandName));

            // Assert
            Assert.Equal(expectedExceptionMessage, actualException.Message);
        }

        [Fact]
        public void CustomDictionaryWithWrongKeyConverterException()
        {
            // Arrange
            var propertyName =
                TypeHelpers.ExtractPropertyName(() => CustomDictionaryWithWrongKeyConverterProperty);
            var propertyInfo = GetType().GetProperty(propertyName);
            var commandName = "MyCommand";
            var converters = new List<IConverter> { new Int32Converter(), new GuidConverter() };
            var expectedExceptionMessage = Constants.ExceptionMessages.ParserExtractKeyConverterIsNotValid(propertyName, commandName, typeof(string), typeof(int));

            // Act
            var actualException =
                Assert.Throws<CommandLineParserException>(
                    () => propertyInfo.ExtractConverter(converters, propertyName, commandName));

            // Assert
            Assert.Equal(expectedExceptionMessage, actualException.Message);
        }

        [Fact]
        public void CustomDictionaryWithWrongValueConverterException()
        {
            // Arrange
            var propertyName =
                TypeHelpers.ExtractPropertyName(() => CustomDictionaryWithWrongValueConverterProperty);
            var propertyInfo = GetType().GetProperty(propertyName);
            var commandName = "MyCommand";
            var converters = new List<IConverter> { new Int32Converter(), new GuidConverter() };
            var expectedExceptionMessage = Constants.ExceptionMessages.ParserExtractValueConverterIsNotValid(propertyName, commandName, typeof(string), typeof(Guid));

            // Act
            var actualException =
                Assert.Throws<CommandLineParserException>(
                    () => propertyInfo.ExtractConverter(converters, propertyName, commandName));

            // Assert
            Assert.Equal(expectedExceptionMessage, actualException.Message);
        }

        [Fact]
        public void OriginalDictionaryWithoutKeyConverterException()
        {
            // Arrange
            var propertyName =
                TypeHelpers.ExtractPropertyName(() => OriginalDictionaryWithWrongKeyConverterProperty);
            var propertyInfo = GetType().GetProperty(propertyName);
            var commandName = "MyCommand";
            var converters = new List<IConverter> { new Int32Converter(), new GuidConverter() };
            var expectedExceptionMessage = Constants.ExceptionMessages.ParserNoKeyConverterFound(propertyName, commandName, typeof(string));

            // Act
            var actualException =
                Assert.Throws<CommandLineParserException>(
                    () => propertyInfo.ExtractConverter(converters, propertyName, commandName));

            // Assert
            Assert.Equal(expectedExceptionMessage, actualException.Message);
        }

        [Fact]
        public void OriginalDictionaryWithoutValueConverterException()
        {
            // Arrange
            var propertyName =
                TypeHelpers.ExtractPropertyName(() => OriginalDictionaryWithWrongValueConverterProperty);
            var propertyInfo = GetType().GetProperty(propertyName);
            var commandName = "MyCommand";
            var converters = new List<IConverter> { new Int32Converter(), new GuidConverter() };
            var expectedExceptionMessage = Constants.ExceptionMessages.ParserNoValueConverterFound(propertyName, commandName, typeof(string));

            // Act
            var actualException =
                Assert.Throws<CommandLineParserException>(
                    () => propertyInfo.ExtractConverter(converters, propertyName, commandName));

            // Assert
            Assert.Equal(expectedExceptionMessage, actualException.Message);
        }

        [Fact]
        public void NullPropertyInfoException()
        {
            // Arrange
            PropertyInfo propertyInfo = null;
            var converters = new List<IConverter> { new Int32Converter(), new GuidConverter() };
            var expectedExceptionMessage = SourceParameterName;

            // Act
            var actualException =
                Assert.Throws<ArgumentNullException>(
                    () => propertyInfo.ExtractConverter(converters, null, null));

            // Assert
            Assert.Equal(expectedExceptionMessage, actualException.ParamName);
        }

        [Fact]
        public void NullOptionNameException()
        {
            // Arrange
            var propertyInfo = GetType().GetProperty(TypeHelpers.ExtractPropertyName(() => OriginalProperty));
            var converters = new List<IConverter> { new Int32Converter(), new GuidConverter() };
            string optionName = null;
            string commandName = null;
            var expectedExceptionMessage = nameof(optionName);

            // Act
            var actualException =
                Assert.Throws<ArgumentNullException>(
                    () => propertyInfo.ExtractConverter(converters, optionName, commandName));

            // Assert
            Assert.Equal(expectedExceptionMessage, actualException.ParamName);
        }

        [Fact]
        public void NullMetadataException()
        {
            // Arrange
            var propertyName = TypeHelpers.ExtractPropertyName(() => OriginalProperty);
            var propertyInfo = GetType().GetProperty(TypeHelpers.ExtractPropertyName(() => OriginalProperty));
            var converters = new List<IConverter> { new Int32Converter(), new GuidConverter() };
            string commandName = null;
            var expectedExceptionMessage = nameof(commandName);

            // Act
            var actualException =
                Assert.Throws<ArgumentNullException>(
                    () => propertyInfo.ExtractConverter(converters, propertyName, commandName));

            // Assert
            Assert.Equal(expectedExceptionMessage, actualException.ParamName);
        }
    }
}