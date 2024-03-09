using System.ComponentModel.DataAnnotations;
using System.Reflection;
using Xunit;

namespace MGR.CommandLineParser.UnitTests.Extensions;

public partial class PropertyInfoExtensionsTests
{
    public class ExtractIsRequiredMetadata
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

            // Act
            var actual = propertyInfo.ExtractIsRequiredMetadata();

            // Assert
            Assert.False(actual);
        }

        [Fact]
        public void NonWritableTest()
        {
            // Arrange
            var propertyInfo =
                GetType().GetProperty(TypeHelpers.ExtractPropertyName(() => WritableIgnoredProperty));

            // Act
            var actual = propertyInfo.ExtractIsRequiredMetadata();

            // Assert
            Assert.True(actual);
        }

        [Fact]
        public void NullPropertyInfoException()
        {
            // Arrange
            PropertyInfo propertyInfo = null;
            var expectedExceptionMessage = SourceParameterName;

            // Act
            var actualException =
                Assert.Throws<ArgumentNullException>(() => propertyInfo.ExtractIsRequiredMetadata());

            // Assert
            Assert.Equal(expectedExceptionMessage, actualException.ParamName);
        }
    }
}