using System;
using Xunit;

namespace Datalist.Tests.Unit
{
    public class DatalistColumnAttributeTests
    {
        #region Constructor: DatalistColumnAttribute()

        [Fact]
        public void DatalistColumnAttribute_NullPosition()
        {
            Assert.Null(new DatalistColumnAttribute().Position);
        }

        [Fact]
        public void DatalistColumnAttribute_NullRelation()
        {
            Assert.Null(new DatalistColumnAttribute().Relation);
        }

        [Fact]
        public void DatalistColumnAttribute_NullFormat()
        {
            Assert.Null(new DatalistColumnAttribute().Format);
        }

        #endregion

        #region Constructor: DatalistColumnAttribute(Int32 position)

        [Fact]
        public void DatalistColumnAttribute_SetsPosition()
        {
            Int32? actual = new DatalistColumnAttribute(-5).Position;
            Int32? expected = -5;

            Assert.Equal(expected, actual);
        }

        #endregion
    }
}
