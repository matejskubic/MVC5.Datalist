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
        public void FilteredRecords()
        {
            Assert.AreEqual(0, data.FilteredRecords);
        }

        [Test]
        public void Columns()
        {
            Assert.AreEqual(0, data.Columns.Count);
        }

        [Test]
        public void Rows()
        {
            Assert.AreEqual(0, data.Rows.Count);
        }

        #endregion
    }
}
