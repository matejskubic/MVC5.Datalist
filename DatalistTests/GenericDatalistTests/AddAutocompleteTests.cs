using Datalist;
using DatalistTests.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DatalistTests.GenericDatalistTests
{
    [TestClass]
    public class AddAutocompleteTests : BaseTests
    {
        [TestMethod]
        [ExpectedException(typeof(DatalistException))]
        public void NoColumnsTest()
        {
            Datalist.Columns.Clear();
            Datalist.BaseAddAutocomplete(null, new TestModel());
        }

        [TestMethod]
        [ExpectedException(typeof(DatalistException))]
        public void NoPropertyTest()
        {
            Datalist.Columns.Clear();
            Datalist.Columns.Add("TestProperty", String.Empty);
            Datalist.BaseAddAutocomplete(new Dictionary<String, String>(), new TestModel());
        }

        [TestMethod]
        public void KeyTest()
        {
            var row = new Dictionary<String, String>();
            Datalist.BaseAddAutocomplete(row, new TestModel());

            Assert.AreEqual(AbstractDatalist.AcKey, row.First().Key);
        }

        [TestMethod]
        public void ValueTest()
        {
            var model = new TestModel();
            var row = new Dictionary<String, String>();
            var firstProperty = model.GetType().GetProperty(Datalist.Columns.First().Key);
            Datalist.BaseAddAutocomplete(row, model);

            Assert.AreEqual(firstProperty.GetValue(model).ToString(), row.First().Value);
        }

        [TestMethod]
        public void RelationValueTest()
        {
            Datalist.Columns.Clear();
            Datalist.Columns.Add("FirstRelationModel.Value", String.Empty);
            var model = new TestModel() { FirstRelationModel = new TestRelationModel() { Value = "Test" } };
            var firstProperty = typeof(TestRelationModel).GetProperty("Value");
            var row = new Dictionary<String, String>();
            Datalist.BaseAddAutocomplete(row, model);

            Assert.AreEqual(firstProperty.GetValue(model.FirstRelationModel).ToString(), row.First().Value);
        }

        [TestMethod]
        public void KeyCountTest()
        {
            var row = new Dictionary<String, String>();
            Datalist.BaseAddAutocomplete(row, new TestModel());

            Assert.AreEqual(1, row.Keys.Count);
        }
    }
}
