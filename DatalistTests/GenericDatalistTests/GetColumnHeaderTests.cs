using Datalist;
using DatalistTests.GenericDatalistTests.Stubs;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Reflection;

namespace DatalistTests.GenericDatalistTests
{
    [TestClass]
    public class GetColumnHeaderTests : GenericDatalistTests
    {
        #region Tests

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NullPropertyTest()
        {
            Datalist.BaseGetColumnHeader(null);
        }

        [TestMethod]
        public void NoAttributeTest()
        {
            PropertyInfo property = typeof(DatalistModel).GetProperty("Sum");
            String actual = Datalist.BaseGetColumnHeader(property);
            String expected = property.Name;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void SinglePropertyTest()
        {
            PropertyInfo property = typeof(DatalistModel).GetProperty("CreationDate");
            String actual = Datalist.BaseGetColumnHeader(property);
            String expected = property.Name;

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void SingleDisplayPropertyTest()
        {
            PropertyInfo property = typeof(DatalistModel).GetProperty("Number");
            String actual = Datalist.BaseGetColumnHeader(property);
            String expected = DatalistModel.DisplayValue;

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
            PropertyInfo property = typeof(DatalistModel).GetProperty("SecondRelationModel");
            String expected = property.GetCustomAttribute<DatalistColumnAttribute>(false).Relation;
            String actual = Datalist.BaseGetColumnHeader(property);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void RelationDisplayPropertyTest()
        {
            PropertyInfo property = typeof(DatalistModel).GetProperty("FirstRelationModel");
            String actual = Datalist.BaseGetColumnHeader(property);
            String expected = DatalistRelationModel.DisplayValue;

            Assert.AreEqual(expected, actual);
        }

        #endregion
    }
}
