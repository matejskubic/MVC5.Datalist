using Datalist;
using NUnit.Framework;
using System;

namespace DatalistTests.Tests
{
    [TestFixture]
    public class DatalistColumnAttributeTests
    {
        #region Constructor: DatalistColumnAttribute()

        [Test]
        public void DatalistColumnAttribute_NullPosition()
        {
            Assert.IsNull(new DatalistColumnAttribute().Position);
        }

        [Test]
        public void DatalistColumnAttribute_NullRelation()
        {
            Assert.IsNull(new DatalistColumnAttribute().Relation);
        }

        [Test]
        public void DatalistColumnAttribute_NullFormat()
        {
            Assert.IsNull(new DatalistColumnAttribute().Format);
        }

        #endregion

        #region Constructor: DatalistColumnAttribute(Int32 position)

        [Test]
        public void DatalistColumnAttribute_SetsPosition()
        {
            Int32? actual = new DatalistColumnAttribute(-5).Position;
            Int32? expected = -5;

            Assert.AreEqual(expected, actual);
        }

        #endregion
    }
}
