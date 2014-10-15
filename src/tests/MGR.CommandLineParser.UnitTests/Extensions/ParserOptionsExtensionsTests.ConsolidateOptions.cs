using System.Linq;
using Xunit;

namespace MGR.CommandLineParser.UnitTests.Extensions
{
    public partial class ParserOptionsExtensionsTests
    {
        public class ConsolidateOptions
        {
            private const int NbDefaultConverter = 15;

            [Fact]
            public void ConsolidateExistingOptions()
            {
                // Arrange
                var parserOptions = new ParserOptions();
                parserOptions.CommandLineName = "MyCommandLine";
                parserOptions.Logo = "MySuperLogo";
                int expectedHashCode = parserOptions.GetHashCode();

                // Act
                ParserOptions actual = parserOptions.ConsolidateOptions();

                // Asser
                Assert.Equal(expectedHashCode, actual.GetHashCode());
                Assert.IsType<DefaultConsole>(actual.Console);
                Assert.IsType<DefaultCommandProvider>(actual.CommandProvider);
                Assert.Equal("MyCommandLine", parserOptions.CommandLineName);
                Assert.Equal("MySuperLogo", parserOptions.Logo);
                Assert.Equal(NbDefaultConverter, parserOptions.Converters.Count());
            }
        }
    }
}