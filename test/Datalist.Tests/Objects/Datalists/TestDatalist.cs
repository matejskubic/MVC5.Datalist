using System.Collections.Generic;
using System.Linq;

namespace Datalist.Tests.Objects
{
    public class TestDatalist<T> : GenericDatalist<T> where T : class
    {
        public IList<T> Models { get; set; }

        public TestDatalist()
        {
            Models = new List<T>();
            DefaultRecordsPerPage = 7;
            DialogTitle = "Test title";
            DefaultSortColumn = "SortCol";
            Url = "http://localhost/Test";
            AdditionalFilters.Add("Test1");
            AdditionalFilters.Add("Test2");
            CurrentFilter.RecordsPerPage = 10;
            CurrentFilter.SearchTerm = "Data up";
            DefaultSortOrder = DatalistSortOrder.Asc;
        }

        public override IQueryable<T> GetModels()
        {
            return Models.AsQueryable();
        }
    }
}
