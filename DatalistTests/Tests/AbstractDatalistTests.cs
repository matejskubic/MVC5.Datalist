using Datalist;
using Moq;
using NUnit.Framework;
using System;
using System.IO;
using System.Web;

namespace DatalistTests.Tests
{
    [TestFixture]
    public class AbstractDatalistTests
    {
        private String baseUrl;
        private AbstractDatalist datalist;

        [SetUp]
        public void SetUp()
        {
            baseUrl = "http://localhost:7013/";
            var request = new HttpRequest(null, baseUrl, null);
            var response = new HttpResponse(new StringWriter());
            HttpContext.Current = new HttpContext(request, response);
            datalist = new Mock<AbstractDatalist>().Object;
        }

        [TearDown]
        public void TearDown()
        {
            HttpContext.Current = null;
        }

        #region Constants

        [Test]
        public void Prefix()
        {
            Assert.AreEqual("Datalist", AbstractDatalist.Prefix);
        }

        [Test]
        public void IdKey()
        {
            Assert.AreEqual("DatalistIdKey", AbstractDatalist.IdKey);
        }

        [Test]
        public void AcKey()
        {
            Assert.AreEqual("DatalistAcKey", AbstractDatalist.AcKey);
        }

        #endregion

        #region Constructor: AbstractDatalist()

        [Test]
        public void DialogTitle()
        {
            String expected = datalist.GetType().Name.Replace(AbstractDatalist.Prefix, String.Empty);
            Assert.AreEqual(expected, datalist.DialogTitle);
        }

        [Test]
        public void DatalistUrl()
        {
            String expected = String.Format("{0}{1}/{2}", baseUrl, AbstractDatalist.Prefix, datalist.GetType().Name.Replace(AbstractDatalist.Prefix, String.Empty));
            Assert.AreEqual(expected, datalist.DatalistUrl);
        }

        [Test]
        public void DefaultSortColumn()
        {
            Assert.IsNull(datalist.DefaultSortColumn);
        }

        [Test]
        public void DefaultRecordsPerPage()
        {
            Assert.AreEqual((UInt32)20, datalist.DefaultRecordsPerPage);
        }

        [Test]
        public void AdditionalFilters()
        {
            Assert.AreEqual(0, datalist.AdditionalFilters.Count);
        }

        [Test]
        public void DefaultSortOrder()
        {
            Assert.AreEqual(DatalistSortOrder.Asc, datalist.DefaultSortOrder);
        }

        [Test]
        public void Columns()
        {
            Assert.AreEqual(0, datalist.Columns.Count);
        }

        [Test]
        public void CurrentFilter()
        {
            Assert.IsNotNull(datalist.CurrentFilter);
        }

        #endregion
    }
}
