using Moq;
using System;
using System.IO;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Xunit;

namespace Datalist.Tests.Unit
{
    public class AbstractDatalistTests : IDisposable
    {
        private AbstractDatalist datalist;

        public AbstractDatalistTests()
        {
            RouteTable.Routes.Clear();
            RouteTable.Routes.MapRoute(
                "Default",
                "{controller}/{action}/{id}",
                new { controller = "Home", action = "Index", id = "" });

            HttpRequest request = new HttpRequest(null, "http://localhost:7013/", null);
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
            String expected = $"/{AbstractDatalist.Prefix}/{datalist.GetType().Name.Replace(AbstractDatalist.Prefix, "")}";
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
