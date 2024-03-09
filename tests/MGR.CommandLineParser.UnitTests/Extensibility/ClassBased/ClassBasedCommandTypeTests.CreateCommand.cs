using MGR.CommandLineParser.Command;
using MGR.CommandLineParser.Extensibility.ClassBased;
using MGR.CommandLineParser.Extensibility.Converters;
using Xunit;

namespace MGR.CommandLineParser.UnitTests.Extensibility.ClassBased;

public partial class ClassBasedCommandTypeTests
{
    public class CreateCommand
    {
        [Fact]
        public void PropertyWithNoConverterException()
        {
            // Arrange
            var testCommandType = new ClassBasedCommandType(typeof(TestBadConverterCommand),
                new List<IConverter>
                {
                    new StringConverter(),
                    new GuidConverter(),
                    new Int32Converter(),
                    new BooleanConverter()
                }, new List<IPropertyOptionAlternateNameGenerator>());
            var serviceProviderSubstitute = Substitute.For<IServiceProvider>();
            serviceProviderSubstitute.GetService(typeof(IClassBasedCommandActivator)).Returns(ClassBasedBasicCommandActivator.Instance);
            var optionName = nameof(TestBadConverterCommand.PropertySimpleWithBadConverter);
            var expectedMessageException =
                Constants.ExceptionMessages.ParserSpecifiedConverterNotValid(optionName,
                    testCommandType.Metadata.Name, typeof(int), typeof(bool));

            // Act
            var actualException =
                Assert.Throws<CommandLineParserException>(
                    () => testCommandType.CreateCommandObjectBuilder(serviceProviderSubstitute));

            // Assert
            Assert.Equal(expectedMessageException, actualException.Message);
        }
        private class TestBadConverterCommand : CommandData, ICommandHandler<TestBadConverterCommand>
        {
#pragma warning disable CS0618 // Type or member is obsolete
            [Converter(typeof(BooleanConverter))]
#pragma warning restore CS0618 // Type or member is obsolete
            public int PropertySimpleWithBadConverter { get; set; }

            public Task<int> ExecuteAsync(TestBadConverterCommand commandData, CancellationToken cancellationToken) => throw new NotImplementedException();
        }
    }
}
