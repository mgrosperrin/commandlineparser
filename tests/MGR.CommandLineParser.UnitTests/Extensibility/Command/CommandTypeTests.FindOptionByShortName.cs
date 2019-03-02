using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using MGR.CommandLineParser.Command;
using MGR.CommandLineParser.Extensibility;
using MGR.CommandLineParser.Extensibility.ClassBased;
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
                var testCommandType = new ClassBasedCommandType(typeof(TestCommand),
                    new List<IConverter> { new StringConverter(), new GuidConverter(), new Int32Converter() }, new List<IOptionAlternateNameGenerator>());
                var serviceProviderMock = new Mock<IServiceProvider>();
                serviceProviderMock.Setup(_ => _.GetService(typeof(IClassBasedCommandActivator)))
                    .Returns(ClassBasedBasicCommandActivator.Instance);
                var classBasedCommandObjectBuilder =
                    (ClassBasedCommandObjectBuilder)testCommandType.CreateCommandObjectBuilder(serviceProviderMock.Object, new ParserOptions());
                var testCommand = (TestCommand)((IClassBasedCommandObject)classBasedCommandObjectBuilder.GenerateCommandObject()).Command;

                // Act
                var actual = classBasedCommandObjectBuilder.FindOptionByShortName(optionName);

                // Assert
                Assert.NotNull(actual);
                Assert.NotNull(testCommand);
                Assert.True(actual.ShouldProvideValue);
                actual.AssignValue("42");
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
