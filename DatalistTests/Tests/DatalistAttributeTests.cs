using Datalist;
using NUnit.Framework;
using System;

namespace DatalistTests.Tests
{
    [TestFixture]
    public class DatalistAttributeTests
    {
        #region Constructor: DatalistAttribute(Type type)

        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Null()
        {
            new DatalistAttribute(null);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void Type()
        {
            new DatalistAttribute(typeof(Object));
        }

        [Test]
        public void Getter()
        {
            var expected = typeof(AbstractDatalist);
            Assert.AreEqual(expected, new DatalistAttribute(expected).Type);
        }

        #endregion
    }
}
