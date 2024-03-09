using System.ComponentModel.DataAnnotations;
using MGR.CommandLineParser.Command;
using MGR.CommandLineParser.Extensibility.ClassBased;
using MGR.CommandLineParser.Extensibility.Converters;
using Xunit;

namespace MGR.CommandLineParser.UnitTests.Extensibility.ClassBased;

public partial class ClassBasedCommandTypeTests
{
    public class FindOptionByShortName
    {
        [Theory]
        [InlineData("pl")]
        public void FoundWithLongOrAlternateName(string optionName)
        {
            // Arrange
            var testCommandType = new ClassBasedCommandType(typeof(TestCommand),
                new List<IConverter> { new StringConverter(), new GuidConverter(), new Int32Converter() }, new List<IPropertyOptionAlternateNameGenerator>());
            var serviceProviderSubstitute = Substitute.For<IServiceProvider>();
            serviceProviderSubstitute.GetService(default).ReturnsForAnyArgs(ClassBasedBasicCommandActivator.Instance);
            var classBasedCommandObjectBuilder = testCommandType.CreateCommandObjectBuilder(serviceProviderSubstitute);
            var testCommand = ((IClassBasedCommandObject<TestCommand, TestCommand>)classBasedCommandObjectBuilder.GenerateCommandObject()).Command;
            var testCommandData = ((IClassBasedCommandObject<TestCommand, TestCommand>)classBasedCommandObjectBuilder.GenerateCommandObject()).CommandData;

            // Act
            var actual = classBasedCommandObjectBuilder.FindOptionByShortName(optionName);

            // Assert
            Assert.NotNull(actual);
            Assert.NotNull(testCommand);
            Assert.True(actual.ShouldProvideValue);
            actual.AssignValue("42");
            Assert.Single(testCommandData.PropertyList);
            Assert.Equal(42, testCommandData.PropertyList.First());

        }

        private class TestCommand : CommandData, ICommandHandler<TestCommand>
        {
            [Display(ShortName = "pl")]
            public List<int> PropertyList { get; set; }

            public Dictionary<string, Guid> PropertyDictionary { get; set; }

            public int PropertySimple { get; set; }

            public Task<int> ExecuteAsync(TestCommand commandData, CancellationToken cancellationToken) => throw new NotImplementedException();
        }
    }
}
