using Datalist;
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
        private HtmlHelper<TestModel> html;

        protected Mock<TestDatalistStub> DatalistMock
        {
            get;
            private set;
        }

        protected TestDatalistStub Datalist
        {
            get;
            private set;
        }

        [SetUp]
        public virtual void SetUp()
        {
            var request = new HttpRequest(null, "http://localhost:7013/", null);
            var response = new HttpResponse(new StringWriter());
            HttpContext.Current = new HttpContext(request, response);
            DatalistMock = new Mock<TestDatalistStub>() { CallBase = true };
            Datalist = DatalistMock.Object;
            html = GetFormHelper();
        }

        [TearDown]
        public virtual void TearDown()
        {
            HttpContext.Current = null;
        }

        [Test]
        public void ItShould()
        {
            var a = html.Datalist("222", 1, new TestDatalistStub());
            Assert.AreEqual("a", a.ToString());
        }

        private static HtmlHelper<TestModel> GetFormHelper()
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
    }
}