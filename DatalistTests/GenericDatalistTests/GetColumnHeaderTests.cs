using Datalist;
using DatalistTests.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace DatalistTests.GenericDatalistTests
{
    [TestClass]
    public class GetColumnHeaderTests : BaseTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NullPropertyTest()
        {
            Datalist.BaseGetColumnHeader(null);
        }

        [TestMethod]
        public void NoAttributeTest()
        {
            PropertyInfo property = typeof(TestModel).GetProperty("Sum");
            String actual = Datalist.BaseGetColumnHeader(property);
            String expected = property.Name;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void SingleDisplayPropertyTest()
        {
            PropertyInfo property = typeof(TestModel).GetProperty("Number");
            String expected = property.GetCustomAttribute<DisplayAttribute>().Name;
            String actual = Datalist.BaseGetColumnHeader(property);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [ExpectedException(typeof(DatalistException))]
        public void NotExistingRelationTest()
        {
            PropertyInfo property = typeof(NoRelationModel).GetProperty("NoRelation");
            Datalist.BaseGetColumnHeader(property);
        }

        [TestMethod]
        public void RelationPropertyTest()
        {
            PropertyInfo property = typeof(TestModel).GetProperty("SecondRelationModel");
            String expected = property.GetCustomAttribute<DatalistColumnAttribute>(false).Relation;
            String actual = Datalist.BaseGetColumnHeader(property);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void RelationDisplayPropertyTest()
        {
            PropertyInfo property = typeof(TestModel).GetProperty("FirstRelationModel");
            String expected = property.PropertyType.GetProperty("Value").GetCustomAttribute<DisplayAttribute>().Name;
            String actual = Datalist.BaseGetColumnHeader(property);

            Assert.AreEqual(expected, actual);
        }
    }
}
