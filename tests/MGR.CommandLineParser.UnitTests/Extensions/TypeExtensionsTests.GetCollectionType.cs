using Xunit;

namespace MGR.CommandLineParser.UnitTests.Extensions;

public partial class TypeExtensionsTests
{
    public class GetCollectionType
    {
        [Fact]
        public void ListIntTest()
        {
            // Arrange
            var testedType = typeof(List<int>);
            var expected = typeof(ICollection<int>);

            // Act
            var actual = testedType.GetCollectionType();

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void ICollectionIntTest()
        {
            // Arrange
            var testedType = typeof(ICollection<int>);
            var expected = typeof(ICollection<int>);

            // Act
            var actual = testedType.GetCollectionType();

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void TupleInt()
        {
            // Arrange
            var testedType = typeof(Tuple<int>);
            Type expected = null;

            // Act
            var actual = testedType.GetCollectionType();

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void DictionaryStringInt()
        {
            // Arrange
            var testedType = typeof(Dictionary<string, int>);
            var expected = typeof(ICollection<KeyValuePair<string, int>>);

            // Act
            var actual = testedType.GetCollectionType();

            // Assert
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void IDictionaryStringInt()
        {
            // Arrange
            var testedType = typeof(IDictionary<string, int>);
            var expected = typeof(ICollection<KeyValuePair<string, int>>);

            // Act
            var actual = testedType.GetCollectionType();

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
            var actualException = Assert.Throws<ArgumentNullException>(() => testedType.GetCollectionType());

            // Assert
            Assert.Equal(expectedExceptionMessage, actualException.ParamName);
        }
    }
}