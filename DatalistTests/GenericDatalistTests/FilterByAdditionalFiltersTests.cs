using Datalist;
using DatalistTests.GenericDatalistTests.Stubs;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DatalistTests.GenericDatalistTests
{
    [TestClass]
    public class FilterByAdditionalFiltersTests : BaseTests
    {
        [TestMethod]
        public void NullValuesTest()
        {
            var expected = Datalist.BaseGetModels().ToList();
            Datalist.CurrentFilter.AdditionalFilters.Add("Id", null);
            Datalist.CurrentFilter.AdditionalFilters.Add("Number", null);
            var actual = Datalist.BaseFilterByAdditionalFilters(Datalist.BaseGetModels()).ToList();
            
            CollectionAssert.AreEquivalent(expected, actual);
        }

        [TestMethod]
        public void FilterTest()
        {
            var numberFilter = 9;
            var stringFilter = "9";
            Datalist.CurrentFilter.AdditionalFilters.Add("Id", stringFilter);
            Datalist.CurrentFilter.AdditionalFilters.Add("Number", numberFilter);
            var actual = Datalist.BaseFilterByAdditionalFilters(Datalist.BaseGetModels()).ToList();
            var expected = Datalist.BaseGetModels().Where(model => model.Id == stringFilter && model.Number == numberFilter).ToList();

            CollectionAssert.AreEquivalent(expected, actual);
        }

        [TestMethod]
        [ExpectedException(typeof(DatalistException))]
        public void NotSupportedTest()
        {
            Datalist.CurrentFilter.AdditionalFilters.Add("CreationDate", DateTime.Now);
            Datalist.BaseFilterByAdditionalFilters(Datalist.BaseGetModels());
        }

        [TestMethod]
        [ExpectedException(typeof(DatalistException))]
        public void NotSupportedEnumTest()
        {
            var datalist = new GenericDatalistStub<EnumModel>();
            datalist.CurrentFilter.AdditionalFilters.Add("IdEnum", DateTime.Now);

            datalist.BaseFilterByAdditionalFilters(new List<EnumModel>().AsQueryable()).ToList();
        }

        [TestMethod]
        [ExpectedException(typeof(DatalistException))]
        public void NoPropertyTest()
        {
            Datalist.CurrentFilter.AdditionalFilters.Add("TestProperty", "Test");
            Datalist.BaseFilterByAdditionalFilters(Datalist.BaseGetModels());
        }
    }
}
