using System;
using System.Collections.Generic;
using MGR.CommandLineParser.Extensibility.Converters;
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
                var expected = true;
                var assignableType = typeof (int);

                // Act
                var actual = converterMoq.Object.CanConvertTo(assignableType);

                // Assert
                Assert.Equal(expected, actual);
            }

            [Fact]
            public void ConvertListIntAndInt()
            {
                // Arrange
                var converterMoq = new Mock<IConverter>();
                converterMoq.SetupGet(converter => converter.TargetType).Returns(typeof (int));
                var expected = true;
                var assignableType = typeof (List<int>);

                // Act
                var actual = converterMoq.Object.CanConvertTo(assignableType);

                // Assert
                Assert.Equal(expected, actual);
            }

            [Fact]
            public void ConvertListIntAndString()
            {
                // Arrange
                var converterMoq = new Mock<IConverter>();
                converterMoq.SetupGet(converter => converter.TargetType).Returns(typeof (string));
                var expected = false;
                var assignableType = typeof (List<int>);

                // Act
                var actual = converterMoq.Object.CanConvertTo(assignableType);

                // Assert
                Assert.Equal(expected, actual);
            }

            [Fact]
            public void ConvertDictionaryIntStringAndInt()
            {
                // Arrange
                var converterMoq = new Mock<IConverter>();
                converterMoq.SetupGet(converter => converter.TargetType).Returns(typeof (int));
                var expected = false;
                var assignableType = typeof (Dictionary<int, string>);

                // Act
                var actual = converterMoq.Object.CanConvertTo(assignableType);

                // Assert
                Assert.Equal(expected, actual);
            }

            [Fact]
            public void ConvertDictionaryIntIntAndString()
            {
                // Arrange
                var converterMoq = new Mock<IConverter>();
                converterMoq.SetupGet(converter => converter.TargetType).Returns(typeof (KeyValuePair<int, int>));
                var expected = true;
                var assignableType = typeof (KeyValuePair<int, int>);

                // Act
                var actual = converterMoq.Object.CanConvertTo(assignableType);

                // Assert
                Assert.Equal(expected, actual);
            }

            [Fact]
            public void NullConverterException()
            {
                // Arrange
                IConverter testedConverter = null;
                var expectedExceptionType = typeof (ArgumentNullException);
                var expectedExceptionMessage = SourceParameterName;

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
                Type targetType = null;
                var expectedExceptionMessage = nameof(targetType);

                // Act
                var actualException =
                    Assert.Throws<ArgumentNullException>(() => converterMoq.Object.CanConvertTo(targetType));

                // Assert
                Assert.Equal(expectedExceptionMessage, actualException.ParamName);
            }
        }
    }
}