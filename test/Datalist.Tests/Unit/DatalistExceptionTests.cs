using Datalist;
using NUnit.Framework;
using System;

namespace Datalist.Tests.Unit
{
    [TestFixture]
    public class DatalistExceptionTests
    {
        #region Constructor: DatalistException(String message)

        [Test]
        public void DatalistException_SetsMessage()
        {
            String actual = new DatalistException("T").Message;
            String expected = "T";

            Assert.AreEqual(expected, actual);
        }

        #endregion
    }
}
