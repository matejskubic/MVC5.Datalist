using System.Collections.Generic;
using System.Linq;

namespace Datalist.Tests.Objects
{
    public class TestDatalist<T> : MvcDatalist<T> where T : class
    {
        public IList<T> Models { get; set; }

        public TestDatalist()
        {
            Filter.Page = 3;
            Filter.Rows = 7;
            Filter.Sort = "Id";
            Dialog = "TestDialog";
            Models = new List<T>();
            Filter.Search = "Term";
            Url = "http://localhost/Test";
            Title = "Test datalist title";
            AdditionalFilters.Add("Test1");
            AdditionalFilters.Add("Test2");
        }

        public override IQueryable<T> GetModels()
        {
            return Models.AsQueryable();
        }
    }
}
