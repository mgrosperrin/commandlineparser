using System;
using System.Collections.Generic;
using Xunit;

namespace MGR.CommandLineParser.UnitTests.Extensions
{
    public partial class TypeExtensionsTests
    {
        public class GetUnderlyingCollectionType
        {
            [Fact]
            public void ListIntTest()
            {
                // Arrange
                var testedType = typeof (List<int>);
                var expected = typeof (int);

                // Act
                var actual = testedType.GetUnderlyingCollectionType();

                // Assert
                Assert.Equal(expected, actual);
            }

            [Fact]
            public void ICollectionIntTest()
            {
                // Arrange
                var testedType = typeof (ICollection<int>);
                var expected = typeof (int);

                // Act
                var actual = testedType.GetUnderlyingCollectionType();

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
                var actual = testedType.GetUnderlyingCollectionType();

                // Assert
                Assert.Equal(expected, actual);
            }

            [Fact]
            public void DictionaryStringInt()
            {
                // Arrange
                var testedType = typeof (Dictionary<string, int>);
                var expected = typeof (KeyValuePair<string, int>);

                // Act
                var actual = testedType.GetUnderlyingCollectionType();

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
                    Assert.Throws<ArgumentNullException>(() => testedType.GetUnderlyingCollectionType());

                // Assert
                Assert.Equal(expectedExceptionMessage, actualException.ParamName);
            }
        }
    }
}