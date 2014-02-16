using Datalist;
using DatalistTests.Models;
using DatalistTests.Stubs;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace DatalistTests.GenericDatalistTests
{
    [TestClass]
    public class AddIdTests : BaseTests
    {
        [TestMethod]
        [ExpectedException(typeof(DatalistException))]
        public void NoIdPropertyTest()
        {
            var datalist = new GenericDatalistStub<NoIdModel>();
            var row = new Dictionary<String, String>();
            var model = new NoIdModel();

            datalist.BaseAddId(row, model);
        }

        [TestMethod]
        public void KeyTest()
        {
            var row = new Dictionary<String, String>();
            Datalist.BaseAddId(row, new TestModel());

            Assert.IsTrue(row.ContainsKey(AbstractDatalist.IdKey));
        }

        [TestMethod]
        public void ValueTest()
        {
            var row = new Dictionary<String, String>();
            var model = new TestModel() { Id = "Test" };

            Datalist.BaseAddId(row, model);

            Assert.IsTrue(row.ContainsValue(model.Id));
        }

        [TestMethod]
        public void KeyCountTest()
        {
            var row = new Dictionary<String, String>();
            Datalist.BaseAddId(row, new TestModel());

            Assert.AreEqual(1, row.Keys.Count);
        }
    }
}
