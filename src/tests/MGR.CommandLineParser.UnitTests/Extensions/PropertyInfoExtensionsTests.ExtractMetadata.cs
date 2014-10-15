using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using MGR.CommandLineParser.Command;
using MGR.CommandLineParser.Converters;
using Xunit;

namespace MGR.CommandLineParser.UnitTests.Extensions
{
    public partial class PropertyInfoExtensionsTests
    {
        public class ExtractMetadata
        {
            public int OriginalProperty { get; set; }

            [IgnoreOptionProperty]
            public string IgnoredProperty { get; set; }

            [Required]
            [Display(Name = "MyCustomName", ShortName = "mcn")]
            [Converter(typeof (StringConverter))]
            public string CustomProperty { get; set; }

            public int NonWritableProperty
            {
                get { return 0; }
            }

            [Fact]
            public void IgnoredPropertyTests()
            {
                // Arrange
                string expectedName = TypeHelpers.ExtractPropertyName(() => IgnoredProperty);
                PropertyInfo propertyInfo = GetType().GetProperty(expectedName);
                var commandMetadata = new CommandMetadataTemplate {Name = "MyCommand"};
                IParserOptions parserOptions = new ParserOptions();

                // Act
                OptionMetadataTemplate actual = propertyInfo.ExtractMetadata(commandMetadata, parserOptions);

                // Assert
                Assert.Null(actual);
            }

            [Fact]
            public void CustomPropertyTests()
            {
                // Arrange
                string expectedName = "MyCustomName";
                string expectedShortName = "mcn";
                PropertyInfo propertyInfo = GetType().GetProperty(TypeHelpers.ExtractPropertyName(() => CustomProperty));
                var commandMetadata = new CommandMetadataTemplate {Name = "MyCommand"};
                IParserOptions parserOptions = new ParserOptions();

                // Act
                OptionMetadataTemplate actual = propertyInfo.ExtractMetadata(commandMetadata, parserOptions);

                // Assert
                Assert.NotNull(actual);
                Assert.Equal(expectedName, actual.Name);
                Assert.Equal(expectedShortName, actual.ShortName);
                Assert.True(actual.IsRequired);
                Assert.IsType<StringConverter>(actual.Converter);
            }

            [Fact]
            public void NullPropertyInfoException()
            {
                // Arrange
                PropertyInfo propertyInfo = null;
                IParserOptions parserOptions = new ParserOptions();
                var commandMetadata = new CommandMetadataTemplate();
                string expectedExceptionMessage = @"propertySource";

                // Act
                var actualException =
                    Assert.Throws<ArgumentNullException>(
                        () => propertyInfo.ExtractMetadata(commandMetadata, parserOptions));

                // Assert
                Assert.Equal(expectedExceptionMessage, actualException.ParamName);
            }

            [Fact]
            public void NullOptionsException()
            {
                // Arrange
                PropertyInfo propertyInfo =
                    GetType().GetProperty(TypeHelpers.ExtractPropertyName(() => OriginalProperty));
                IParserOptions parserOptions = null;
                var commandMetadata = new CommandMetadataTemplate();
                string expectedExceptionMessage = @"options";

                // Act
                var actualException =
                    Assert.Throws<ArgumentNullException>(
                        () => propertyInfo.ExtractMetadata(commandMetadata, parserOptions));

                // Assert
                Assert.Equal(expectedExceptionMessage, actualException.ParamName);
            }

            [Fact]
            public void NullMetadataException()
            {
                // Arrange
                PropertyInfo propertyInfo =
                    GetType().GetProperty(TypeHelpers.ExtractPropertyName(() => OriginalProperty));
                IParserOptions parserOptions = new ParserOptions();
                CommandMetadataTemplate commandMetadata = null;
                string expectedExceptionMessage = @"commandMetadataTemplate";

                // Act
                var actualException =
                    Assert.Throws<ArgumentNullException>(
                        () => propertyInfo.ExtractMetadata(commandMetadata, parserOptions));

                // Assert
                Assert.Equal(expectedExceptionMessage, actualException.ParamName);
            }

            [Fact]
            public void NonWritablePropertyInfoException()
            {
                // Arrange
                PropertyInfo propertyInfo =
                    GetType().GetProperty(TypeHelpers.ExtractPropertyName(() => NonWritableProperty));
                IParserOptions parserOptions = new ParserOptions();
                var commandMetadata = new CommandMetadataTemplate {Name = "MyCommand"};
                string expectedExceptionMessage =
                    "The option 'NonWritableProperty' of the command 'MyCommand' must be writable or implements ICollection<T>.";

                // Act
                var actualException =
                    Assert.Throws<CommandLineParserException>(
                        () => propertyInfo.ExtractMetadata(commandMetadata, parserOptions));

                // Assert
                Assert.Equal(expectedExceptionMessage, actualException.Message);
            }
        }
    }
}