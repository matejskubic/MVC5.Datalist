using Datalist;
using DatalistTests.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Reflection;

namespace DatalistTests.GenericDatalistTests
{
    [TestClass]
    public class GetColumnKeyTests : BaseTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NullPropertyTest()
        {
            Datalist.BaseGetColumnKey(null);
        }

        [TestMethod]
        public void NoAttributeTest()
        {
            PropertyInfo property = typeof(TestModel).GetProperty("Sum");
            String actual = Datalist.BaseGetColumnKey(property);
            String expected = property.Name;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [ExpectedException(typeof(DatalistException))]
        public void NotExistingRelationTest()
        {
            PropertyInfo property = typeof(NoRelationModel).GetProperty("NoRelation");
            Datalist.BaseGetColumnKey(property);
        }

        [TestMethod]
        public void RelationPropertyTest()
        {
            PropertyInfo property = typeof(TestModel).GetProperty("FirstRelationModel");
            String expected = String.Format("{0}.{1}", property.Name, property.GetCustomAttribute<DatalistColumnAttribute>(false).Relation);
            String actual = Datalist.BaseGetColumnKey(property);

            Assert.AreEqual(expected, actual);
        }
    }
}
