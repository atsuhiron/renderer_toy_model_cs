using RendererToyModelCs.Extension;

namespace RendererToyModelCsTests.Extension
{
    public class EnumerableExtTests
    {
        [Fact]
        public void WhereNotNullTest()
        {
            IEnumerable<string?> list = new List<string?>() { "1", "2", null, "4" };
            List<string> actual = list.WhereNotNull().ToList();

            Assert.Equal(3, actual.Count);
            Assert.NotNull(actual[0]);
            Assert.NotNull(actual[1]);
            Assert.NotNull(actual[2]);
        }
    }
}
