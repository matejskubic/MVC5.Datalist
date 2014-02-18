using Datalist;
using NUnit.Framework;

namespace DatalistTests.Tests
{
    [TestFixture]
    public class DefaultsTests
    {
        private DatalistFilter filter;

        [SetUp]
        public void SetUp()
        {
            filter = new DatalistFilter();
        }

        #region Constructor: DatalistFilter()

        [Test]
        public void Id()
        {
            Assert.IsNull(filter.Id);
        }

        [Test]
        public void Page()
        {
            Assert.AreEqual(0, filter.Page);
        }

        [Test]
        public void SearchTerm()
        {
            Assert.IsNull(filter.SearchTerm);
        }

        [Test]
        public void SortColumn()
        {
            Assert.IsNull(filter.SortColumn);
        }

        [Test]
        public void SortOrder()
        {
            Assert.AreEqual(DatalistSortOrder.Asc, filter.SortOrder);
        }

        [Test]
        public void RecordsPerPage()
        {
            Assert.AreEqual(0, filter.RecordsPerPage);
        }

        [Test]
        public void AdditionalFilters()
        {
            Assert.AreEqual(0, filter.AdditionalFilters.Count);
        }

        #endregion
    }
}
