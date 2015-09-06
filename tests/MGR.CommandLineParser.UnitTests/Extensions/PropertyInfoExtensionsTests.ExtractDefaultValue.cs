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
                var propertyInfo =
                    GetType().GetProperty(TypeHelpers.ExtractPropertyName(() => OriginalProperty));
                var optionMetadata = new OptionMetadataTemplate(null, null);

                // Act
                var actual = propertyInfo.ExtractDefaultValue(optionMetadata);

                // Assert
                Assert.Null(actual.DefaultValue);
            }

            [Fact]
            public void CustomIntTypeTest()
            {
                // Arrange
                var propertyInfo =
                    GetType().GetProperty(TypeHelpers.ExtractPropertyName(() => CustomIntProperty));
                var optionMetadata = new OptionMetadataTemplate(propertyInfo, null)
                {
                    Converter = new Int32Converter()
                };
                var expected = 42;

                // Act
                var actual = propertyInfo.ExtractDefaultValue(optionMetadata);

                // Assert
                Assert.Equal(expected, actual.DefaultValue);
            }

            [Fact]
            public void CustomStringTypeTest()
            {
                // Arrange
                var propertyInfo =
                    GetType().GetProperty(TypeHelpers.ExtractPropertyName(() => CustomStringProperty));
                var optionMetadata = new OptionMetadataTemplate(propertyInfo, null);
                var expected = "Test";

                // Act
                var actual = propertyInfo.ExtractDefaultValue(optionMetadata);

                // Assert
                Assert.Equal(expected, actual.DefaultValue);
            }

            [Fact]
            public void CustomGuidTypeTest()
            {
                // Arrange
                var propertyInfo =
                    GetType().GetProperty(TypeHelpers.ExtractPropertyName(() => CustomGuidProperty));
                var optionMetadata = new OptionMetadataTemplate(propertyInfo, null)
                {
                    Converter = new GuidConverter()
                };
                var expected = new Guid(GuidValue);

                // Act
                var actual = propertyInfo.ExtractDefaultValue(optionMetadata);

                // Assert
                Assert.Equal(expected, actual.DefaultValue);
            }

            [Fact]
            public void CustomStringListTest()
            {
                // Arrange
                var expectedName = TypeHelpers.ExtractPropertyName(() => CustomStringListProperty);
                var propertyInfo = GetType().GetProperty(expectedName);
                var optionMetadata = new OptionMetadataTemplate(propertyInfo, null);

                // Act
                var actual = propertyInfo.ExtractDefaultValue(optionMetadata);

                // Assert
                Assert.Null(actual.DefaultValue);
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
                    Assert.Throws<ArgumentNullException>(() => propertyInfo.ExtractDefaultValue(optionMetadata));

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
                    Assert.Throws<ArgumentNullException>(() => propertyInfo.ExtractDefaultValue(optionMetadataTemplate));

                // Assert
                Assert.Equal(expectedExceptionMessage, actualException.ParamName);
            }
        }
    }
}