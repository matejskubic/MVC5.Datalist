using Datalist;
using NUnit.Framework;
using System;

namespace DatalistTests.Tests
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
            Assert.AreEqual("TestKey", new DatalistColumn("TestKey", String.Empty).Key);
        }

        [Test]
        public void DatalistColumn_SetsHeader()
        {
            Assert.AreEqual("TestHeader", new DatalistColumn(String.Empty, "TestHeader").Header);
        }

        [Test]
        public void DatalistColumn_SetsCssClass()
        {
            Assert.AreEqual("TestCss", new DatalistColumn(String.Empty, String.Empty, "TestCss").CssClass);
        }

        #endregion
    }
}
