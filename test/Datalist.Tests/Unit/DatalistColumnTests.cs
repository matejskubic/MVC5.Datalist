using System;
using Xunit;

namespace Datalist.Tests.Unit
{
    public class DatalistColumnTests
    {
        #region Constructor: DatalistColumn(String key, String header, String cssClass = "")

        [Fact]
        public void Add_OnNullKeyThrows()
        {
            Assert.Throws<ArgumentNullException>(() => new DatalistColumn(null, ""));
        }

        [Fact]
        public void Add_OnNullHeaderThrows()
        {
            Assert.Throws<ArgumentNullException>(() => new DatalistColumn("", null));
        }

        [Fact]
        public void Add_OnNullCssClass()
        {
            Assert.Throws<ArgumentNullException>(() => new DatalistColumn("", "", null));
        }

        [Fact]
        public void DatalistColumn_SetsKey()
        {
            String actual = new DatalistColumn("TestKey", "").Key;
            String expected = "TestKey";

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void DatalistColumn_SetsHeader()
        {
            String actual = new DatalistColumn("", "TestHeader").Header;
            String expected = "TestHeader";

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void DatalistColumn_SetsCssClass()
        {
            String actual = new DatalistColumn("", "", "TestCss").CssClass;
            String expected = "TestCss";

            Assert.Equal(expected, actual);
        }

        #endregion
    }
}
