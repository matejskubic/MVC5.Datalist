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

        #region DatalistFilter()

        [Fact]
        public void DatalistFilter_SetsId()
        {
            Assert.Null(filter.Id);
        }

        [Fact]
        public void DatalistFilter_SetsPage()
        {
            Assert.Equal(0, filter.Page);
        }

        [Fact]
        public void DatalistFilter_SetsSearchTerm()
        {
            Assert.Null(filter.Search);
        }

        [Fact]
        public void DatalistFilter_SetsSortColumn()
        {
            Assert.Null(filter.SortColumn);
        }

        [Fact]
        public void DatalistFilter_SetsSortOrder()
        {
            Assert.Equal(DatalistSortOrder.Asc, filter.SortOrder);
        }

        [Fact]
        public void DatalistFilter_SetsRecordsPerPage()
        {
            Assert.Equal(0, filter.Rows);
        }

        [Fact]
        public void DatalistFilter_SetsAdditionalFilters()
        {
            Assert.Empty(filter.AdditionalFilters);
        }

        #endregion
    }
}
