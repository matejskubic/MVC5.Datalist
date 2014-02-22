using Datalist;
using NUnit.Framework;
using System;

namespace DatalistTests.Tests
{
    [TestFixture]
    public class DatalistColumnTests
    {
        #region Constructor: DatalistColumn()
        
        [Test]
        public void DatalistColumn_NullKey()
        {
            Assert.IsNull(new DatalistColumn().Key);
        }

        [Test]
        public void DatalistColumn_NullHeader()
        {
            Assert.IsNull(new DatalistColumn().Header);
        }

        [Test]
        public void DatalistColumn_NullCssClass()
        {
            Assert.IsNull(new DatalistColumn().CssClass);
        }

        #endregion

        #region Constructor: DatalistColumn(String key, String header, String cssClass = null)

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
