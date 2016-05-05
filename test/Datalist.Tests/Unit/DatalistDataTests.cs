using System;
using Xunit;

namespace Datalist.Tests.Unit
{
    public class DatalistDataTests
    {
        #region DatalistData()

        [Fact]
        public void DatalistData_SetsFilteredRecords()
        {
            Int32 actual = new DatalistData().FilteredRecords;
            Int32 expected = 0;

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void DatalistData_SetsColumns()
        {
            Assert.Empty(new DatalistData().Columns);
        }

        [Fact]
        public void DatalistData_SetsRows()
        {
            Assert.Empty(new DatalistData().Rows);
        }

        #endregion
    }
}
