using Moq;
using System;
using System.IO;
using System.Web;
using Xunit;

namespace Datalist.Tests.Unit
{
    public class AbstractDatalistTests : IDisposable
    {
        private AbstractDatalist datalist;
        private String baseUrl;

        public AbstractDatalistTests()
        {
            baseUrl = "http://localhost:7013/";
            HttpRequest request = new HttpRequest(null, baseUrl, null);
            HttpResponse response = new HttpResponse(new StringWriter());
            HttpContext.Current = new HttpContext(request, response);
            datalist = new Mock<AbstractDatalist>().Object;
        }
        public void Dispose()
        {
            HttpContext.Current = null;
        }

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

        #region Constructor: AbstractDatalist()

        [Fact]
        public void AbstractDatalist_SetsDialogTitle()
        {
            String expected = datalist.GetType().Name.Replace(AbstractDatalist.Prefix, "");
            String actual = datalist.DialogTitle;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void AbstractDatalist_SetsDatalistUrl()
        {
            String expected = String.Format("{0}{1}/{2}", baseUrl, AbstractDatalist.Prefix, datalist.GetType().Name.Replace(AbstractDatalist.Prefix, ""));
            String actual = datalist.DatalistUrl;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void AbstractDatalist_SetsDefaultSortColumn()
        {
            Assert.Null(datalist.DefaultSortColumn);
        }

        [Fact]
        public void AbstractDatalist_SetsDefaultRecordsPerPage()
        {
            UInt32 actual = datalist.DefaultRecordsPerPage;
            UInt32 expected = 20;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void AbstractDatalist_SetsAdditionalFilters()
        {
            Assert.Empty(datalist.AdditionalFilters);
        }

        [Fact]
        public void AbstractDatalist_SetsDefaultSortOrder()
        {
            DatalistSortOrder actual = datalist.DefaultSortOrder;
            DatalistSortOrder expected = DatalistSortOrder.Asc;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void AbstractDatalist_SetsColumns()
        {
            Assert.Empty(datalist.Columns);
        }

        [Fact]
        public void AbstractDatalist_SetsCurrentFilter()
        {
            Assert.NotNull(datalist.CurrentFilter);
        }

        #endregion
    }
}
