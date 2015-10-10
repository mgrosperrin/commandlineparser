using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using Xunit;

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

                // Act
                var actual = propertyInfo.ExtractDefaultValue(_ => _);

                // Assert
                Assert.Null(actual);
            }

            [Fact]
            public void CustomIntTypeTest()
            {
                // Arrange
                var propertyInfo =
                    GetType().GetProperty(TypeHelpers.ExtractPropertyName(() => CustomIntProperty));
                var expected = 42;

                // Act
                var actual = propertyInfo.ExtractDefaultValue(_ => _);

                // Assert
                Assert.IsType<int>(actual);
                Assert.Equal(expected, actual);
            }

            [Fact]
            public void CustomStringTypeTest()
            {
                // Arrange
                var propertyInfo =
                    GetType().GetProperty(TypeHelpers.ExtractPropertyName(() => CustomStringProperty));
                var expected = "Test";

                // Act
                var actual = propertyInfo.ExtractDefaultValue(_ => _);

                // Assert
                Assert.IsType<string>(actual);
                Assert.Equal(expected, actual);
            }

            [Fact]
            public void CustomGuidTypeTest()
            {
                // Arrange
                var propertyInfo =
                    GetType().GetProperty(TypeHelpers.ExtractPropertyName(() => CustomGuidProperty));
                var expected = new Guid(GuidValue);

                // Act
                var actual = propertyInfo.ExtractDefaultValue(value => Guid.Parse(value.ToString()));

                // Assert
                Assert.IsType<Guid>(actual);
                Assert.Equal(expected, actual);
            }

            [Fact]
            public void CustomStringListTest()
            {
                // Arrange
                var expectedName = TypeHelpers.ExtractPropertyName(() => CustomStringListProperty);
                var propertyInfo = GetType().GetProperty(expectedName);

                // Act
                var actual = propertyInfo.ExtractDefaultValue(_ => _);

                // Assert
                Assert.Null(actual);
            }
        }
    }
}