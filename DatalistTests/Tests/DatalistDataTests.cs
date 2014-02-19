using Datalist;
using NUnit.Framework;

namespace DatalistTests.Tests
{
    [TestFixture]
    public class DatalistDataTests
    {
        private DatalistData data;

        [SetUp]
        public void SetUp()
        {
            data = new DatalistData();
        }

        #region Constructor: DatalistData()

        [Test]
        public void DatalistData_ZeroFilteredRecords()
        {
            Assert.AreEqual(0, data.FilteredRecords);
        }

        [Test]
        public void DatalistData_EmptyColumns()
        {
            CollectionAssert.IsEmpty(data.Columns);
        }

        [Test]
        public void DatalistData_EmptyRows()
        {
            CollectionAssert.IsEmpty(data.Rows);
        }

        #endregion
    }
}
