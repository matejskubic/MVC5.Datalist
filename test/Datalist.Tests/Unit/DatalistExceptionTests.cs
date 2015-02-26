using System;
using Xunit;

namespace Datalist.Tests.Unit
{
    public class DatalistExceptionTests
    {
        #region Constructor: DatalistException(String message)

        [Fact]
        public void DatalistException_SetsMessage()
        {
            String actual = new DatalistException("T").Message;
            String expected = "T";

            Assert.Equal(expected, actual);
        }

        #endregion
    }
}
