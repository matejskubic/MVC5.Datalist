using System;
using Xunit;

namespace Datalist.Tests.Unit
{
    public class DatalistColumnTests
    {
        #region DatalistColumn(String key, String header, String cssClass = null)

        [Fact]
        public void Add_NullKey_Throws()
        {
            ArgumentNullException actual = Assert.Throws<ArgumentNullException>(() => new DatalistColumn(null, ""));

            Assert.Equal("key", actual.ParamName);
        }

        [Fact]
        public void DatalistColumn_Key()
        {
            String actual = new DatalistColumn("Test", "").Key;
            String expected = "Test";

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void DatalistColumn_Header()
        {
            String actual = new DatalistColumn("", "Test").Header;
            String expected = "Test";

            Assert.Equal(expected, actual);
        }

        #endregion
    }
}
