using Datalist;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DatalistTests.DatalistFilterTests
{
    [TestClass]
    public class DefaultsTests
    {
        private DatalistFilter filter;

        [TestInitialize]
        public void TestInit()
        {
            filter = new DatalistFilter();
        }

        
        [TestMethod]
        public void IdTest()
        {
            Assert.IsNull(filter.Id);
        }

        [TestMethod]
        public void PageTest()
        {
            Assert.AreEqual(0, filter.Page);
        }

        [TestMethod]
        public void SearchTermTest()
        {
            Assert.IsNull(filter.SearchTerm);
        }

        [TestMethod]
        public void SortColumnTest()
        {
            Assert.IsNull(filter.SortColumn);
        }

        [TestMethod]
        public void SortOrderTest()
        {
            Assert.AreEqual(DatalistSortOrder.Asc, filter.SortOrder);
        }

        [TestMethod]
        public void RecordsPerPageTest()
        {
            Assert.AreEqual(0, filter.RecordsPerPage);
        }

        [TestMethod]
        public void AdditionalFiltersTest()
        {
            Assert.AreEqual(0, filter.AdditionalFilters.Count);
        }
    }
}
