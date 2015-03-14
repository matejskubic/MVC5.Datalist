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
            Assert.Equal("Datalist", AbstractDatalist.Prefix);
        }

        [Fact]
        public void IdKey_IsConstant()
        {
            Assert.Equal("DatalistIdKey", AbstractDatalist.IdKey);
        }

        [Fact]
        public void AcKey_IsConstant()
        {
            Assert.Equal("DatalistAcKey", AbstractDatalist.AcKey);
        }

        #endregion

        #region Constructor: AbstractDatalist()

        [Fact]
        public void AbstractDatalist_DefaultDialogTitle()
        {
            String expected = datalist.GetType().Name.Replace(AbstractDatalist.Prefix, "");
            String actual = datalist.DialogTitle;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void AbstractDatalist_DefaultDatalistUrl()
        {
            String expected = String.Format("{0}{1}/{2}", baseUrl, AbstractDatalist.Prefix, datalist.GetType().Name.Replace(AbstractDatalist.Prefix, ""));
            String actual = datalist.DatalistUrl;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void AbstractDatalist_NullDefaultSortColumn()
        {
            Assert.Null(datalist.DefaultSortColumn);
        }

        [Fact]
        public void AbstractDatalist_DefaultDefaultRecordsPerPage()
        {
            UInt32 actual = datalist.DefaultRecordsPerPage;
            UInt32 expected = 20;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void AbstractDatalist_EmptyAdditionalFilters()
        {
            Assert.Empty(datalist.AdditionalFilters);
        }

        [Fact]
        public void AbstractDatalist_AscDefaultSortOrder()
        {
            DatalistSortOrder actual = datalist.DefaultSortOrder;
            DatalistSortOrder expected = DatalistSortOrder.Asc;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void AbstractDatalist_EmptyColumns()
        {
            Assert.Empty(datalist.Columns);
        }

        [Fact]
        public void AbstractDatalist_NotNullCurrentFilter()
        {
            Assert.NotNull(datalist.CurrentFilter);
        }

        #endregion
    }
}
