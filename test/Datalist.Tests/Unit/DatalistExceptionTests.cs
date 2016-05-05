using System;
using Xunit;

namespace Datalist.Tests.Unit
{
    public class DatalistExceptionTests
    {
        #region DatalistException(String message)

        [Fact]
        public void DatalistException_SetsMessage()
        {
            String actual = new DatalistException("Test").Message;
            String expected = "Test";

            Assert.Equal(expected, actual);
        }

        #endregion
    }
}
