using NUnit.Framework;
using System;

namespace Datalist.Tests.Unit
{
    [TestFixture]
    public class DatalistDataTests
    {
        #region Constructor: DatalistData()

        [Test]
        public void DatalistData_ZeroFilteredRecords()
        {
            Int32 actual = new DatalistData().FilteredRecords;
            Int32 expected = 0;

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void DatalistData_EmptyColumns()
        {
            CollectionAssert.IsEmpty(new DatalistData().Columns);
        }

        [Test]
        public void DatalistData_EmptyRows()
        {
            CollectionAssert.IsEmpty(new DatalistData().Rows);
        }

        #endregion
    }
}
