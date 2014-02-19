using Datalist;
using NUnit.Framework;

namespace DatalistTests.Tests
{
    [TestFixture]
    public class DatalistExceptionTests
    {
        #region Constructor: DatalistException(String message)

        [Test]
        public void DatalistException_SetsMessage()
        {
            var expected = "Test message";
            var exception = new DatalistException(expected);

            Assert.AreEqual(expected, exception.Message);
        }

        #endregion
    }
}
