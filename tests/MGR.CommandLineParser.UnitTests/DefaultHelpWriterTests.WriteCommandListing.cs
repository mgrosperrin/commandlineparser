using System;
using Moq;
using Xunit;

namespace MGR.CommandLineParser.UnitTests
{
    public partial class DefaultHelpWriterTests
    {
        public class WriteCommandListing
        {
            [Fact]
            public void NullParserOptionsThrowsException()
            {
                var defaultHelpWriter = new DefaultHelpWriter(null, null);
                var expectedParamNameException = "parserOptions";

                var actualException = Assert.Throws<ArgumentNullException>(() => defaultHelpWriter.WriteCommandListing(null));

                Assert.Equal(expectedParamNameException, actualException.ParamName);
            }
        }
    }
}