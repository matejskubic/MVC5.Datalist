using Datalist;
using DatalistTests.GenericDatalistTests.Stubs;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DatalistTests.GenericDatalistTests
{
    [TestClass]
    public class AddIdTests : GenericDatalistTests
    {
        [TestMethod]
        public void KeyTest()
        {
            var row = new Dictionary<String, String>();
            Datalist.BaseAddId(row, new DatalistModel(1));

            Assert.AreEqual(AbstractDatalist.IdKey, row.First().Key);
        }

        [TestMethod]
        public void KeyCountTest()
        {
            var row = new Dictionary<String, String>();
            Datalist.BaseAddId(row, new DatalistModel(1));

            Assert.AreEqual(1, row.Keys.Count);
        }

        [TestMethod]
        public void ValueTest()
        {
            var row = new Dictionary<String, String>();
            var model = new DatalistModel(1);
            Datalist.BaseAddId(row, model);

            Assert.AreEqual(model.Id, row.First().Value);
        }
    }
}
