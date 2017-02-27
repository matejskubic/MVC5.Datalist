using Xunit;

namespace Datalist.Tests.Unit
{
    public class DatalistExceptionTests
    {
        #region DatalistException(String message)

        [Fact]
        public void DatalistException_Message()
        {
            Assert.Equal("Test", new DatalistException("Test").Message);
        }

        #endregion
    }
}
