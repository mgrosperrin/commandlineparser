using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using MGR.CommandLineParser.Extensibility.ClassBased;
using Xunit;

namespace MGR.CommandLineParser.UnitTests.Extensions
{
    public partial class PropertyInfoExtensionsTests
    {
        public class ExtractOptionDisplayInfoMetadata
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
                var expected = TypeHelpers.ExtractPropertyName(() => OriginalProperty);
                var propertyInfo = GetType().GetProperty(expected);

                // Act
                var actual = propertyInfo.ExtractOptionDisplayInfoMetadata(new List<IPropertyOptionAlternateNameGenerator>());

                // Assert
                Assert.Equal(expected, actual.Name);
                Assert.Equal(expected, actual.ShortName);
                Assert.True(string.IsNullOrEmpty(actual.Description));
            }

            [Fact]
            public void CustomNameTest()
            {
                // Arrange
                var expectedName = "CustomName";
                var expectedShortName = expectedName;
                var propertyInfo =
                    GetType().GetProperty(TypeHelpers.ExtractPropertyName(() => CustomNameProperty));

                // Act
                var actual = propertyInfo.ExtractOptionDisplayInfoMetadata(new List<IPropertyOptionAlternateNameGenerator>());

                // Assert
                Assert.Equal(expectedName, actual.Name);
                Assert.Equal(expectedShortName, actual.ShortName);
                Assert.True(string.IsNullOrEmpty(actual.Description));
            }

            [Fact]
            public void CustomShortNameTest()
            {
                // Arrange
                var expectedName = TypeHelpers.ExtractPropertyName(() => CustomShortNameProperty);
                var expectedShortName = "csnp";
                var propertyInfo = GetType().GetProperty(expectedName);

                // Act
                var actual = propertyInfo.ExtractOptionDisplayInfoMetadata(new List<IPropertyOptionAlternateNameGenerator>());

                // Assert
                Assert.Equal(expectedName, actual.Name);
                Assert.Equal(expectedShortName, actual.ShortName);
                Assert.True(string.IsNullOrEmpty(actual.Description));
            }

            [Fact]
            public void CustomNameAndShortNameTest()
            {
                // Arrange
                var expectedName = "CustomName";
                var expectedShortName = "csnp";
                var propertyInfo =
                    GetType().GetProperty(TypeHelpers.ExtractPropertyName(() => CustomNameAndShortNameProperty));

                // Act
                var actual = propertyInfo.ExtractOptionDisplayInfoMetadata(new List<IPropertyOptionAlternateNameGenerator>());

                // Assert
                Assert.Equal(expectedName, actual.Name);
                Assert.Equal(expectedShortName, actual.ShortName);
                Assert.True(string.IsNullOrEmpty(actual.Description));
            }

            [Fact]
            public void CustomNameShortNameAndDescriptionTest()
            {
                // Arrange
                var expectedName = "CustomName";
                var expectedShortName = "csnp";
                var expectedDescription = "My custom description";
                var propertyInfo =
                    GetType()
                        .GetProperty(TypeHelpers.ExtractPropertyName(() => CustomNameAndShortNameDescriptionProperty));

                // Act
                var actual = propertyInfo.ExtractOptionDisplayInfoMetadata(new List<IPropertyOptionAlternateNameGenerator>());

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
                var expectedExceptionMessage = SourceParameterName;

                // Act
                var actualException =
                    // ReSharper disable once ExpressionIsAlwaysNull
                    Assert.Throws<ArgumentNullException>(() => propertyInfo.ExtractOptionDisplayInfoMetadata(new List<IPropertyOptionAlternateNameGenerator>()));

                // Assert
                Assert.Equal(expectedExceptionMessage, actualException.ParamName);
            }
        }
    }
}