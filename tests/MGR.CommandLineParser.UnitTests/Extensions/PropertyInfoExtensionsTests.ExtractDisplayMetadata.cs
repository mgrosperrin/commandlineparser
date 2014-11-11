using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using MGR.CommandLineParser.Command;
using Xunit;

namespace MGR.CommandLineParser.UnitTests.Extensions
{
    public partial class PropertyInfoExtensionsTests
    {
        public class ExtractDisplayMetadata
        {
            public int OriginalProperty { get; set; }

            [Display(Name = "CustomName")]
            public int CustomNameProperty { get; set; }

            [Display(ShortName = "csnp")]
            public int CustomShortNameProperty { get; set; }

            [Display(Name = "CustomName", ShortName = "csnp")]
            public int CustomNameAndShortNameProperty { get; set; }

            [Display(Name = "CustomName", ShortName = "csnp", Description = "My custom description")]
            public int CustomNameAndShortNameDescriptionProperty { get; set; }

            [Fact]
            public void OriginalTest()
            {
                // Arrange
                string expected = TypeHelpers.ExtractPropertyName(() => OriginalProperty);
                PropertyInfo propertyInfo = GetType().GetProperty(expected);
                var optionMetadata = new OptionMetadataTemplate(null, null);

                // Act
                OptionMetadataTemplate actual = propertyInfo.ExtractDisplayMetadata(optionMetadata);

                // Assert
                Assert.Equal(expected, actual.Name);
                Assert.Equal(expected, actual.ShortName);
                Assert.True(string.IsNullOrEmpty(actual.Description));
            }

            [Fact]
            public void CustomNameTest()
            {
                // Arrange
                string expectedName = "CustomName";
                string expectedShortName = expectedName;
                PropertyInfo propertyInfo =
                    GetType().GetProperty(TypeHelpers.ExtractPropertyName(() => CustomNameProperty));
                var optionMetadata = new OptionMetadataTemplate(null, null);

                // Act
                OptionMetadataTemplate actual = propertyInfo.ExtractDisplayMetadata(optionMetadata);

                // Assert
                Assert.Equal(expectedName, actual.Name);
                Assert.Equal(expectedShortName, actual.ShortName);
                Assert.True(string.IsNullOrEmpty(actual.Description));
            }

            [Fact]
            public void CustomShortNameTest()
            {
                // Arrange
                string expectedName = TypeHelpers.ExtractPropertyName(() => CustomShortNameProperty);
                string expectedShortName = "csnp";
                PropertyInfo propertyInfo = GetType().GetProperty(expectedName);
                var optionMetadata = new OptionMetadataTemplate(null, null);

                // Act
                OptionMetadataTemplate actual = propertyInfo.ExtractDisplayMetadata(optionMetadata);

                // Assert
                Assert.Equal(expectedName, actual.Name);
                Assert.Equal(expectedShortName, actual.ShortName);
                Assert.True(string.IsNullOrEmpty(actual.Description));
            }

            [Fact]
            public void CustomNameAndShortNameTest()
            {
                // Arrange
                string expectedName = "CustomName";
                string expectedShortName = "csnp";
                PropertyInfo propertyInfo =
                    GetType().GetProperty(TypeHelpers.ExtractPropertyName(() => CustomNameAndShortNameProperty));
                var optionMetadata = new OptionMetadataTemplate(null, null);

                // Act
                OptionMetadataTemplate actual = propertyInfo.ExtractDisplayMetadata(optionMetadata);

                // Assert
                Assert.Equal(expectedName, actual.Name);
                Assert.Equal(expectedShortName, actual.ShortName);
                Assert.True(string.IsNullOrEmpty(actual.Description));
            }

            [Fact]
            public void CustomNameShortNameAndDescriptionTest()
            {
                // Arrange
                string expectedName = "CustomName";
                string expectedShortName = "csnp";
                string expectedDescription = "My custom description";
                PropertyInfo propertyInfo =
                    GetType()
                        .GetProperty(TypeHelpers.ExtractPropertyName(() => CustomNameAndShortNameDescriptionProperty));
                var optionMetadata = new OptionMetadataTemplate(null, null);

                // Act
                OptionMetadataTemplate actual = propertyInfo.ExtractDisplayMetadata(optionMetadata);

                // Assert
                Assert.Equal(expectedName, actual.Name);
                Assert.Equal(expectedShortName, actual.ShortName);
                Assert.Equal(expectedDescription, actual.Description);
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
                    Assert.Throws<ArgumentNullException>(() => propertyInfo.ExtractDisplayMetadata(optionMetadata));

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
                    Assert.Throws<ArgumentNullException>(() => propertyInfo.ExtractDisplayMetadata(optionMetadata));

                // Assert
                Assert.Equal(expectedExceptionMessage, actualException.ParamName);
            }
        }
    }
}