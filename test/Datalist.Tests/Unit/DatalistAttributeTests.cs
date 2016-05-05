using System;
using Xunit;

namespace Datalist.Tests.Unit
{
    public class DatalistAttributeTests
    {
        #region DatalistAttribute(Type type)

        [Fact]
        public void DatalistAttribute_NullType_Throws()
        {
            ArgumentNullException actual = Assert.Throws<ArgumentNullException>(() => new DatalistAttribute(null));

            Assert.Equal("type", actual.ParamName);
        }

        [Fact]
        public void DatalistAttribute_NotDatalistType_Throws()
        {
            ArgumentException exception = Assert.Throws<ArgumentException>(() => new DatalistAttribute(typeof(Object)));

            String expected = $"'{typeof(Object).Name}' type does not implement '{typeof(AbstractDatalist).Name}'.";
            String actual = exception.Message;

            Assert.Equal(expected, actual);
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
