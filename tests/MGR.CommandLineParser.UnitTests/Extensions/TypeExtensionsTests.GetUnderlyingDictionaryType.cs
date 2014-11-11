using System;
using System.Collections.Generic;
using Xunit;

namespace MGR.CommandLineParser.UnitTests.Extensions
{
    public partial class TypeExtensionsTests
    {
        public class GetUnderlyingDictionaryType
        {
            [Fact]
            public void DictionaryStringIntKeyTest()
            {
                // Arrange
                Type testedType = typeof (Dictionary<string, int>);
                Type expected = typeof (string);

                // Act
                Type actual = testedType.GetUnderlyingDictionaryType(true);

                // Assert
                Assert.Equal(expected, actual);
            }

            [Fact]
            public void IDictionaryStringIntKeyTest()
            {
                // Arrange
                Type testedType = typeof (IDictionary<string, int>);
                Type expected = typeof (string);

                // Act
                Type actual = testedType.GetUnderlyingDictionaryType(true);

                // Assert
                Assert.Equal(expected, actual);
            }

            [Fact]
            public void DictionaryStringIntValueTest()
            {
                // Arrange
                Type testedType = typeof (Dictionary<string, int>);
                Type expected = typeof (int);

                // Act
                Type actual = testedType.GetUnderlyingDictionaryType(false);

                // Assert
                Assert.Equal(expected, actual);
            }

            [Fact]
            public void IDictionaryStringIntValueTest()
            {
                // Arrange
                Type testedType = typeof (IDictionary<string, int>);
                Type expected = typeof (int);

                // Act
                Type actual = testedType.GetUnderlyingDictionaryType(false);

                // Assert
                Assert.Equal(expected, actual);
            }

            [Fact]
            public void TupleIntKeyTest()
            {
                // Arrange
                Type testedType = typeof (Tuple<int>);
                Type expected = null;

                // Act
                Type actual = testedType.GetUnderlyingDictionaryType(true);

                // Assert
                Assert.Equal(expected, actual);
            }

            [Fact]
            public void TupleIntValueTest()
            {
                // Arrange
                Type testedType = typeof (Tuple<int>);
                Type expected = null;

                // Act
                Type actual = testedType.GetUnderlyingDictionaryType(false);

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
                    Assert.Throws<ArgumentNullException>(() => testedType.GetUnderlyingDictionaryType(true));

                // Assert
                Assert.Equal(expectedExceptionMessage, actualException.ParamName);
            }
        }
    }
}