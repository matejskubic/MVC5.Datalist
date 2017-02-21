using System;
using Xunit;
using Xunit.Extensions;

namespace Datalist.Tests.Unit
{
    public class DatalistAttributeTests
    {
        #region DatalistAttribute(Type type)

        [Theory]
        [InlineData(null)]
        [InlineData(typeof(Object))]
        public void DatalistAttribute_NoDatalist_Throws(Type type)
        {
            ArgumentException exception = Assert.Throws<ArgumentException>(() => new DatalistAttribute(type));

            String expected = $"'{type?.Name}' type does not implement '{typeof(MvcDatalist).Name}'.";
            String actual = exception.Message;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void DatalistAttribute_Type()
        {
            Type actual = new DatalistAttribute(typeof(MvcDatalist)).Type;
            Type expected = typeof(MvcDatalist);

            Assert.Equal(expected, actual);
        }

        #endregion
    }
}
