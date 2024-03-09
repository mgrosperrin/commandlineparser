using Xunit;

namespace MGR.CommandLineParser.UnitTests.Extensions;

public partial class EnumeratorExtensionsTests
{
    public class PrependWith
    {
        [Fact]
        public void Prepend_Correctly_Add_Before_Items()
        {
            // Arrange
            var initialList = new List<string> { "Should", "Be", "Prefixed" };
            var prefix = "This";
            var expected = new List<string> { "This", "Should", "Be", "Prefixed" }.GetEnumerator();

            // Act
            var actual = initialList.GetEnumerator().PrefixWith(prefix);

            // Assert
            while (true)
            {
                var shouldHaveNext = expected.MoveNext();
                Assert.Equal(shouldHaveNext, actual.MoveNext());
                Assert.Equal(expected.Current, actual.Current);
                if (!shouldHaveNext)
                {
                    break;
                }
            }
            expected.Dispose();
            actual.Dispose();
        }
    }
}