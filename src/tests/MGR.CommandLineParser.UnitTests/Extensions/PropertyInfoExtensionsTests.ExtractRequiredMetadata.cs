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
                PropertyInfo propertyInfo =
                    GetType().GetProperty(TypeHelpers.ExtractPropertyName(() => WritableProperty));
                var optionMetadata = new OptionMetadataTemplate(null, null);
                bool expected = false;

                // Act
                OptionMetadataTemplate actual = propertyInfo.ExtractRequiredMetadata(optionMetadata);

                // Assert
                Assert.Equal(expected, actual.IsRequired);
            }

            [Fact]
            public void NonWritableTest()
            {
                // Arrange
                PropertyInfo propertyInfo =
                    GetType().GetProperty(TypeHelpers.ExtractPropertyName(() => WritableIgnoredProperty));
                var optionMetadata = new OptionMetadataTemplate(null, null);
                bool expected = true;

                // Act
                OptionMetadataTemplate actual = propertyInfo.ExtractRequiredMetadata(optionMetadata);

                // Assert
                Assert.Equal(expected, actual.IsRequired);
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
                    Assert.Throws<ArgumentNullException>(() => propertyInfo.ExtractRequiredMetadata(optionMetadata));

                // Assert
                Assert.Equal(expectedExceptionMessage, actualException.ParamName);
            }

            [Fact]
            public void NullMetadataException()
            {
                // Arrange
                PropertyInfo propertyInfo = GetType().GetProperty("WritableIgnoredProperty");
                OptionMetadataTemplate optionMetadata = null;
                string expectedExceptionMessage = @"metadata";

                // Act
                var actualException =
                    Assert.Throws<ArgumentNullException>(() => propertyInfo.ExtractRequiredMetadata(optionMetadata));

                // Assert
                Assert.Equal(expectedExceptionMessage, actualException.ParamName);
            }
        }
    }
}