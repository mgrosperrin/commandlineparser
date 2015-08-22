using System;
using Moq;
using Xunit;

namespace MGR.CommandLineParser.UnitTests.Extensions
{
    public partial class ParserOptionsExtensionsTests
    {
        public class AsReadOnly
        {
            [Fact]
            public void ReadOnlyParserTest()
            {
                // Arrange
                var parserOptionsMock = new Mock<IParserOptions>();
                parserOptionsMock.SetupGet(mock => mock.CommandLineName).Returns("MyCommandLine");
                parserOptionsMock.SetupGet(mock => mock.Logo).Returns("MySuperLogo");

                // Act
                var actual = parserOptionsMock.Object.AsReadOnly();

                // Assert
                Assert.NotNull(actual);
                Assert.IsType<ReadOnlyParserOptions>(actual);
            }

            [Fact]
            public void NullParserOptionsException()
            {
                // Arrange
                IParserOptions parserOptions = null;
                var expectedExceptionMessage = @"options";

                // Act
                // ReSharper disable once ExpressionIsAlwaysNull
                var actualException = Assert.Throws<ArgumentNullException>(() => parserOptions.AsReadOnly());

                // Assert
                Assert.Equal(expectedExceptionMessage, actualException.ParamName);
            }
        }
    }
}