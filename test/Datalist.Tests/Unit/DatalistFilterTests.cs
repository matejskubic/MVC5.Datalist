using Xunit;

namespace Datalist.Tests.Unit
{
    public class DatalistFilterTests
    {
        private DatalistFilter filter;

        public DatalistFilterTests()
        {
            filter = new DatalistFilter();
        }

        #region Constructor: DatalistFilter()

        [Fact]
        public void DatalistFilter_NullId()
        {
            Assert.Null(filter.Id);
        }

        [Fact]
        public void DatalistFilter_ZeroPage()
        {
            Assert.Equal(0, filter.Page);
        }

        [Fact]
        public void DatalistFilter_NullSearchTerm()
        {
            Assert.Null(filter.SearchTerm);
        }

        [Fact]
        public void DatalistFilter_NullSortColumn()
        {
            Assert.Null(filter.SortColumn);
        }

        [Fact]
        public void DatalistFilter_AscSortOrder()
        {
            Assert.Equal(DatalistSortOrder.Asc, filter.SortOrder);
        }

        [Fact]
        public void DatalistFilter_ZeroRecordsPerPage()
        {
            Assert.Equal(0, filter.RecordsPerPage);
        }

        [Fact]
        public void DatalistFilter_EmptyAdditionalFilters()
        {
            Assert.Empty(filter.AdditionalFilters);
        }

        #endregion
    }
}
