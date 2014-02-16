using Datalist;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DatalistTests.DatalistColumnTests
{
    [TestClass]
    public class ConstructorTests
    {
        [TestMethod]
        public void ParameterlessPositionTest()
        {
            Assert.IsNull(new DatalistColumnAttribute().Position);
        }

        [TestMethod]
        public void ParameterlessRelationTest()
        {
            Assert.IsNull(new DatalistColumnAttribute().Relation);
        }

        [TestMethod]
        public void PositionTest()
        {
            Assert.AreEqual(-5, new DatalistColumnAttribute(-5).Position);
        }
    }
}
