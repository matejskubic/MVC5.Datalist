﻿using Datalist;
using DatalistTests.TestContext.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DatalistTests.GenericDatalistTests
{
    [TestClass]
    public class AddAutocompleteTests : GenericDatalistTests
    {
        [TestMethod]
        public void KeyTest()
        {
            var row = new Dictionary<String, String>();
            Datalist.BaseAddAutocomplete(row, new TestModel(1));

            Assert.AreEqual(AbstractDatalist.AcKey, row.First().Key);
        }

        [TestMethod]
        public void KeyCountTest()
        {
            var row = new Dictionary<String, String>();
            Datalist.BaseAddAutocomplete(row, new TestModel(1));

            Assert.AreEqual(1, row.Keys.Count);
        }

        [TestMethod]
        public void ValueTest()
        {
            var firstProperty = typeof(TestModel).GetProperty(Datalist.Columns.First().Key);
            var row = new Dictionary<String, String>();
            var model = new TestModel(1);
            Datalist.BaseAddAutocomplete(row, model);

            Assert.AreEqual(firstProperty.GetValue(model).ToString(), row.First().Value);
        }

        [TestMethod]
        [ExpectedException(typeof(DatalistException))]
        public void NoColumnsTest()
        {
            Datalist.Columns.Clear();
            var model = new TestModel(1);
            var row = new Dictionary<String, String>();

            Datalist.BaseAddAutocomplete(row, model);
        }
    }
}
