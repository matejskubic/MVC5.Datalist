using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Moq.Protected;
using System.Linq;

namespace DatalistTests.GenericDatalistTests
{
    [TestClass]
    public class GetDataTests : GenericDatalistTests
    {
        [TestMethod]
        public void GetModelsCalledTest()
        {
            Datalist.GetData();
            DatalistMock.Protected().Verify("GetModels", Times.Once());
        }
        
        [TestMethod]
        public void FilterByIdCalledTest()
        {
            Datalist.CurrentFilter.Id = "1";
            Datalist.GetData();
            DatalistMock.Protected().Verify("FilterById", Times.Once(), Datalist.Models.AsQueryable());
        }

        [TestMethod]
        public void FilterByIdNotCalledTest()
        {
            Datalist.CurrentFilter.Id = null;
            Datalist.GetData();
            DatalistMock.Protected().Verify("FilterById", Times.Never(), Datalist.Models.AsQueryable());
        }

        [TestMethod]
        public void FilterByAdditionalFiltersCalledTest()
        {
            Datalist.CurrentFilter.AdditionalFilters.Add("Id", "1");
            Datalist.GetData();
            DatalistMock.Protected().Verify("FilterByAdditionalFilters", Times.Once(), Datalist.Models.AsQueryable());
        }

        [TestMethod]
        public void FilterByAdditionalFiltersNotCalledBecauseEmptyTest()
        {
            Datalist.CurrentFilter.AdditionalFilters.Clear();
            Datalist.GetData();
            DatalistMock.Protected().Verify("FilterByAdditionalFilters", Times.Never(), Datalist.Models.AsQueryable());
        }

        [TestMethod]
        public void FilterByAdditionalFiltersNotCalledBecauseIdFilteredTest()
        {
            Datalist.CurrentFilter.Id = "1";
            Datalist.GetData();
            DatalistMock.Protected().Verify("FilterByAdditionalFilters", Times.Never(), Datalist.Models.AsQueryable());
        }

        [TestMethod]
        public void FilterBySearchTermCalledTest()
        {
            Datalist.CurrentFilter.Id = null;

            Datalist.GetData();
            DatalistMock.Protected().Verify("FilterBySearchTerm", Times.Once(), Datalist.Models.AsQueryable());
        }

        [TestMethod]
        public void FilterBySearchTermFilteredCalledTest()
        {
            Datalist.CurrentFilter.AdditionalFilters.Add("Id", "1");
            Datalist.CurrentFilter.Id = null;

            Datalist.GetData();
            DatalistMock.Protected().Verify("FilterBySearchTerm", Times.Once(), Datalist.Models.AsQueryable().Where(model => model.Id == "1"));
        }

        [TestMethod]
        public void FilterBySearchTermNotCalledTest()
        {
            Datalist.CurrentFilter.Id = "1";
            Datalist.GetData();
            DatalistMock.Protected().Verify("FilterBySearchTerm", Times.Never(), Datalist.Models.AsQueryable());
        }

        [TestMethod]
        public void FormDatalistDataCalledTest()
        {
            Datalist.GetData();
            DatalistMock.Protected().Verify("FormDatalistData", Times.Never(), Datalist.Models.AsQueryable());
        }
    }
}
