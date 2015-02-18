using NUnit.Framework;

namespace Datalist.Tests.Unit
{
    [TestFixture]
    public class DatalistFilterTests
    {
        private DatalistFilter filter;

        [SetUp]
        public void SetUp()
        {
            filter = new DatalistFilter();
        }

        #region Constructor: DatalistFilter()

        [Test]
        public void DatalistFilter_NullId()
        {
            Assert.IsNull(filter.Id);
        }

        [Test]
        public void DatalistFilter_ZeroPage()
        {
            Assert.AreEqual(0, filter.Page);
        }

        [Test]
        public void DatalistFilter_NullSearchTerm()
        {
            Assert.IsNull(filter.SearchTerm);
        }

        [Test]
        public void DatalistFilter_NullSortColumn()
        {
            Assert.IsNull(filter.SortColumn);
        }

        [Test]
        public void DatalistFilter_AscSortOrder()
        {
            Assert.AreEqual(DatalistSortOrder.Asc, filter.SortOrder);
        }

        [Test]
        public void DatalistFilter_ZeroRecordsPerPage()
        {
            Assert.AreEqual(0, filter.RecordsPerPage);
        }

        [Test]
        public void DatalistFilter_EmptyAdditionalFilters()
        {
            CollectionAssert.IsEmpty(filter.AdditionalFilters);
        }

        #endregion
    }
}
