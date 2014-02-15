using DatalistTests.GenericDatalistTests.Stubs;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.IO;
using System.Web;

namespace DatalistTests.GenericDatalistTests
{
    [TestClass]
    public class GenericDatalistTests
    {
        #region Set up / Tear down

        protected Mock<GenericDatalistStub<DatalistModel>> DatalistMock
        {
            get;
            set;
        }
        protected GenericDatalistStub<DatalistModel> Datalist
        {
            get;
            set;
        }

        [TestInitialize]
        public virtual void TestInit()
        {
            var request = new HttpRequest(null, "http://localhost:7013/", null);
            var response = new HttpResponse(new StringWriter());
            HttpContext.Current = new HttpContext(request, response);
            DatalistMock = new Mock<GenericDatalistStub<DatalistModel>>() { CallBase = true };
            Datalist = DatalistMock.Object;

            for (Int32 i = 0; i < 100; i++)
                Datalist.Models.Add(new DatalistModel(i));
        }

        [TestCleanup]
        public virtual void TestCleanUp()
        {
            HttpContext.Current = null;
        }

        #endregion
    }
}
