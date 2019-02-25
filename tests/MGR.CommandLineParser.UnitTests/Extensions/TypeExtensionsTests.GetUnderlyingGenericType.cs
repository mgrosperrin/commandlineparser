using System;
using System.Collections.Generic;
using Xunit;

namespace MGR.CommandLineParser.UnitTests.Extensions
{
    public partial class TypeExtensionsTests
    {
        public class GetUnderlyingGenericType
        {
            [Fact]
            public void TestNonGenericType()
            {
                // Arrange
                var testedType = typeof (int);

                // Act
                var actualType = testedType.GetUnderlyingGenericType();

                // Assert
                Assert.Null(actualType);
            }

            [Fact]
            public void TestGenericListType()
            {
                // Arrange
                var testedType = typeof (List<int>);
                var expected = typeof (int);

                // Act
                var actual = testedType.GetUnderlyingGenericType();

                // Assert
                Assert.Equal(expected, actual);
            }

            [Fact]
            public void TestFirstDictionaryType()
            {
                // Arrange
                var testedType = typeof (Dictionary<string, int>);
                var expected = typeof (string);

                // Act
                var actual = testedType.GetUnderlyingGenericType();

                // Assert
                Assert.Equal(expected, actual);
            }

            [Fact]
            public void TestSecondDictionaryType()
            {
                // Arrange
                var testedType = typeof (Dictionary<string, int>);
                var expected = typeof (int);
                var index = 1;

                // Act
                var actual = testedType.GetUnderlyingGenericType(index);

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
                // ReSharper disable once ExpressionIsAlwaysNull
                var actualException = Assert.Throws<ArgumentNullException>(() => testedType.GetUnderlyingGenericType());

                // Assert
                Assert.Equal(expectedExceptionMessage, actualException.ParamName);
            }
        }
    }
}