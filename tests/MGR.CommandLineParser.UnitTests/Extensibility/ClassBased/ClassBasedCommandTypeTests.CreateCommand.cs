using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MGR.CommandLineParser.Command;
using MGR.CommandLineParser.Extensibility.ClassBased;
using MGR.CommandLineParser.Extensibility.Converters;
using Moq;
using Xunit;

namespace MGR.CommandLineParser.UnitTests.Extensibility.ClassBased
{
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
                var serviceProviderMock = new Mock<IServiceProvider>();
                serviceProviderMock.Setup(_ => _.GetService(typeof(IClassBasedCommandActivator)))
                    .Returns(ClassBasedBasicCommandActivator.Instance);
                var optionName = nameof(TestBadConverterCommand.PropertySimpleWithBadConverter);
                var expectedMessageException =
                    Constants.ExceptionMessages.ParserSpecifiedConverterNotValid(optionName,
                        testCommandType.Metadata.Name, typeof(int), typeof(bool));

                // Act
                var actualException =
                    Assert.Throws<CommandLineParserException>(
                        () => testCommandType.CreateCommandObjectBuilder(serviceProviderMock.Object));

                // Assert
                Assert.Equal(expectedMessageException, actualException.Message);
            }
            private class TestBadConverterCommand : ICommand
            {
#pragma warning disable CS0618 // Type or member is obsolete
                [Converter(typeof(BooleanConverter))]
#pragma warning restore CS0618 // Type or member is obsolete
                // ReSharper disable once UnusedMember.Local
                // ReSharper disable once UnusedAutoPropertyAccessor.Local
                public int PropertySimpleWithBadConverter { get; set; }

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
