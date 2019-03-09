using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
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
        public class FindOption
        {
            [Theory]
            [InlineData("property-list")]
            [InlineData("OtherName")]
            public void FoundWithLongOrAlternateName(string optionName)
            {
                // Arrange
                var propertyOptionAlternateNameGeneratorMock = new Mock<IPropertyOptionAlternateNameGenerator>();
                propertyOptionAlternateNameGeneratorMock.Setup(_ => _.GenerateAlternateNames(It.IsAny<PropertyInfo>()))
                    .Returns<PropertyInfo>(p =>
                    {
                        if (p.Name == "PropertyList")
                        {
                            return new List<string> {"OtherName"};
                        }

                        return Enumerable.Empty<string>();
                    });
                var testCommandType = new ClassBasedCommandType(typeof(TestCommand),
                    new List<IConverter> { new StringConverter(), new GuidConverter(), new Int32Converter() },
                    new List<IPropertyOptionAlternateNameGenerator>{ propertyOptionAlternateNameGeneratorMock .Object});
                var serviceProviderMock = new Mock<IServiceProvider>();
                serviceProviderMock.Setup(_ => _.GetService(typeof(IClassBasedCommandActivator)))
                    .Returns(ClassBasedBasicCommandActivator.Instance);
                var classBasedCommandObjectBuilder =
                    (ClassBasedCommandObjectBuilder)testCommandType.CreateCommandObjectBuilder(serviceProviderMock.Object);
                var testCommand = (TestCommand)((IClassBasedCommandObject)classBasedCommandObjectBuilder.GenerateCommandObject()).Command;

                // Act
                var actual = classBasedCommandObjectBuilder.FindOption(optionName);

                // Assert
                Assert.NotNull(actual);
                Assert.NotNull(testCommand);
                Assert.True(actual.ShouldProvideValue);
                actual.AssignValue("42");
                Assert.Single(testCommand.PropertyList);
                Assert.Equal(42, testCommand.PropertyList.First());

            }

            internal class TestCommand : ICommand
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
