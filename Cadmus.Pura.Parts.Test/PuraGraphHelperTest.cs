using Xunit;

namespace Cadmus.Pura.Parts.Test
{
    public sealed class PuraGraphHelperTest
    {
        [Fact]
        public void GetGraphSql_NotNull()
        {
            Assert.NotNull(PuraGraphHelper.GetGraphSql());
        }
    }
}
