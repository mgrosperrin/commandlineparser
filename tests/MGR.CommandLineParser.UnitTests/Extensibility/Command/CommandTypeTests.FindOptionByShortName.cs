using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using MGR.CommandLineParser.Command;
using MGR.CommandLineParser.Extensibility;
using MGR.CommandLineParser.Extensibility.Command;
using MGR.CommandLineParser.Extensibility.Converters;
using Moq;
using Xunit;

namespace MGR.CommandLineParser.UnitTests.Extensibility.Command
{
    public partial class CommandTypeTests
    {
        public class FindOptionByShortName
        {
            [Theory]
            [InlineData("pl")]
            public void FoundWithLongOrAlternateName(string optionName)
            {
                // Arrange
                var testCommandType = new CommandType(typeof(TestCommand),
                    new List<IConverter> { new StringConverter(), new GuidConverter(), new Int32Converter() }, new List<IOptionAlternateNameGenerator>());
                var serviceProviderMock = new Mock<IServiceProvider>();
                serviceProviderMock.Setup(_ => _.GetService(typeof(ICommandActivator)))
                    .Returns(BasicCommandActivator.Instance);
                var testCommand = testCommandType.CreateCommand(serviceProviderMock.Object, new ParserOptions()) as TestCommand;

                // Act
                var actual = testCommandType.FindOptionByShortName(optionName);

                // Assert
                Assert.NotNull(actual);
                Assert.NotNull(testCommand);
                Assert.False(actual.OptionalValue);
                actual.AssignValue("42", testCommand);
                Assert.Single(testCommand.PropertyList);
                Assert.Equal(42, testCommand.PropertyList.First());

            }

            private class TestCommand : ICommand
            {
                [Display(ShortName = "pl")]
                public List<int> PropertyList { get; set; }
                public Dictionary<string, Guid> PropertyDictionary { get; set; }
                public int PropertySimple { get; set; }

                #region ICommand Members

                public Task<int> ExecuteAsync()
                {
                    throw new NotImplementedException();
                }

                public IList<string> Arguments
                {
                    get { throw new NotImplementedException(); }
                }

                #endregion
            }
        }
    }
}
