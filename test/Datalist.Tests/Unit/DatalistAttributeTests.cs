using System;
using Xunit;

namespace Datalist.Tests.Unit
{
    public class DatalistAttributeTests
    {
        #region Constructor: DatalistAttribute(Type type)

        [Fact]
        public void DatalistAttribute_NullThrows()
        {
            Assert.Throws<ArgumentNullException>(() => new DatalistAttribute(null));
        }

        [Fact]
        public void DatalistAttribute_UnassignableTypeThrows()
        {
            Assert.Throws<ArgumentException>(() => new DatalistAttribute(typeof(Object)));
        }

        [Fact]
        public void DatalistAttribute_SetsType()
        {
            Type actual = new DatalistAttribute(typeof(AbstractDatalist)).Type;
            Type expected = typeof(AbstractDatalist);

            Assert.Equal(expected, actual);
        }

        #endregion
    }
}
