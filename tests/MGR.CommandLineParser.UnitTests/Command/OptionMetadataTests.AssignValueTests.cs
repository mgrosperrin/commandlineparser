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
                DefaultServiceResolver.RegisterServices(() => new List<IConverter> { new StringConverter(), new GuidConverter(), new Int32Converter() });
                CommandMetadata commandMetadata = testCommand.ExtractMetadata();
                int expected = 42;
                int expectedLength = 1;
                string option = "42";

                // Act
                commandMetadata.GetOption("PropertyList").AssignValue(option);

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
                DefaultServiceResolver.RegisterServices(() => new List<IConverter> { new StringConverter(), new GuidConverter(), new Int32Converter() });
                CommandMetadata commandMetadata = testCommand.ExtractMetadata();
                string expectedKey = "keyTest";
                var guid = "18591394-096C-476F-A8B7-71903E27DAB5";
                Guid expectedValue = Guid.Parse(guid);
                int expectedLength = 1;
                string option = "keyTest=" + guid;

                // Act
                commandMetadata.GetOption("PropertyDictionary").AssignValue(option);

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
                DefaultServiceResolver.RegisterServices(() => new List<IConverter> { new StringConverter(), new GuidConverter(), new Int32Converter() });
                CommandMetadata commandMetadata = testCommand.ExtractMetadata();
                int expected = 42;
                string option = "42";

                // Act
                commandMetadata.GetOption("PropertySimple").AssignValue(option);

                // Assert
                Assert.Equal(expected, testCommand.PropertySimple);
            }

            [Fact]
            public void PropertyWithNoConverterException()
            {
                // Arrange
                var testCommand = new TestCommand();
                testCommand.PropertyList = new List<int>();
                DefaultServiceResolver.RegisterServices(() => new List<IConverter> { new StringConverter(), new GuidConverter(), new Int32Converter() });
                CommandMetadata commandMetadata = testCommand.ExtractMetadata();
                OptionMetadata optionMetadata = commandMetadata.GetOption("PropertySimple");
                optionMetadata.Converter = new BooleanConverter();
                string expectedMessageException = string.Format(CultureInfo.CurrentUICulture,
                    "The specified converter is not valid : target type is '{1}' and option type is '{0}'.",
                    typeof (int).FullName, typeof (bool).FullName);

                // Act
                var actualException =
                    Assert.Throws<CommandLineParserException>(
                        () => optionMetadata.AssignValue(string.Empty));

                // Assert
                Assert.Equal(expectedMessageException, actualException.Message);
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