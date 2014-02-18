using Datalist;
using NUnit.Framework;

namespace DatalistTests.Tests
{
    [TestFixture]
    public class DatalistColumnAttributeTests
    {
        #region Constructor: DatalistColumnAttribute()

        [Test]
        public void ParameterlessPosition()
        {
            Assert.IsNull(new DatalistColumnAttribute().Position);
        }

        [Test]
        public void ParameterlessRelation()
        {
            Assert.IsNull(new DatalistColumnAttribute().Relation);
        }

        #endregion

        #region Constructor: DatalistColumnAttribute(Int32 position)

        [Test]
        public void Position()
        {
            Assert.AreEqual(-5, new DatalistColumnAttribute(-5).Position);
        }

        #endregion
    }
}
