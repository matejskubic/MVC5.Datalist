using Datalist;
using DatalistTests.GenericDatalistTests.Stubs;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DatalistTests.GenericDatalistTests
{
    [TestClass]
    public class FilterBySearchTermTests : BaseTests
    {
        [TestMethod]
        public void NullTermTest()
        {
            Datalist.CurrentFilter.SearchTerm = null;
            var expected = Datalist.BaseGetModels().ToList();
            var actual = Datalist.BaseFilterBySearchTerm(Datalist.BaseGetModels()).ToList();

            CollectionAssert.AreEquivalent(expected, actual);
        }

        [TestMethod]
        public void WhiteSpaceTermTest()
        {
            Datalist.CurrentFilter.SearchTerm = " ";
            var actual = Datalist.BaseFilterBySearchTerm(Datalist.BaseGetModels()).ToList();
            var expected = Datalist.BaseGetModels().Where(model =>
                (model.Id != null && model.Id.ToLower().Contains(Datalist.CurrentFilter.SearchTerm)) ||
                (model.FirstRelationModel != null && model.FirstRelationModel.Value != null && model.FirstRelationModel.Value.ToLower().Contains(Datalist.CurrentFilter.SearchTerm)) ||
                (model.SecondRelationModel != null && model.SecondRelationModel.Value != null && model.SecondRelationModel.Value.ToLower().Contains(Datalist.CurrentFilter.SearchTerm))).ToList();

            CollectionAssert.AreEquivalent(expected, actual);
        }

        [TestMethod]
        public void ContainsSearchTest()
        {
            var term = "1";
            Datalist.CurrentFilter.SearchTerm = term;
            var actual = Datalist.BaseFilterBySearchTerm(Datalist.BaseGetModels()).ToList();
            var expected = Datalist.BaseGetModels().Where(model =>
                (model.Id != null && model.Id.ToLower().Contains(term)) ||
                (model.FirstRelationModel != null && model.FirstRelationModel.Value != null && model.FirstRelationModel.Value.ToLower().Contains(term)) ||
                (model.SecondRelationModel != null && model.SecondRelationModel.Value != null && model.SecondRelationModel.Value.ToLower().Contains(term)));

            CollectionAssert.AreEquivalent(expected.ToList(), actual);
        }

        [TestMethod]
        public void NoStringPropertiesTest()
        {
            var datalist = new GenericDatalistStub<NoStringValuesModel>();
            datalist.CurrentFilter.SearchTerm = "Test";

            var expected = new List<NoStringValuesModel>() { new NoStringValuesModel() };
            var actual = datalist.BaseFilterBySearchTerm(expected.AsQueryable()).ToList();

            CollectionAssert.AreEquivalent(expected, actual);
        }

        [TestMethod]
        [ExpectedException(typeof(DatalistException))]
        public void NoPropertyTest()
        {
            Datalist.CurrentFilter.SearchTerm = "Test";
            Datalist.Columns.Add("TestProperty", String.Empty);
            Datalist.BaseFilterBySearchTerm(Datalist.BaseGetModels());
        }
    }

    public class NoStringValuesModel
    {
        [DatalistColumn]
        public Decimal Value { get; set; }
    }
}
