using Datalist;
using DatalistTests.GenericDatalistTests.Stubs;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace DatalistTests.GenericDatalistTests
{
    [TestClass]
    public class FilterByIdTests : GenericDatalistTests
    {
        #region Tests

        [TestMethod]
        [ExpectedException(typeof(DatalistException))]
        public void NoIdTest()
        {
            var noIdDatalist = new GenericDatalistStub<NoIdModel>();
            noIdDatalist.BaseFilterById(noIdDatalist.Models);
        }

        [TestMethod]
        public void StringIdTest()
        {
            var id = "9";
            Datalist.CurrentFilter.Id = id;

            var expected = Datalist.Models.Where(model => model.Id == id).ToList();
            var actual = Datalist.BaseFilterById(Datalist.Models).ToList();

            CollectionAssert.AreEquivalent(expected, actual);
        }

        [TestMethod]
        public void NumericIdTest()
        {
            var numericIdDatalist = new GenericDatalistStub<NumericIdModel>();
            for (Int32 i = 0; i < 100; i++)
                numericIdDatalist.Models.Add(new NumericIdModel(i));

            var id = "9";
            numericIdDatalist.CurrentFilter.Id = id;

            var expected = numericIdDatalist.Models.Where(model => model.Id == 9).ToList();
            var actual = numericIdDatalist.BaseFilterById(numericIdDatalist.Models).ToList();

            CollectionAssert.AreEquivalent(expected, actual);
        }
        // TODO: Remove regions
        [TestMethod]
        [ExpectedException(typeof(DatalistException))]
        public void NonNumericIdTest()
        {
            var nonNumericIdDatalist = new GenericDatalistStub<NonNumericIdModel>();
            for (Int32 i = 0; i < 100; i++)
                nonNumericIdDatalist.Models.Add(new NonNumericIdModel(i));

            var id = "9";
            nonNumericIdDatalist.CurrentFilter.Id = id;

            var expected = nonNumericIdDatalist.Models.Where(model => model.Id == 9).ToList();
            var actual = nonNumericIdDatalist.BaseFilterById(nonNumericIdDatalist.Models).ToList();

            CollectionAssert.AreEquivalent(expected, actual);
        }

        #endregion
    }
}
