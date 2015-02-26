using System;
using Xunit;

namespace Datalist.Tests.Unit
{
    public class DatalistDataTests
    {
        #region Constructor: DatalistData()

        [Fact]
        public void DatalistData_ZeroFilteredRecords()
        {
            Int32 actual = new DatalistData().FilteredRecords;
            Int32 expected = 0;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void DatalistData_EmptyColumns()
        {
            Assert.Empty(new DatalistData().Columns);
        }

        [Fact]
        public void DatalistData_EmptyRows()
        {
            Assert.Empty(new DatalistData().Rows);
        }

        #endregion
    }
}
