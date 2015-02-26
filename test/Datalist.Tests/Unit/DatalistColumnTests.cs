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
            Assert.Throws<ArgumentNullException>(() => new DatalistColumn(null, String.Empty));
        }

        [Fact]
        public void Add_OnNullHeaderThrows()
        {
            Assert.Throws<ArgumentNullException>(() => new DatalistColumn(String.Empty, null));
        }

        [Fact]
        public void Add_OnNullCssClass()
        {
            Assert.Throws<ArgumentNullException>(() => new DatalistColumn(String.Empty, String.Empty, null));
        }

        [Fact]
        public void DatalistColumn_SetsKey()
        {
            String actual = new DatalistColumn("TestKey", String.Empty).Key;
            String expected = "TestKey";

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void DatalistColumn_SetsHeader()
        {
            String actual = new DatalistColumn(String.Empty, "TestHeader").Header;
            String expected = "TestHeader";

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void DatalistColumn_SetsCssClass()
        {
            String actual = new DatalistColumn(String.Empty, String.Empty, "TestCss").CssClass;
            String expected = "TestCss";

            Assert.Equal(expected, actual);
        }

        #endregion
    }
}
