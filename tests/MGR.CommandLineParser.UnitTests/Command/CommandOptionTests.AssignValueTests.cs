using System;
using System.Collections.Generic;
using System.Linq;
using MGR.CommandLineParser.Command;
using MGR.CommandLineParser.Converters;
using Moq;
using Xunit;

namespace MGR.CommandLineParser.UnitTests.Command
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
                    new List<IConverter> {new StringConverter(), new GuidConverter(), new Int32Converter()});
                var dependencyResolverScopeMock = new Mock<IDependencyResolverScope>();
                dependencyResolverScopeMock.Setup(_ => _.ResolveDependency<ICommandActivator>())
                    .Returns(BasicCommandActivator.Instance);
                var testCommand =
                    testCommandType.CreateCommand(dependencyResolverScopeMock.Object, new ParserOptions()) as
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
                    new List<IConverter> {new StringConverter(), new GuidConverter(), new Int32Converter()});
                var dependencyResolverScopeMock = new Mock<IDependencyResolverScope>();
                dependencyResolverScopeMock.Setup(_ => _.ResolveDependency<ICommandActivator>())
                    .Returns(BasicCommandActivator.Instance);
                var testCommand =
                    testCommandType.CreateCommand(dependencyResolverScopeMock.Object, new ParserOptions()) as
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
                    new List<IConverter> {new StringConverter(), new GuidConverter(), new Int32Converter()});
                var dependencyResolverScopeMock = new Mock<IDependencyResolverScope>();
                dependencyResolverScopeMock.Setup(_ => _.ResolveDependency<ICommandActivator>())
                    .Returns(BasicCommandActivator.Instance);
                var testCommand =
                    testCommandType.CreateCommand(dependencyResolverScopeMock.Object, new ParserOptions()) as
                        TestCommand;
                var optionName = nameof(TestCommand.PropertySimple);
                var expected = 42;
                var option = "42";

                // Act
                testCommandType.FindOption(optionName).AssignValue(option, testCommand);

                // Assert
                Assert.Equal(expected, testCommand.PropertySimple);
            }

            [Fact]
            public void PropertyWithNoConverterException()
            {
                // Arrange
                var testCommandType = new CommandType(typeof (TestBadConverterCommand),
                    new List<IConverter>
                    {
                        new StringConverter(),
                        new GuidConverter(),
                        new Int32Converter(),
                        new BooleanConverter()
                    });
                var dependencyResolverScopeMock = new Mock<IDependencyResolverScope>();
                dependencyResolverScopeMock.Setup(_ => _.ResolveDependency<ICommandActivator>())
                    .Returns(BasicCommandActivator.Instance);
                var testCommand =
                    testCommandType.CreateCommand(dependencyResolverScopeMock.Object, new ParserOptions()) as
                        TestBadConverterCommand;
                var optionName = nameof(TestBadConverterCommand.PropertySimpleWithBadConverter);
                var expectedMessageException =
                    Constants.ExceptionMessages.ParserSpecifiedConverterNotValid(optionName,
                        testCommandType.Metadata.Name, typeof (int), typeof (bool));

                // Act
                var actualException =
                    Assert.Throws<CommandLineParserException>(
                        () => testCommandType.FindOption(optionName).AssignValue(string.Empty, testCommand));

                // Assert
                Assert.Equal(expectedMessageException, actualException.Message);
            }

            private class TestCommand : ICommand
            {
                public List<int> PropertyList { get; set; }
                public Dictionary<string, Guid> PropertyDictionary { get; set; }
                public int PropertySimple { get; set; }

                #region ICommand Members

                public int Execute()
                {
                    throw new NotImplementedException();
                }

                public IList<string> Arguments
                {
                    get { throw new NotImplementedException(); }
                }

                #endregion
            }

            private class TestBadConverterCommand : ICommand
            {
                [Converter(typeof (BooleanConverter))]
                public int PropertySimpleWithBadConverter { get; set; }

                #region ICommand Members

                public int Execute()
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