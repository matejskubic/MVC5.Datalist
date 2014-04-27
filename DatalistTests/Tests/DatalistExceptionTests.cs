using Datalist;
using NUnit.Framework;
using System;

namespace DatalistTests.Tests
{
    [TestFixture]
    public class DatalistExceptionTests
    {
        #region Constructor: DatalistException(String message)

        [Test]
        public void DatalistException_SetsMessage()
        {
            String expected = "Test message";
            DatalistException exception = new DatalistException(expected);

            Assert.AreEqual(expected, exception.Message);
        }

        #endregion
    }
}
