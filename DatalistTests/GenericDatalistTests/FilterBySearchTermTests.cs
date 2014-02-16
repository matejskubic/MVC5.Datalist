using Datalist;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
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
        [ExpectedException(typeof(DatalistException))]
        public void NoPropertyTest()
        {
            Datalist.CurrentFilter.SearchTerm = "Test";
            Datalist.Columns.Add("TestProperty", String.Empty);
            Datalist.BaseFilterBySearchTerm(Datalist.BaseGetModels());
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
            Datalist.CurrentFilter.SearchTerm = "1";
            var actual = Datalist.BaseFilterBySearchTerm(Datalist.BaseGetModels()).ToList();
            var expected = Datalist.BaseGetModels().Where(model =>
                (model.Id != null && model.Id.ToLower().Contains(Datalist.CurrentFilter.SearchTerm)) ||
                (model.FirstRelationModel != null && model.FirstRelationModel.Value != null && model.FirstRelationModel.Value.ToLower().Contains(Datalist.CurrentFilter.SearchTerm)) ||
                (model.SecondRelationModel != null && model.SecondRelationModel.Value != null && model.SecondRelationModel.Value.ToLower().Contains(Datalist.CurrentFilter.SearchTerm)));

            CollectionAssert.AreEquivalent(expected.ToList(), actual);
        }

        [TestMethod]
        public void NoStringPropertiesTest()
        {
            Datalist.Columns.Clear();
            Datalist.Columns.Add("Number", String.Empty);
            Datalist.CurrentFilter.SearchTerm = "Test";

            var expected = Datalist.BaseGetModels().ToList();
            var actual = Datalist.BaseFilterBySearchTerm(expected.AsQueryable()).ToList();

            CollectionAssert.AreEquivalent(expected, actual);
        }
    }
}
