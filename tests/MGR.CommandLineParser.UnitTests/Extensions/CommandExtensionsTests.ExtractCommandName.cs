using System;
using System.Collections.Generic;
using MGR.CommandLineParser.Command;
using Xunit;

namespace MGR.CommandLineParser.UnitTests.Extensions
{
    public partial class CommandExtensionsTests
    {
        public class ExtractCommandName
        {
            [Fact]
            public void HelloCommandTest()
            {
                // Arrange
                ICommand command = new HelloCommand();
                var execpted = "Hello";

                // Act
                var actual = command.ExtractCommandName();

                // Assert
                Assert.Equal(execpted, actual);
            }

            [Fact]
            public void NullCommandException()
            {
                // Arrange
                ICommand command = null;
                var expectedExceptionMessage = SourceParameterName;

                // Act
                var actualException = Assert.Throws<ArgumentNullException>(() => command.ExtractCommandName());

                // Assert
                Assert.Equal(expectedExceptionMessage, actualException.ParamName);
            }

            private class HelloCommand : ICommand
            {
                public int Execute()
                {
                    throw new NotImplementedException();
                }

                public IList<string> Arguments
                {
                    get { throw new NotImplementedException(); }
                }
            }
        }
    }
}