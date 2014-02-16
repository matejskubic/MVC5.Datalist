using DatalistTests.Stubs;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.IO;
using System.Web;

namespace DatalistTests.GenericDatalistTests
{
    [TestClass]
    public class BaseTests
    {
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

        [TestInitialize]
        public virtual void TestInit()
        {
            var request = new HttpRequest(null, "http://localhost:7013/", null);
            var response = new HttpResponse(new StringWriter());
            HttpContext.Current = new HttpContext(request, response);
            DatalistMock = new Mock<TestDatalistStub>() { CallBase = true };
            Datalist = DatalistMock.Object;
        }

        [TestCleanup]
        public virtual void TestCleanUp()
        {
            HttpContext.Current = null;
        }
    }
}
