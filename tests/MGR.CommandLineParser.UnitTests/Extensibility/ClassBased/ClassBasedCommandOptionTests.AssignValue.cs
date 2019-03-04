using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using MGR.CommandLineParser.Command;
using MGR.CommandLineParser.Extensibility;
using MGR.CommandLineParser.Extensibility.ClassBased;
using MGR.CommandLineParser.Extensibility.Command;
using MGR.CommandLineParser.Extensibility.Converters;
using Moq;
using Xunit;

namespace MGR.CommandLineParser.UnitTests.Extensibility.ClassBased
{
    public partial class ClassBasedCommandOptionTests
    {
        public class AssignValue
        {
            private static (TestCommand Command, ClassBasedCommandOption CommandOption) CreateCommandOptionForProperty<T>(
                Expression<Func<TestCommand, T>> propertyExpression)
            {
                var propertyInfo =
                    typeof(TestCommand).GetProperty(TypeHelpers.ExtractPropertyName(propertyExpression));
                return CreateCommandOptionForProperty(propertyInfo);
            }
            private static (TestCommand Command, ClassBasedCommandOption CommandOption) CreateCommandOptionForProperty(
                    PropertyInfo propertyInfo)
                {
                    var testCommand = new TestCommand();
                    var commandMetadataMock = new Mock<ICommandMetadata>();
                    commandMetadataMock.SetupGet(_ => _.Name)
                        .Returns("test");
                    var commandOption = new ClassBasedCommandOption(
                        ClassBasedCommandOptionMetadata.Create(
                            propertyInfo,
                            commandMetadataMock.Object,
                            new List<IConverter> { new StringConverter(), new GuidConverter(), new Int32Converter() },
                            new List<IOptionAlternateNameGenerator>()
                        ),
                        testCommand);
                    return (testCommand, commandOption);
                }

            private class TestCommand : ICommand
            {
                public List<int> PropertyList { get; set; }
                public Dictionary<string, Guid> PropertyDictionary { get; set; }
                public int PropertySimple { get; set; }

                #region ICommand Members

                public Task<int> ExecuteAsync() => throw new NotImplementedException();

                public IList<string> Arguments => throw new NotImplementedException();

                #endregion
            }

            [Fact]
            public void PropertyDictionaryAddTest()
            {
                // Arrange
                var (testCommand, commandOption) = CreateCommandOptionForProperty(_ => _.PropertyDictionary);
                var expectedKey = "keyTest";
                var guid = "18591394-096C-476F-A8B7-71903E27DAB5";
                var expectedValue = Guid.Parse(guid);
                var expectedLength = 1;
                var option = "keyTest=" + guid;

                // Act
                commandOption.AssignValue(option);

                // Assert
                Assert.NotNull(testCommand.PropertyDictionary);
                Assert.Equal(expectedLength, testCommand.PropertyDictionary.Count);
                var first = testCommand.PropertyDictionary.First();
                Assert.Equal(expectedKey, first.Key);
                Assert.Equal(expectedValue, first.Value);
            }

            [Fact]
            public void PropertyListAddTest()
            {
                // Arrange
                var (testCommand, commandOption) = CreateCommandOptionForProperty(_ => _.PropertyList);
                var expected = 42;
                var expectedLength = 1;
                var option = "42";

                // Act
                commandOption.AssignValue(option);

                // Assert
                Assert.NotNull(testCommand.PropertyList);
                Assert.IsType<List<int>>(testCommand.PropertyList);
                Assert.Equal(expectedLength, testCommand.PropertyList.Count);
                Assert.Equal(expected, testCommand.PropertyList[0]);
            }

            [Fact]
            public void PropertySimpleTest()
            {
                // Arrange
                var (testCommand, commandOption) = CreateCommandOptionForProperty<int>(_ => _.PropertySimple);
                var expected = 42;
                var option = "42";

                // Act
                commandOption.AssignValue(option);

                // Assert
                Assert.Equal(expected, testCommand.PropertySimple);
            }
        }
    }
}