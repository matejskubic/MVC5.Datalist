using Datalist;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DatalistTests.DatalistDataTests
{
    [TestClass]
    public class ConstructorTests
    {
        private DatalistData data;

        [TestInitialize]
        public void TestInit()
        {
            data = new DatalistData();
        }

        
        [TestMethod]
        public void FilteredRecordsTest()
        {
            Assert.AreEqual(0, data.FilteredRecords);
        }

        [TestMethod]
        public void ColumnsTest()
        {
            Assert.AreEqual(0, data.Columns.Count);
        }

        [TestMethod]
        public void RowsTest()
        {
            Assert.AreEqual(0, data.Rows.Count);
        }
    }
}
