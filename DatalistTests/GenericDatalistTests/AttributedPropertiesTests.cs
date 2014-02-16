using Datalist;
using DatalistTests.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using System.Reflection;

namespace DatalistTests.GenericDatalistTests
{
    [TestClass]
    public class AttributedPropertiesTests : BaseTests
    {
        [TestMethod]
        public void AttributedTest()
        {
            var actual = Datalist.BaseAttributedProperties.ToList();
            var expected = typeof(TestModel).GetProperties()
                .Where(property => property.GetCustomAttribute<DatalistColumnAttribute>(false) != null)
                .OrderBy(property => property.GetCustomAttribute<DatalistColumnAttribute>(false).Position)
                .ToList();

            CollectionAssert.AreEqual(expected, actual);
        }
    }
}
