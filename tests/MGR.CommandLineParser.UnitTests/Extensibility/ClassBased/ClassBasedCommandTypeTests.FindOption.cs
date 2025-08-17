using System.ComponentModel.DataAnnotations;
using System.Reflection;
using MGR.CommandLineParser.Command;
using MGR.CommandLineParser.Extensibility.ClassBased;
using MGR.CommandLineParser.Extensibility.Converters;
using Xunit;

namespace MGR.CommandLineParser.UnitTests.Extensibility.ClassBased;

public partial class ClassBasedCommandTypeTests
{
    public class FindOption
    {
        [Theory]
        [InlineData("property-list")]
        [InlineData("OtherName")]
        public void FoundWithLongOrAlternateName(string optionName)
        {
            // Arrange
            var propertyOptionAlternateNameGeneratorSubstitute = Substitute.For<IPropertyOptionAlternateNameGenerator>();
            propertyOptionAlternateNameGeneratorSubstitute.GenerateAlternateNames(default).ReturnsForAnyArgs(x =>
                {
                    var p = (PropertyInfo)x[0];
                    if (p.Name == "PropertyList")
                    {
                        return new List<string> { "OtherName" };
                    }

                    return Enumerable.Empty<string>();
                });
            var testCommandType = new ClassBasedCommandType(typeof(TestCommand),
                new List<IConverter> { new StringConverter(), new GuidConverter(), new Int32Converter() },
                new List<IPropertyOptionAlternateNameGenerator> { propertyOptionAlternateNameGeneratorSubstitute });
            var serviceProviderSubstitute = Substitute.For<IServiceProvider>();
            serviceProviderSubstitute.GetService(typeof(IClassBasedCommandActivator)).Returns(ClassBasedBasicCommandActivator.Instance);
            var classBasedCommandObjectBuilder = testCommandType.CreateCommandObjectBuilder(serviceProviderSubstitute);
            var testCommand = ((IClassBasedCommandObject<TestCommand, TestCommand>)classBasedCommandObjectBuilder.GenerateCommandObject()).Command;
            var testCommandData = ((IClassBasedCommandObject<TestCommand, TestCommand>)classBasedCommandObjectBuilder.GenerateCommandObject()).CommandData;

            // Act
            var actual = classBasedCommandObjectBuilder.FindOption(optionName);

            // Assert
            Assert.NotNull(actual);
            Assert.NotNull(testCommand);
            Assert.True(actual.ShouldProvideValue);
            actual.AssignValue("42");
            Assert.Single(testCommandData.PropertyList);
            Assert.Equal(42, testCommandData.PropertyList.First());

        }

        internal class TestCommand : CommandData, ICommandHandler<TestCommand>
        {
            [Display(ShortName = "pl")]
            public List<int> PropertyList { get; set; }
            public Dictionary<string, Guid> PropertyDictionary { get; set; }
            public int PropertySimple { get; set; }

            public Task<int> ExecuteAsync(TestCommand commandData, CancellationToken cancellationToken) => throw new NotImplementedException();
        }
    }
}
