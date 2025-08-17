using Xunit;

namespace MGR.CommandLineParser.UnitTests.Extensions;

public partial class TypeExtensionsTests
{
    public class GetUnderlyingDictionaryType
    {
        [Fact]
        public void DictionaryStringIntKeyTest()
        {
            // Arrange
            var testedType = typeof(Dictionary<string, int>);
            var expected = typeof(string);

            // Act
            var actual = testedType.GetUnderlyingDictionaryType(true);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void IDictionaryStringIntKeyTest()
        {
            // Arrange
            var testedType = typeof(IDictionary<string, int>);
            var expected = typeof(string);

            // Act
            var actual = testedType.GetUnderlyingDictionaryType(true);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void DictionaryStringIntValueTest()
        {
            // Arrange
            var testedType = typeof(Dictionary<string, int>);
            var expected = typeof(int);

            // Act
            var actual = testedType.GetUnderlyingDictionaryType(false);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void IDictionaryStringIntValueTest()
        {
            // Arrange
            var testedType = typeof(IDictionary<string, int>);
            var expected = typeof(int);

            // Act
            var actual = testedType.GetUnderlyingDictionaryType(false);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void TupleIntKeyTest()
        {
            // Arrange
            var testedType = typeof(Tuple<int>);
            Type expected = null;

            // Act
            var actual = testedType.GetUnderlyingDictionaryType(true);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void TupleIntValueTest()
        {
            // Arrange
            var testedType = typeof(Tuple<int>);
            Type expected = null;

            // Act
            var actual = testedType.GetUnderlyingDictionaryType(false);

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void NullTypeException()
        {
            // Arrange
            Type testedType = null;
            var expectedExceptionMessage = SourceParameterName;

            // Act
            var actualException =
                Assert.Throws<ArgumentNullException>(() => testedType.GetUnderlyingDictionaryType(true));

            // Assert
            Assert.Equal(expectedExceptionMessage, actualException.ParamName);
        }
    }
}