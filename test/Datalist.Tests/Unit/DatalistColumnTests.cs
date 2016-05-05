using System;
using Xunit;

namespace Datalist.Tests.Unit
{
    public class DatalistColumnTests
    {
        #region DatalistColumn(String key, String header, String cssClass = "")

        [Fact]
        public void Add_NullKey_Throws()
        {
            ArgumentNullException actual = Assert.Throws<ArgumentNullException>(() => new DatalistColumn(null, ""));

            Assert.Equal("key", actual.ParamName);
        }

        [Fact]
        public void Add_NullHeader_Throws()
        {
            ArgumentNullException actual = Assert.Throws<ArgumentNullException>(() => new DatalistColumn("", null));

            Assert.Equal("header", actual.ParamName);
        }

        [Fact]
        public void Add_NullCssClass_Throws()
        {
            ArgumentNullException actual = Assert.Throws<ArgumentNullException>(() => new DatalistColumn("", "", null));

            Assert.Equal("cssClass", actual.ParamName);
        }

        [Fact]
        public void DatalistColumn_SetsKey()
        {
            String actual = new DatalistColumn("Test", "").Key;
            String expected = "Test";

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void DatalistColumn_SetsHeader()
        {
            String actual = new DatalistColumn("", "Test").Header;
            String expected = "Test";

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void DatalistColumn_SetsCssClass()
        {
            String actual = new DatalistColumn("", "", "Test").CssClass;
            String expected = "Test";

            Assert.Equal(expected, actual);
        }

        #endregion
    }
}
