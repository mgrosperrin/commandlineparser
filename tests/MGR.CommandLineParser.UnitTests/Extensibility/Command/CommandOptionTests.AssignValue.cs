using System;
using System.Collections.Generic;
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
    public partial class CommandOptionTests
    {
        public class AssignValue
        {
            [Fact]
            public void PropertyListAddTest()
            {
                // Arrange
                var testCommandType = new CommandType(typeof (TestCommand),
                    new List<IConverter> {new StringConverter(), new GuidConverter(), new Int32Converter()}, new List<IOptionAlternateNameGenerator>());
                var serviceProviderMock = new Mock<IServiceProvider>();
                serviceProviderMock.Setup(_ => _.GetService(typeof(ICommandActivator)))
                    .Returns(BasicCommandActivator.Instance);
                var testCommand =
                    testCommandType.CreateCommand(serviceProviderMock.Object, new ParserOptions()) as
                        TestCommand;
                var optionName = nameof(TestCommand.PropertyList);
                var expected = 42;
                var expectedLength = 1;
                var option = "42";

                // Act
                testCommandType.FindOption(optionName).AssignValue(option, testCommand);

                // Assert
                Assert.NotNull(testCommand.PropertyList);
                Assert.IsType<List<int>>(testCommand.PropertyList);
                Assert.Equal(expectedLength, testCommand.PropertyList.Count);
                Assert.Equal(expected, testCommand.PropertyList[0]);
            }

            [Fact]
            public void PropertyDictionaryAddTest()
            {
                // Arrange
                var testCommandType = new CommandType(typeof (TestCommand),
                    new List<IConverter> {new StringConverter(), new GuidConverter(), new Int32Converter()}, new List<IOptionAlternateNameGenerator>());
                var serviceProviderMock = new Mock<IServiceProvider>();
                serviceProviderMock.Setup(_ => _.GetService(typeof(ICommandActivator)))
                    .Returns(BasicCommandActivator.Instance);
                var testCommand =
                    testCommandType.CreateCommand(serviceProviderMock.Object, new ParserOptions()) as
                        TestCommand;
                var optionName = nameof(TestCommand.PropertyDictionary);
                var expectedKey = "keyTest";
                var guid = "18591394-096C-476F-A8B7-71903E27DAB5";
                var expectedValue = Guid.Parse(guid);
                var expectedLength = 1;
                var option = "keyTest=" + guid;

                // Act
                testCommandType.FindOption(optionName).AssignValue(option, testCommand);

                // Assert
                Assert.NotNull(testCommand.PropertyDictionary);
                Assert.IsType<Dictionary<string, Guid>>(testCommand.PropertyDictionary);
                Assert.Equal(expectedLength, testCommand.PropertyDictionary.Count);
                var first = testCommand.PropertyDictionary.First();
                Assert.Equal(expectedKey, first.Key);
                Assert.Equal(expectedValue, first.Value);
            }

            [Fact]
            public void PropertySimpleTest()
            {
                // Arrange
                var testCommandType = new CommandType(typeof (TestCommand),
                    new List<IConverter> {new StringConverter(), new GuidConverter(), new Int32Converter()}, new List<IOptionAlternateNameGenerator>());
                var serviceProviderMock = new Mock<IServiceProvider>();
                serviceProviderMock.Setup(_ => _.GetService(typeof(ICommandActivator)))
                    .Returns(BasicCommandActivator.Instance);
                var testCommand =
                    testCommandType.CreateCommand(serviceProviderMock.Object, new ParserOptions()) as
                        TestCommand;
                var optionName = nameof(TestCommand.PropertySimple);
                var expected = 42;
                var option = "42";

                // Act
                testCommandType.FindOption(optionName).AssignValue(option, testCommand);

                // Assert
                Assert.Equal(expected, testCommand.PropertySimple);
            }

            private class TestCommand : ICommand
            {
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