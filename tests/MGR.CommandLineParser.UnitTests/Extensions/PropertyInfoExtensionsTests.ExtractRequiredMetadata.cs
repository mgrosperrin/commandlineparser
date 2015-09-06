using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using MGR.CommandLineParser.Command;
using Xunit;

namespace MGR.CommandLineParser.UnitTests.Extensions
{
    public partial class PropertyInfoExtensionsTests
    {
        public class ExtractRequiredMetadata
        {
            public int WritableProperty { get; set; }

            [Required]
            public int WritableIgnoredProperty { get; set; }

            [Fact]
            public void WritableTest()
            {
                // Arrange
                var propertyInfo =
                    GetType().GetProperty(TypeHelpers.ExtractPropertyName(() => WritableProperty));
                var optionMetadata = new OptionMetadataTemplate(null, null);
                var expected = false;

                // Act
                var actual = propertyInfo.ExtractRequiredMetadata(optionMetadata);

                // Assert
                Assert.Equal(expected, actual.IsRequired);
            }

            [Fact]
            public void NonWritableTest()
            {
                // Arrange
                var propertyInfo =
                    GetType().GetProperty(TypeHelpers.ExtractPropertyName(() => WritableIgnoredProperty));
                var optionMetadata = new OptionMetadataTemplate(null, null);
                var expected = true;

                // Act
                var actual = propertyInfo.ExtractRequiredMetadata(optionMetadata);

                // Assert
                Assert.Equal(expected, actual.IsRequired);
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
                    Assert.Throws<ArgumentNullException>(() => propertyInfo.ExtractRequiredMetadata(optionMetadata));

                // Assert
                Assert.Equal(expectedExceptionMessage, actualException.ParamName);
            }

            [Fact]
            public void NullMetadataException()
            {
                // Arrange
                var propertyInfo = GetType().GetProperty(nameof(WritableIgnoredProperty));
                OptionMetadataTemplate optionMetadataTemplate = null;
                var expectedExceptionMessage = nameof(optionMetadataTemplate);

                // Act
                var actualException =
                    Assert.Throws<ArgumentNullException>(() => propertyInfo.ExtractRequiredMetadata(optionMetadataTemplate));

                // Assert
                Assert.Equal(expectedExceptionMessage, actualException.ParamName);
            }
        }
    }
}