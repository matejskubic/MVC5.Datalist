using Xunit;

namespace Datalist.Tests.Unit
{
    public class DatalistDataTests
    {
        #region DatalistData()

        [Fact]
        public void DatalistData_CreatesEmpty()
        {
            DatalistData actual = new DatalistData();

            Assert.Equal(0, actual.FilteredRows);
            Assert.Empty(actual.Columns);
            Assert.Empty(actual.Rows);
        }

        #endregion
    }
}
