using Datalist;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.IO;
using System.Web;

namespace DatalistTests.DatalistAttributeTests
{
    [TestClass]
    public class TypePropertyTests
    {
        #region

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NullTest()
        {
            new DatalistAttribute(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TypeTest()
        {
            new DatalistAttribute(typeof(Object));
        }

        [TestMethod]
        public void SetterGetterTest()
        {
            var request = new HttpRequest(null, "http://localhost:7013/", null);
            var response = new HttpResponse(new StringWriter());
            HttpContext.Current = new HttpContext(request, response);
            var datalist = new Mock<AbstractDatalist>().Object;

            var attribute = new DatalistAttribute(datalist.GetType());
            var expected = datalist.GetType();
            HttpContext.Current = null;

            Assert.AreEqual(expected, attribute.Type);
        }

        #endregion
    }
}
