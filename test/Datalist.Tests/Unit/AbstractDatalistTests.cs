using NSubstitute;
using Xunit;

namespace Datalist.Tests.Unit
{
    public class AbstractDatalistTests
    {
        #region Constants

        [Fact]
        public void Prefix_IsConstant()
        {
            Assert.True(typeof(AbstractDatalist).GetField("Prefix").IsLiteral);
            Assert.Equal("Datalist", AbstractDatalist.Prefix);
        }

        [Fact]
        public void IdKey_IsConstant()
        {
            Assert.True(typeof(AbstractDatalist).GetField("IdKey").IsLiteral);
            Assert.Equal("DatalistIdKey", AbstractDatalist.IdKey);
        }

        [Fact]
        public void AcKey_IsConstant()
        {
            Assert.True(typeof(AbstractDatalist).GetField("AcKey").IsLiteral);
            Assert.Equal("DatalistAcKey", AbstractDatalist.AcKey);
        }

        #endregion

        #region AbstractDatalist()

        [Fact]
        public void AbstractDatalist_Defaults()
        {
            AbstractDatalist actual = Substitute.For<AbstractDatalist>();

            Assert.Empty(actual.AdditionalFilters);
            Assert.Equal(20, actual.Filter.Rows);
            Assert.NotNull(actual.Filter);
            Assert.Empty(actual.Columns);
        }

        #endregion
    }
}
