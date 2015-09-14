using System;
using System.Collections.Generic;
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
            private const string CustomPropertyName = "MyCustomName";
            private const string CustomPropertyShortName = "mcn";
            public int OriginalProperty { get; set; }

            [IgnoreOptionProperty]
            public string IgnoredProperty { get; set; }

            [Required]
            [Display(Name = CustomPropertyName, ShortName = CustomPropertyShortName)]
            [Converter(typeof (StringConverter))]
            public string CustomProperty { get; set; }

            public int NonWritableProperty => 0;

            [Fact]
            public void IgnoredPropertyTests()
            {
                // Arrange
                var expectedName = TypeHelpers.ExtractPropertyName(() => IgnoredProperty);
                var propertyInfo = GetType().GetProperty(expectedName);
                var commandMetadata = new CommandMetadataTemplate {Name = "MyCommand"};

                // Act
                var actual = propertyInfo.ExtractMetadata(commandMetadata);

                // Assert
                Assert.Null(actual);
            }

            [Fact]
            public void CustomPropertyTests()
            {
                // Arrange
                var expectedName = CustomPropertyName;
                var expectedShortName = CustomPropertyShortName;
                var propertyInfo = GetType().GetProperty(TypeHelpers.ExtractPropertyName(() => CustomProperty));
                var commandMetadata = new CommandMetadataTemplate {Name = "MyCommand"};

                // Act
                var actual = propertyInfo.ExtractMetadata(commandMetadata);

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
                var commandMetadata = new CommandMetadataTemplate();
                var expectedExceptionMessage = SourceParameterName;

                // Act
                var actualException =
                    Assert.Throws<ArgumentNullException>(
                        // ReSharper disable once ExpressionIsAlwaysNull
                        () => propertyInfo.ExtractMetadata(commandMetadata));

                // Assert
                Assert.Equal(expectedExceptionMessage, actualException.ParamName);
            }

            [Fact]
            public void NullMetadataException()
            {
                // Arrange
                var propertyInfo =
                    GetType().GetProperty(TypeHelpers.ExtractPropertyName(() => OriginalProperty));
                CommandMetadataTemplate commandMetadataTemplate = null;
                var expectedExceptionMessage = nameof(commandMetadataTemplate);

                // Act
                // ReSharper disable once ExpressionIsAlwaysNull
                var actualException = Assert.Throws<ArgumentNullException>(() => propertyInfo.ExtractMetadata(commandMetadataTemplate));

                // Assert
                Assert.Equal(expectedExceptionMessage, actualException.ParamName);
            }

            [Fact]
            public void NonWritablePropertyInfoException()
            {
                // Arrange
                var propertyInfo =
                    GetType().GetProperty(TypeHelpers.ExtractPropertyName(() => NonWritableProperty));
                var commandMetadata = new CommandMetadataTemplate {Name = "MyCommand"};
                var expectedExceptionMessage = Constants.ExceptionMessages.ParserExtractMetadataPropertyShouldBeWritableOrICollection(nameof(NonWritableProperty), "MyCommand");

                // Act
                var actualException = Assert.Throws<CommandLineParserException>(() => propertyInfo.ExtractMetadata(commandMetadata));

                // Assert
                Assert.Equal(expectedExceptionMessage, actualException.Message);
            }
        }
    }
}