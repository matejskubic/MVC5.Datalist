using Datalist;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace DatalistTests.GenericDatalistTests
{
    [TestClass]
    public class FilterByAdditionalFiltersTests : GenericDatalistTests
    {
        [TestMethod]
        public void NullValuesTest()
        {
            Datalist.CurrentFilter.AdditionalFilters.Add("Id", null);
            Datalist.CurrentFilter.AdditionalFilters.Add("Number", null);
            var actual = Datalist.BaseFilterByAdditionalFilters(Datalist.Models).ToList();
            var expected = Datalist.Models;

            CollectionAssert.AreEquivalent(expected, actual);
        }

        [TestMethod]
        public void FilterTest()
        {
            Int32 filter = 9;
            Datalist.CurrentFilter.AdditionalFilters.Add("Id", filter);
            Datalist.CurrentFilter.AdditionalFilters.Add("Number", filter);
            var actual = Datalist.BaseFilterByAdditionalFilters(Datalist.Models).ToList();
            var expected = Datalist.Models.Where(model => model.Id == filter.ToString() && model.Number == filter).ToList();

            CollectionAssert.AreEquivalent(expected, actual);
        }

        [TestMethod]
        [ExpectedException(typeof(DatalistException))]
        public void UnfilterableTest()
        {
            Datalist.CurrentFilter.AdditionalFilters.Add("CreationDate", DateTime.Now);
            Datalist.BaseFilterByAdditionalFilters(Datalist.Models);
        }
    }
}
