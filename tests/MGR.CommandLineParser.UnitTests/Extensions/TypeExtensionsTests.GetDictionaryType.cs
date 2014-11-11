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
                Type testedType = typeof (List<int>);
                Type expected = null;

                // Act
                Type actual = testedType.GetDictionaryType();

                // Assert
                Assert.Equal(expected, actual);
            }

            [Fact]
            public void ICollectionIntTest()
            {
                // Arrange
                Type testedType = typeof (ICollection<int>);
                Type expected = null;

                // Act
                Type actual = testedType.GetDictionaryType();

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
                Type actual = testedType.GetDictionaryType();

                // Assert
                Assert.Equal(expected, actual);
            }

            [Fact]
            public void DictionaryStringInt()
            {
                // Arrange
                Type testedType = typeof (Dictionary<string, int>);
                Type expected = typeof (IDictionary<string, int>);

                // Act
                Type actual = testedType.GetDictionaryType();

                // Assert
                Assert.Equal(expected, actual);
            }

            [Fact]
            public void IDictionaryStringInt()
            {
                // Arrange
                Type testedType = typeof (IDictionary<string, int>);
                Type expected = typeof (IDictionary<string, int>);

                // Act
                Type actual = testedType.GetDictionaryType();

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
                var actualException = Assert.Throws<ArgumentNullException>(() => testedType.GetDictionaryType());

                // Assert
                Assert.Equal(expectedExceptionMessage, actualException.ParamName);
            }
        }
    }
}