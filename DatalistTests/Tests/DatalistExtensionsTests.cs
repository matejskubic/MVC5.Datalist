using DatalistTests.Objects.Models;
using DatalistTests.Objects.Stubs;
using Moq;
using NUnit.Framework;
using System.Collections;
using System.IO;
using System.Web;
using System.Web.Mvc;

namespace DatalistTests
{
    [TestFixture]
    public class DatalistExtensionsTests
    {
        private Mock<TestDatalistStub> datalistMock;
        private HtmlHelper<TestModel> html;
        private TestDatalistStub datalist;

        [SetUp]
        public virtual void SetUp()
        {
            HttpContext.Current = new HttpContext(
                new HttpRequest(null, "http://localhost:7013/", null),
                new HttpResponse(new StringWriter()));

            datalistMock = new Mock<TestDatalistStub>() { CallBase = true };
            datalist = datalistMock.Object;
            html = MockHtmlHelper();
        }

        [TearDown]
        public virtual void TearDown()
        {
            HttpContext.Current = null;
        }
        
        #region Test helpers

        private static HtmlHelper<TestModel> MockHtmlHelper()
        {
            ViewDataDictionary<TestModel> viewData = new ViewDataDictionary<TestModel>();
            viewData.Model = new TestModel();

            Mock<IViewDataContainer> mockContainer = new Mock<IViewDataContainer>();
            mockContainer.Setup(c => c.ViewData).Returns(viewData);

            Mock<ViewContext> mockViewContext = new Mock<ViewContext>() { CallBase = true };
            mockViewContext.Setup(c => c.ViewData).Returns(viewData);
            mockViewContext.Setup(c => c.HttpContext.Items).Returns(new Hashtable());

            return new HtmlHelper<TestModel>(mockViewContext.Object, mockContainer.Object);
        }

        #endregion
    }
}