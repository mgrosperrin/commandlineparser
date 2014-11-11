using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using MGR.CommandLineParser.Command;
using MGR.CommandLineParser.Converters;
using Moq;
using Xunit;

namespace MGR.CommandLineParser.UnitTests.Command
{
    public partial class OptionMetadataTests
    {
        public class AssignValue
        {
            [Fact]
            public void PropertyListAddTest()
            {
                // Arrange
                var testCommand = new TestCommand();
                testCommand.PropertyList = new List<int>();
                var parserOptionsMock = new Mock<IParserOptions>();
                parserOptionsMock.SetupGet(mock => mock.Converters)
                    .Returns(new List<IConverter>
                    {
                        new Int32Converter(),
                        new StringConverter(),
                        new GuidConverter()
                    });
                CommandMetadata commandMetadata = testCommand.ExtractMetadata(parserOptionsMock.Object);
                int expected = 42;
                int expectedLength = 1;
                string option = "42";

                // Act
                commandMetadata.GetOption("PropertyList").AssignValue(option, parserOptionsMock.Object);

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
                var testCommand = new TestCommand();
                testCommand.PropertyDictionary = new Dictionary<string, Guid>();
                var parserOptionsMock = new Mock<IParserOptions>();
                parserOptionsMock.SetupGet(mock => mock.Converters)
                    .Returns(new List<IConverter>
                    {
                        new Int32Converter(),
                        new StringConverter(),
                        new GuidConverter()
                    });
                CommandMetadata commandMetadata = testCommand.ExtractMetadata(parserOptionsMock.Object);
                string expectedKey = "keyTest";
                Guid expectedValue = Guid.Parse("18591394-096C-476F-A8B7-71903E27DAB5");
                int expectedLength = 1;
                string option = "keyTest=18591394-096C-476F-A8B7-71903E27DAB5";

                // Act
                commandMetadata.GetOption("PropertyDictionary").AssignValue(option, parserOptionsMock.Object);

                // Assert
                Assert.NotNull(testCommand.PropertyDictionary);
                Assert.IsType<Dictionary<string, Guid>>(testCommand.PropertyDictionary);
                Assert.Equal(expectedLength, testCommand.PropertyDictionary.Count);
                KeyValuePair<string, Guid> first = testCommand.PropertyDictionary.First();
                Assert.Equal(expectedKey, first.Key);
                Assert.Equal(expectedValue, first.Value);
            }

            [Fact]
            public void PropertySimpleTest()
            {
                // Arrange
                var testCommand = new TestCommand();
                testCommand.PropertyList = new List<int>();
                var parserOptionsMock = new Mock<IParserOptions>();
                parserOptionsMock.SetupGet(mock => mock.Converters)
                    .Returns(new List<IConverter>
                    {
                        new Int32Converter(),
                        new StringConverter(),
                        new GuidConverter()
                    });
                CommandMetadata commandMetadata = testCommand.ExtractMetadata(parserOptionsMock.Object);
                int expected = 42;
                string option = "42";

                // Act
                commandMetadata.GetOption("PropertySimple").AssignValue(option, parserOptionsMock.Object);

                // Assert
                Assert.Equal(expected, testCommand.PropertySimple);
            }

            [Fact]
            public void PropertyWithNoConverterException()
            {
                // Arrange
                var testCommand = new TestCommand();
                testCommand.PropertyList = new List<int>();
                var parserOptionsMock = new Mock<IParserOptions>();
                parserOptionsMock.SetupGet(mock => mock.Converters)
                    .Returns(new List<IConverter>
                    {
                        new Int32Converter(),
                        new StringConverter(),
                        new GuidConverter()
                    });
                CommandMetadata commandMetadata = testCommand.ExtractMetadata(parserOptionsMock.Object);
                OptionMetadata optionMetadata = commandMetadata.GetOption("PropertySimple");
                optionMetadata.Converter = new BooleanConverter();
                string expectedMessageException = string.Format(CultureInfo.CurrentUICulture,
                    "The specified converter is not valid : target type is '{1}' and option type is '{0}'.",
                    typeof (int).FullName, typeof (bool).FullName);

                // Act
                var actualException =
                    Assert.Throws<CommandLineParserException>(
                        () => optionMetadata.AssignValue(string.Empty, parserOptionsMock.Object));

                // Assert
                Assert.Equal(expectedMessageException, actualException.Message);
            }

            [Fact]
            public void NullParserOptionsException()
            {
                // Arrange
                IParserOptions nullOptions = null;
                var optionMetadata = new OptionMetadata(new OptionMetadataTemplate(null, null),
                    new CommandMetadata(new CommandMetadataTemplate(), new TestCommand()));
                string expectedMessageException = @"options";

                // Act
                var actualException =
                    Assert.Throws<ArgumentNullException>(() => optionMetadata.AssignValue(string.Empty, nullOptions));

                // Assert
                Assert.Equal(expectedMessageException, actualException.ParamName);
            }

            private class TestCommand : ICommand
            {
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

                public List<int> PropertyList { get; set; }
                public Dictionary<string, Guid> PropertyDictionary { get; set; }
                public int PropertySimple { get; set; }
            }
        }
    }
}