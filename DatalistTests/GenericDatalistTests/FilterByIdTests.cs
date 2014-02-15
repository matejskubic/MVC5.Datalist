using Datalist;
using DatalistTests.GenericDatalistTests.Stubs;
using DatalistTests.TestContext.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DatalistTests.GenericDatalistTests
{
    [TestClass]
    public class FilterByIdTests : BaseTests
    {
        [TestMethod]
        [ExpectedException(typeof(DatalistException))]
        public void NoIdTest()
        {
            var datalist = new GenericDatalistStub<NoIdModel>();
            datalist.BaseFilterById(new List<NoIdModel>().AsQueryable());
        }

        [TestMethod]
        public void StringIdTest()
        {
            Datalist.CurrentFilter.Id = "9";
            var expected = Datalist.BaseGetModels().Where(model => model.Id == Datalist.CurrentFilter.Id).ToList();
            var actual = Datalist.BaseFilterById(Datalist.BaseGetModels()).ToList();

            CollectionAssert.AreEquivalent(expected, actual);
        }

        [TestMethod]
        public void NumericIdTest()
        {
            var models = new List<NumericIdModel>();
            var datalist = new GenericDatalistStub<NumericIdModel>();
            for (Int32 i = 0; i < 100; i++)
                models.Add(new NumericIdModel(i));

            var id = "9";
            datalist.CurrentFilter.Id = id;

            var expected = models.Where(model => model.Id == 9).ToList();
            var actual = datalist.BaseFilterById(models.AsQueryable()).ToList();

            CollectionAssert.AreEquivalent(expected, actual);
        }
        
        [TestMethod]
        [ExpectedException(typeof(DatalistException))]
        public void NonNumericIdTest()
        {
            var datalist = new GenericDatalistStub<NonNumericIdModel>();
            datalist.CurrentFilter.Id = "9";

            var actual = datalist.BaseFilterById(new List<NonNumericIdModel>().AsQueryable()).ToList();
        }
    }
}
