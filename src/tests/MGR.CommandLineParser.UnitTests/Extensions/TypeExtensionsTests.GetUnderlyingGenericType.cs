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
                Type testedType = typeof (int);

                // Act
                Type actualType = testedType.GetUnderlyingGenericType();

                // Assert
                Assert.Null(actualType);
            }

            [Fact]
            public void TestGenericListType()
            {
                // Arrange
                Type testedType = typeof (List<int>);
                Type expected = typeof (int);

                // Act
                Type actual = testedType.GetUnderlyingGenericType();

                // Assert
                Assert.Equal(expected, actual);
            }

            [Fact]
            public void TestFirstDictionaryType()
            {
                // Arrange
                Type testedType = typeof (Dictionary<string, int>);
                Type expected = typeof (string);

                // Act
                Type actual = testedType.GetUnderlyingGenericType();

                // Assert
                Assert.Equal(expected, actual);
            }

            [Fact]
            public void TestSecondDictionaryType()
            {
                // Arrange
                Type testedType = typeof (Dictionary<string, int>);
                Type expected = typeof (int);
                int index = 1;

                // Act
                Type actual = testedType.GetUnderlyingGenericType(index);

                // Assert
                Assert.Equal(expected, actual);
            }

            [Fact]
            public void TestNullType()
            {
                // Arrange
                Type testedType = null;
                string expectedExceptionMessage = @"type";

                // Act
                var actualException = Assert.Throws<ArgumentNullException>(() => testedType.GetUnderlyingGenericType());

                // Assert
                Assert.Equal(expectedExceptionMessage, actualException.ParamName);
            }
        }
    }
}