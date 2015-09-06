using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using MGR.CommandLineParser.Command;
using Xunit;
using GuidConverter = MGR.CommandLineParser.Converters.GuidConverter;
using Int32Converter = MGR.CommandLineParser.Converters.Int32Converter;

namespace MGR.CommandLineParser.UnitTests.Extensions
{
    public partial class PropertyInfoExtensionsTests
    {
        public class ExtractDefaultValue
        {
            private const string GuidValue = "0DCC5598-E071-47ED-9BC1-5E4C3E923A49";
            public int OriginalProperty { get; set; }

            [Display(Name = "CustomName")]
            [DefaultValue(42)]
            public int CustomIntProperty { get; set; }

            [DefaultValue("Test")]
            public string CustomStringProperty { get; set; }

            [DefaultValue(GuidValue)]
            public Guid CustomGuidProperty { get; set; }

            [DefaultValue("Test|Test2")]
            public List<string> CustomStringListProperty { get; set; }

            [Fact]
            public void OriginalTest()
            {
                // Arrange
                PropertyInfo propertyInfo =
                    GetType().GetProperty(TypeHelpers.ExtractPropertyName(() => OriginalProperty));
                var optionMetadata = new OptionMetadataTemplate(null, null);

                // Act
                OptionMetadataTemplate actual = propertyInfo.ExtractDefaultValue(optionMetadata);

                // Assert
                Assert.Null(actual.DefaultValue);
            }

            [Fact]
            public void CustomIntTypeTest()
            {
                // Arrange
                PropertyInfo propertyInfo =
                    GetType().GetProperty(TypeHelpers.ExtractPropertyName(() => CustomIntProperty));
                var optionMetadata = new OptionMetadataTemplate(propertyInfo, null);
                optionMetadata.Converter = new Int32Converter();
                int expected = 42;

                // Act
                OptionMetadataTemplate actual = propertyInfo.ExtractDefaultValue(optionMetadata);

                // Assert
                Assert.Equal(expected, actual.DefaultValue);
            }

            [Fact]
            public void CustomStringTypeTest()
            {
                // Arrange
                PropertyInfo propertyInfo =
                    GetType().GetProperty(TypeHelpers.ExtractPropertyName(() => CustomStringProperty));
                var optionMetadata = new OptionMetadataTemplate(propertyInfo, null);
                string expected = "Test";

                // Act
                OptionMetadataTemplate actual = propertyInfo.ExtractDefaultValue(optionMetadata);

                // Assert
                Assert.Equal(expected, actual.DefaultValue);
            }

            [Fact]
            public void CustomGuidTypeTest()
            {
                // Arrange
                PropertyInfo propertyInfo =
                    GetType().GetProperty(TypeHelpers.ExtractPropertyName(() => CustomGuidProperty));
                var optionMetadata = new OptionMetadataTemplate(propertyInfo, null);
                optionMetadata.Converter = new GuidConverter();
                var expected = new Guid(GuidValue);

                // Act
                OptionMetadataTemplate actual = propertyInfo.ExtractDefaultValue(optionMetadata);

                // Assert
                Assert.Equal(expected, actual.DefaultValue);
            }

            [Fact]
            public void CustomStringListTest()
            {
                // Arrange
                string expectedName = TypeHelpers.ExtractPropertyName(() => CustomStringListProperty);
                PropertyInfo propertyInfo = GetType().GetProperty(expectedName);
                var optionMetadata = new OptionMetadataTemplate(propertyInfo, null);

                // Act
                OptionMetadataTemplate actual = propertyInfo.ExtractDefaultValue(optionMetadata);

                // Assert
                Assert.Null(actual.DefaultValue);
            }

            [Fact]
            public void NullPropertyInfoException()
            {
                // Arrange
                PropertyInfo propertyInfo = null;
                var optionMetadata = new OptionMetadataTemplate(null, null);
                string expectedExceptionMessage = SourceParameterName;

                // Act
                var actualException =
                    Assert.Throws<ArgumentNullException>(() => propertyInfo.ExtractDefaultValue(optionMetadata));

                // Assert
                Assert.Equal(expectedExceptionMessage, actualException.ParamName);
            }

            [Fact]
            public void NullMetadataException()
            {
                // Arrange
                PropertyInfo propertyInfo =
                    GetType().GetProperty(TypeHelpers.ExtractPropertyName(() => OriginalProperty));
                OptionMetadataTemplate optionMetadataTemplate = null;
                string expectedExceptionMessage = nameof(optionMetadataTemplate);

                // Act
                var actualException =
                    Assert.Throws<ArgumentNullException>(() => propertyInfo.ExtractDefaultValue(optionMetadataTemplate));

                // Assert
                Assert.Equal(expectedExceptionMessage, actualException.ParamName);
            }
        }
    }
}