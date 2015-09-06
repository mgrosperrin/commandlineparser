using System;
using System.Collections.Generic;
using Xunit;

namespace MGR.CommandLineParser.UnitTests.Extensions
{
    public partial class TypeExtensionsTests
    {
        public class GetDictionaryType
        {
            [Fact]
            public void ListIntTest()
            {
                // Arrange
                var testedType = typeof (List<int>);
                Type expected = null;

                // Act
                var actual = testedType.GetDictionaryType();

                // Assert
                Assert.Equal(expected, actual);
            }

            [Fact]
            public void ICollectionIntTest()
            {
                // Arrange
                var testedType = typeof (ICollection<int>);
                Type expected = null;

                // Act
                var actual = testedType.GetDictionaryType();

                // Assert
                Assert.Equal(expected, actual);
            }

            [Fact]
            public void TupleInt()
            {
                // Arrange
                var testedType = typeof (Tuple<int>);
                Type expected = null;

                // Act
                var actual = testedType.GetDictionaryType();

                // Assert
                Assert.Equal(expected, actual);
            }

            [Fact]
            public void DictionaryStringInt()
            {
                // Arrange
                var testedType = typeof (Dictionary<string, int>);
                var expected = typeof (IDictionary<string, int>);

                // Act
                var actual = testedType.GetDictionaryType();

                // Assert
                Assert.Equal(expected, actual);
            }

            [Fact]
            public void IDictionaryStringInt()
            {
                // Arrange
                var testedType = typeof (IDictionary<string, int>);
                var expected = typeof (IDictionary<string, int>);

                // Act
                var actual = testedType.GetDictionaryType();

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
                var actualException = Assert.Throws<ArgumentNullException>(() => testedType.GetDictionaryType());

                // Assert
                Assert.Equal(expectedExceptionMessage, actualException.ParamName);
            }
        }
    }
}