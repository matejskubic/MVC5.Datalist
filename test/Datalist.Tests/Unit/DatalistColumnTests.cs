using NUnit.Framework;
using System;

namespace Datalist.Tests.Unit
{
    [TestFixture]
    public class DatalistColumnTests
    {
        #region Constructor: DatalistColumn(String key, String header, String cssClass = "")

        [Test]
        public void Add_OnNullKeyThrows()
        {
            Assert.Throws<ArgumentNullException>(() => new DatalistColumn(null, String.Empty));
        }

        [Test]
        public void Add_OnNullHeaderThrows()
        {
            Assert.Throws<ArgumentNullException>(() => new DatalistColumn(String.Empty, null));
        }

        [Test]
        public void Add_OnNullCssClass()
        {
            Assert.Throws<ArgumentNullException>(() => new DatalistColumn(String.Empty, String.Empty, null));
        }

        [Test]
        public void DatalistColumn_SetsKey()
        {
            String actual = new DatalistColumn("TestKey", String.Empty).Key;
            String expected = "TestKey";

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void DatalistColumn_SetsHeader()
        {
            String actual = new DatalistColumn(String.Empty, "TestHeader").Header;
            String expected = "TestHeader";

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void DatalistColumn_SetsCssClass()
        {
            String actual = new DatalistColumn(String.Empty, String.Empty, "TestCss").CssClass;
            String expected = "TestCss";

            Assert.AreEqual(expected, actual);
        }

        #endregion
    }
}
