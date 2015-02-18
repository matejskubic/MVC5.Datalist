using NUnit.Framework;
using System;

namespace Datalist.Tests.Unit
{
    [TestFixture]
    public class DatalistAttributeTests
    {
        #region Constructor: DatalistAttribute(Type type)

        [Test]
        public void DatalistAttribute_NullThrows()
        {
            Assert.Throws<ArgumentNullException>(() => new DatalistAttribute(null));
        }

        [Test]
        public void DatalistAttribute_UnassignableTypeThrows()
        {
            Assert.Throws<ArgumentException>(() => new DatalistAttribute(typeof(Object)));
        }

        [Test]
        public void DatalistAttribute_SetsType()
        {
            Type actual = new DatalistAttribute(typeof(AbstractDatalist)).Type;
            Type expected = typeof(AbstractDatalist);

            Assert.AreEqual(expected, actual);
        }

        #endregion
    }
}
