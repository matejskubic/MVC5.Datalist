using System.Collections.Generic;
using System.Linq;

namespace Datalist.Tests.Objects
{
    public class TestDatalist<T> : GenericDatalist<T> where T : class
    {
        public IList<T> Models { get; set; }

        public TestDatalist()
        {
            DefaultRows = 7;
            Filter.Rows = 10;
            Title = "Test title";
            Models = new List<T>();
            Filter.Search = "Data up";
            DefaultSortColumn = "SortCol";
            Url = "http://localhost/Test";
            AdditionalFilters.Add("Test1");
            AdditionalFilters.Add("Test2");
            DefaultSortOrder = DatalistSortOrder.Asc;
        }

        public override IQueryable<T> GetModels()
        {
            return Models.AsQueryable();
        }
    }
}
