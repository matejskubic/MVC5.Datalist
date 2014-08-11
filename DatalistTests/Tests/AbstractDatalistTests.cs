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
        private AbstractDatalist datalist;
        private String baseUrl;

        [SetUp]
        public void SetUp()
        {
            baseUrl = "http://localhost:7013/";
            HttpRequest request = new HttpRequest(null, baseUrl, null);
            HttpResponse response = new HttpResponse(new StringWriter());
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
        public void Prefix_IsConstant()
        {
            Assert.AreEqual("Datalist", AbstractDatalist.Prefix);
        }

        [Test]
        public void IdKey_IsConstant()
        {
            Assert.AreEqual("DatalistIdKey", AbstractDatalist.IdKey);
        }

        [Test]
        public void AcKey_IsConstant()
        {
            Assert.AreEqual("DatalistAcKey", AbstractDatalist.AcKey);
        }

        #endregion

        #region Constructor: AbstractDatalist()

        [Test]
        public void AbstractDatalist_DefaultDialogTitle()
        {
            String expected = datalist.GetType().Name.Replace(AbstractDatalist.Prefix, String.Empty);
            String actual = datalist.DialogTitle;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void AbstractDatalist_DefaultDatalistUrl()
        {
            String expected = String.Format("{0}{1}/{2}", baseUrl, AbstractDatalist.Prefix, datalist.GetType().Name.Replace(AbstractDatalist.Prefix, String.Empty));
            String actual = datalist.DatalistUrl;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void AbstractDatalist_NullDefaultSortColumn()
        {
            Assert.IsNull(datalist.DefaultSortColumn);
        }

        [Test]
        public void AbstractDatalist_DefaultDefaultRecordsPerPage()
        {
            UInt32 actual = datalist.DefaultRecordsPerPage;
            UInt32 expected = 20;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void AbstractDatalist_EmptyAdditionalFilters()
        {
            CollectionAssert.IsEmpty(datalist.AdditionalFilters);
        }

        [Test]
        public void AbstractDatalist_AscDefaultSortOrder()
        {
            DatalistSortOrder actual = datalist.DefaultSortOrder;
            DatalistSortOrder expected = DatalistSortOrder.Asc;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void AbstractDatalist_EmptyColumns()
        {
            CollectionAssert.IsEmpty(datalist.Columns);
        }

        [Test]
        public void AbstractDatalist_NotNullCurrentFilter()
        {
            Assert.IsNotNull(datalist.CurrentFilter);
        }

        #endregion
    }
}
