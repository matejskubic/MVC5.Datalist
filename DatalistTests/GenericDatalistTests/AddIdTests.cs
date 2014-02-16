using Datalist;
using DatalistTests.GenericDatalistTests.Stubs;
using DatalistTests.TestContext.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DatalistTests.GenericDatalistTests
{
    [TestClass]
    public class AddIdTests : BaseTests
    {
        [TestMethod]
        public void KeyTest()
        {
            var row = new Dictionary<String, String>();
            Datalist.BaseAddId(row, new TestModel());

            Assert.AreEqual(AbstractDatalist.IdKey, row.First().Key);
        }

        [TestMethod]
        public void KeyCountTest()
        {
            var row = new Dictionary<String, String>();
            Datalist.BaseAddId(row, new TestModel());

            Assert.AreEqual(1, row.Keys.Count);
        }

        [TestMethod]
        public void ValueTest()
        {
            var row = new Dictionary<String, String>();
            var model = new TestModel() { Id = "Test" };

            Datalist.BaseAddId(row, model);

            Assert.AreEqual(model.Id, row.First().Value);
        }

        [TestMethod]
        [ExpectedException(typeof(DatalistException))]
        public void NoIdPropertyTest()
        {
            var datalist = new GenericDatalistStub<NoIdModel>();
            var row = new Dictionary<String, String>();
            var model = new NoIdModel();

            datalist.BaseAddId(row, model);
        }
    }
}
