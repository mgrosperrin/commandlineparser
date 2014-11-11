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
                Type testedType = typeof (List<int>);
                Type expected = typeof (int);

                // Act
                Type actual = testedType.GetUnderlyingCollectionType();

                // Assert
                Assert.Equal(expected, actual);
            }

            [Fact]
            public void ICollectionIntTest()
            {
                // Arrange
                Type testedType = typeof (ICollection<int>);
                Type expected = typeof (int);

                // Act
                Type actual = testedType.GetUnderlyingCollectionType();

                // Assert
                Assert.Equal(expected, actual);
            }

            [Fact]
            public void TupleInt()
            {
                // Arrange
                Type testedType = typeof (Tuple<int>);
                Type expected = null;

                // Act
                Type actual = testedType.GetUnderlyingCollectionType();

                // Assert
                Assert.Equal(expected, actual);
            }

            [Fact]
            public void DictionaryStringInt()
            {
                // Arrange
                Type testedType = typeof (Dictionary<string, int>);
                Type expected = typeof (KeyValuePair<string, int>);

                // Act
                Type actual = testedType.GetUnderlyingCollectionType();

                // Assert
                Assert.Equal(expected, actual);
            }

            [Fact]
            public void NullTypeException()
            {
                // Arrange
                Type testedType = null;
                string expectedExceptionMessage = @"type";

                // Act
                var actualException =
                    Assert.Throws<ArgumentNullException>(() => testedType.GetUnderlyingCollectionType());

                // Assert
                Assert.Equal(expectedExceptionMessage, actualException.ParamName);
            }
        }
    }
}