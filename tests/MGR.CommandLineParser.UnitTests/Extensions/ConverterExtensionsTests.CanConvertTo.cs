using System;
using System.Collections.Generic;
using MGR.CommandLineParser.Converters;
using Moq;
using Xunit;

namespace MGR.CommandLineParser.UnitTests.Extensions
{
    public partial class ConverterExtensionsTests
    {
        public class CanConvertTo
        {
            [Fact]
            public void ConvertInt()
            {
                // Arrange
                var converterMoq = new Mock<IConverter>();
                converterMoq.SetupGet(converter => converter.TargetType).Returns(typeof (int));
                bool expected = true;
                Type assignableType = typeof (int);

                // Act
                bool actual = converterMoq.Object.CanConvertTo(assignableType);

                // Assert
                Assert.Equal(expected, actual);
            }

            [Fact]
            public void ConvertListIntAndInt()
            {
                // Arrange
                var converterMoq = new Mock<IConverter>();
                converterMoq.SetupGet(converter => converter.TargetType).Returns(typeof (int));
                bool expected = true;
                Type assignableType = typeof (List<int>);

                // Act
                bool actual = converterMoq.Object.CanConvertTo(assignableType);

                // Assert
                Assert.Equal(expected, actual);
            }

            [Fact]
            public void ConvertListIntAndString()
            {
                // Arrange
                var converterMoq = new Mock<IConverter>();
                converterMoq.SetupGet(converter => converter.TargetType).Returns(typeof (string));
                bool expected = false;
                Type assignableType = typeof (List<int>);

                // Act
                bool actual = converterMoq.Object.CanConvertTo(assignableType);

                // Assert
                Assert.Equal(expected, actual);
            }

            [Fact]
            public void ConvertDictionaryIntStringAndInt()
            {
                // Arrange
                var converterMoq = new Mock<IConverter>();
                converterMoq.SetupGet(converter => converter.TargetType).Returns(typeof (int));
                bool expected = false;
                Type assignableType = typeof (Dictionary<int, string>);

                // Act
                bool actual = converterMoq.Object.CanConvertTo(assignableType);

                // Assert
                Assert.Equal(expected, actual);
            }

            [Fact]
            public void ConvertDictionaryIntIntAndString()
            {
                // Arrange
                var converterMoq = new Mock<IConverter>();
                converterMoq.SetupGet(converter => converter.TargetType).Returns(typeof (KeyValuePair<int, int>));
                bool expected = true;
                Type assignableType = typeof (KeyValuePair<int, int>);

                // Act
                bool actual = converterMoq.Object.CanConvertTo(assignableType);

                // Assert
                Assert.Equal(expected, actual);
            }

            [Fact]
            public void NullConverterException()
            {
                // Arrange
                IConverter testedConverter = null;
                Type expectedExceptionType = typeof (ArgumentNullException);
                string expectedExceptionMessage = @"converter";

                // Act
                var actualException =
                    Assert.Throws<ArgumentNullException>(() => testedConverter.CanConvertTo(expectedExceptionType));

                // Assert
                Assert.Equal(expectedExceptionMessage, actualException.ParamName);
            }

            [Fact]
            public void NullValueException()
            {
                // Arrange
                var converterMoq = new Mock<IConverter>();
                converterMoq.SetupGet(converter => converter.TargetType).Returns(typeof (int));
                Type testedType = null;
                string expectedExceptionMessage = @"targetType";

                // Act
                var actualException =
                    Assert.Throws<ArgumentNullException>(() => converterMoq.Object.CanConvertTo(testedType));

                // Assert
                Assert.Equal(expectedExceptionMessage, actualException.ParamName);
            }
        }
    }
}