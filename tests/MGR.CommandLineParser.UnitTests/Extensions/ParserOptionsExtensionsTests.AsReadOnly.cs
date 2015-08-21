using System;
using System.Collections.Generic;
using System.Linq;
using MGR.CommandLineParser.Converters;
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
                var commandProviderMock = new Mock<ICommandProvider>();
                var parserOptionsMock = new Mock<IParserOptions>();
                parserOptionsMock.SetupGet(mock => mock.CommandLineName).Returns("MyCommandLine");
                parserOptionsMock.SetupGet(mock => mock.Logo).Returns("MySuperLogo");
                parserOptionsMock.SetupGet(mock => mock.CommandProvider).Returns(commandProviderMock.Object);
                parserOptionsMock.SetupGet(mock => mock.Converters).Returns(new List<IConverter>());

                // Act
                IParserOptions actual = parserOptionsMock.Object.AsReadOnly();

                // Assert
                Assert.NotNull(actual);
                Assert.IsType<ReadOnlyParserOptions>(actual);
                Assert.Equal(commandProviderMock.Object, actual.CommandProvider);
                Assert.Equal(0, actual.Converters.Count());
            }

            [Fact]
            public void NullParserOptionsException()
            {
                // Arrange
                IParserOptions parserOptions = null;
                string expectedExceptionMessage = @"options";

                // Act
                var actualException = Assert.Throws<ArgumentNullException>(() => parserOptions.AsReadOnly());

                // Assert
                Assert.Equal(expectedExceptionMessage, actualException.ParamName);
            }
        }
    }
}