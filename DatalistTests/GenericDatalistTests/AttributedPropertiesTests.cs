using Datalist;
using DatalistTests.GenericDatalistTests.Stubs;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Reflection;

namespace DatalistTests.GenericDatalistTests
{
    [TestClass]
    public class AttributedPropertiesTests : GenericDatalistTests
    {
        #region Tests

        [TestMethod]
        public void AttributeTest()
        {
            var actual = Datalist.BaseAttributedProperties.ToList();
            var expected = typeof(DatalistModel).GetProperties()
                .Where(property => property.GetCustomAttribute<DatalistColumnAttribute>(false) != null)
                .ToList();

            CollectionAssert.AreEquivalent(expected, actual);
        }

        [TestMethod]
        public void OrderTest()
        {
            var actual = Datalist.BaseAttributedProperties.ToList();
            var expected = typeof(DatalistModel).GetProperties()
                .Where(property => property.GetCustomAttribute<DatalistColumnAttribute>(false) != null)
                .OrderBy(property => property.GetCustomAttribute<DatalistColumnAttribute>(false).Position)
                .ToList();

            CollectionAssert.AreEqual(expected, actual);
        }

        #endregion
    }
}
