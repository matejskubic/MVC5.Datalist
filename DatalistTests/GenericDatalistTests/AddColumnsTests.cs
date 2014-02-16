using Datalist;
using DatalistTests.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DatalistTests.GenericDatalistTests
{
    [TestClass]
    public class AddColumnsTests : BaseTests
    {
        [TestMethod]
        [ExpectedException(typeof(DatalistException))]
        public void NoColumnsTest()
        {
            Datalist.Columns.Clear();
            Datalist.BaseAddColumns(null, new TestModel());
        }

        [TestMethod]
        [ExpectedException(typeof(DatalistException))]
        public void NoPropertyTest()
        {
            Datalist.Columns.Clear();
            Datalist.Columns.Add("TestProperty", String.Empty);
            Datalist.BaseAddColumns(new Dictionary<String, String>(), new TestModel());
        }

        [TestMethod]
        public void KeysTest()
        {
            var row = new Dictionary<String, String>();
            Datalist.BaseAddColumns(row, new TestModel());

            CollectionAssert.AreEqual(Datalist.Columns.Keys, row.Keys);
        }

        [TestMethod]
        public void ValuesTest()
        {
            var expected = new List<String>();
            var row = new Dictionary<String, String>();
            var model = new TestModel() { FirstRelationModel = new TestRelationModel() { Value = "Test" } };
            foreach (KeyValuePair<String, String> column in Datalist.Columns)
                expected.Add(GetValue(model, column.Key));

            Datalist.BaseAddColumns(row, model);

            CollectionAssert.AreEqual(expected, row.Values);
        }
        private String GetValue(Object model, String fullPropertyName)
        {
            if (model == null) return String.Empty;

            Object value = null;
            Type type = model.GetType();
            String[] properties = fullPropertyName.Split('.');
            var property = type.GetProperty(properties[0]);

            if (properties.Length == 1)
                value = property.GetValue(model);
            else
                value = GetValue(property.GetValue(model), String.Join(".", properties.Skip(1)));

            return value != null ? value.ToString() : String.Empty;
        }

        [TestMethod]
        public void KeyCountTest()
        {
            var row = new Dictionary<String, String>();
            Datalist.BaseAddColumns(row, new TestModel());

            Assert.AreEqual(Datalist.Columns.Count, row.Keys.Count);
        }
    }
}
