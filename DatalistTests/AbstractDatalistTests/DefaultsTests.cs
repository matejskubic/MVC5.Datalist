using Datalist;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.IO;
using System.Web;

namespace DatalistTests.AbstractDatalistTests
{
    [TestClass]
    public class DefaultsTests
    {
        private String baseUrl;
        private AbstractDatalist datalist;

        [TestInitialize]
        public void TestInit()
        {
            baseUrl = "http://localhost:7013/";
            var request = new HttpRequest(null, baseUrl, null);
            var response = new HttpResponse(new StringWriter());
            HttpContext.Current = new HttpContext(request, response);
            datalist = new Mock<AbstractDatalist>().Object;
        }

        [TestCleanup]
        public void TestCleanUp()
        {
            HttpContext.Current = null;
        }


        [TestMethod]
        public void PrefixTest()
        {
            Assert.AreEqual("Datalist", AbstractDatalist.Prefix);
        }

        [TestMethod]
        public void IdKeyTest()
        {
            Assert.AreEqual("DatalistIdKey", AbstractDatalist.IdKey);
        }

        [TestMethod]
        public void AcKeyTest()
        {
            Assert.AreEqual("DatalistAcKey", AbstractDatalist.AcKey);
        }

        [TestMethod]
        public void DialogTitleTest()
        {
            String expected = datalist.GetType().Name.Replace(AbstractDatalist.Prefix, String.Empty);
            Assert.AreEqual(expected, datalist.DialogTitle);
        }

        [TestMethod]
        public void DatalistUrlTest()
        {
            String expected = String.Format("{0}{1}/{2}", baseUrl, AbstractDatalist.Prefix, datalist.GetType().Name.Replace(AbstractDatalist.Prefix, String.Empty));
            Assert.AreEqual(expected, datalist.DatalistUrl);
        }

        [TestMethod]
        public void DefaultSortColumnTest()
        {
            Assert.IsNull(datalist.DefaultSortColumn);
        }

        [TestMethod]
        public void DefaultRecordsPerPageTest()
        {
            Assert.AreEqual((UInt32)20, datalist.DefaultRecordsPerPage);
        }

        [TestMethod]
        public void AdditionalFiltersTest()
        {
            Assert.AreEqual(0, datalist.AdditionalFilters.Count);
        }

        [TestMethod]
        public void DefaultSortOrderTest()
        {
            Assert.AreEqual(DatalistSortOrder.Asc, datalist.DefaultSortOrder);
        }

        [TestMethod]
        public void ColumnsTest()
        {
            Assert.AreEqual(0, datalist.Columns.Count);
        }

        [TestMethod]
        public void CurrentFilterTest()
        {
            Assert.IsNotNull(datalist.CurrentFilter);
        }
    }
}
