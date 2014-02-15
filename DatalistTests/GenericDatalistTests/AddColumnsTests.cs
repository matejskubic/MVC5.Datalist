using DatalistTests.TestContext.Models;
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
        public void KeysTest()
        {
            var row = new Dictionary<String, String>();
            Datalist.BaseAddColumns(row, new TestModel(1));

            CollectionAssert.AreEqual(Datalist.Columns.Keys, row.Keys);
        }

        [TestMethod]
        public void KeyCountTest()
        {
            var row = new Dictionary<String, String>();
            Datalist.BaseAddColumns(row, new TestModel(1));

            Assert.AreEqual(Datalist.Columns.Count, row.Keys.Count);
        }

        [TestMethod]
        public void ValuesTest()
        {
            var model = new TestModel(1);
            var expected = new List<String>();
            var row = new Dictionary<String, String>();
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
    }
}
