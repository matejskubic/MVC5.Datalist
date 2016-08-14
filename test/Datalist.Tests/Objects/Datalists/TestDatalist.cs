using System.Collections.Generic;
using System.Linq;

namespace Datalist.Tests.Objects
{
    public class TestDatalist<T> : GenericDatalist<T> where T : class
    {
        public IList<T> Models { get; set; }

        public TestDatalist()
        {
            Filter.Page = 3;
            Filter.Rows = 7;
            Title = "Test title";
            Models = new List<T>();
            Filter.Search = "Term";
            Filter.SortColumn = "Id";
            Url = "http://localhost/Test";
            AdditionalFilters.Add("Test1");
            AdditionalFilters.Add("Test2");
        }

        public override IQueryable<T> GetModels()
        {
            return Models.AsQueryable();
        }
    }
}
