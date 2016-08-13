using System;
using Xunit;

namespace Datalist.Tests.Unit
{
    public class DatalistAttributeTests
    {
        #region DatalistAttribute(Type type)

        [Fact]
        public void DatalistAttribute_NoDatalist_Throws()
        {
            ArgumentException exception = Assert.Throws<ArgumentException>(() => new DatalistAttribute(typeof(Object)));

            String expected = $"'{typeof(Object).Name}' type does not implement '{typeof(AbstractDatalist).Name}'.";
            String actual = exception.Message;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void DatalistAttribute_Type()
        {
            Type actual = new DatalistAttribute(typeof(AbstractDatalist)).Type;
            Type expected = typeof(AbstractDatalist);

            Assert.Equal(expected, actual);
        }

        #endregion
    }
}
