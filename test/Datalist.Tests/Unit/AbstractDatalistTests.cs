using NSubstitute;
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

        #region AbstractDatalist()

        [Fact]
        public void AbstractDatalist_Defaults()
        {
            AbstractDatalist actual = Substitute.For<AbstractDatalist>();

            Assert.Equal<UInt32>(20, actual.DefaultRows);
            Assert.Empty(actual.AdditionalFilters);
            Assert.NotNull(actual.Filter);
            Assert.Empty(actual.Columns);
        }

        #endregion
    }
}
