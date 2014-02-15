using Datalist;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace DatalistTests.DatalistColumnTests
{
    [TestClass]
    public class ConstructorTests
    {
        #region Tests

        [TestMethod]
        public void ParameterlessPositionTest()
        {
            var column = new DatalistColumnAttribute();
            Assert.IsNull(column.Position);
        }

        [TestMethod]
        public void ParameterlessRelationTest()
        {
            var column = new DatalistColumnAttribute();
            Assert.IsNull(column.Relation);
        }

        [TestMethod]
        public void PositionTest()
        {
            Int32 expected = -5;
            var column = new DatalistColumnAttribute(expected);
            Assert.AreEqual(expected, column.Position);
        }

        [TestMethod]
        public void RelationTest()
        {
            var column = new DatalistColumnAttribute();
            var expected = "TestRelation";
            column.Relation = expected;

            Assert.AreEqual(expected, column.Relation);
        }

        #endregion
    }
}
