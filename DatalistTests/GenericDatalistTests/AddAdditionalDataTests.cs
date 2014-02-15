using DatalistTests.TestContext.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace DatalistTests.GenericDatalistTests
{
    [TestClass]
    public class AddAdditionalDataTests : BaseTests
    {
        [TestMethod]
        public void KeyCountTest()
        {
            var row = new Dictionary<String, String>();
            Datalist.BaseAddAdditionalData(row, new TestModel(1));

            Assert.AreEqual(0, row.Keys.Count);
        }
    }
}
