using Datalist;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace DatalistTests.GenericDatalistTests
{
    [TestClass]
    public class SortTests : BaseTests
    {
        [TestMethod]
        public void SortColumnTest()
        {
            Datalist.CurrentFilter.SortColumn = Datalist.BaseAttributedProperties.First().Name;
            var expected = Datalist.BaseGetModels().OrderBy(model => model.Number).ToList();
            var actual = Datalist.BaseSort(Datalist.BaseGetModels()).ToList();

            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void DefaultSortColumnTest()
        {
            Datalist.CurrentFilter.SortColumn = null;
            Datalist.BaseDefaultSortColumn = Datalist.BaseAttributedProperties.First().Name;
            var expected = Datalist.BaseGetModels().OrderBy(model => model.Number).ToList();
            var actual = Datalist.BaseSort(Datalist.BaseGetModels()).ToList();

            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        [ExpectedException(typeof(DatalistException))]
        public void NoSortColumnTest()
        {
            Datalist.CurrentFilter.SortColumn = "TestProperty";
            Datalist.BaseSort(Datalist.BaseGetModels());
        }

        [TestMethod]
        [ExpectedException(typeof(DatalistException))]
        public void NoDefaultSortColumnTest()
        {
            Datalist.BaseDefaultSortColumn = "TestProperty";
            Datalist.CurrentFilter.SortColumn = null;
            Datalist.BaseSort(Datalist.BaseGetModels());
        }

        [TestMethod]
        public void FirstColumnSortTest()
        {
            Datalist.BaseDefaultSortColumn = null;
            Datalist.CurrentFilter.SortColumn = null;
            var actual = Datalist.BaseSort(Datalist.BaseGetModels()).ToList();
            var expected = Datalist.BaseGetModels().OrderBy(model => model.Number).ToList();

            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        [ExpectedException(typeof(DatalistException))]
        public void NoColumnsTest()
        {
            Datalist.Columns.Clear();
            Datalist.BaseSort(Datalist.BaseGetModels());
        }
    }
}
