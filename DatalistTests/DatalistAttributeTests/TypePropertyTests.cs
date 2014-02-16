using Datalist;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace DatalistTests.DatalistAttributeTests
{
    [TestClass]
    public class TypePropertyTests
    {
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
        public void GetterTest()
        {
            var expected = typeof(AbstractDatalist);
            Assert.AreEqual(expected, new DatalistAttribute(expected).Type);
        }
    }
}
