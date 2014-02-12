using Datalist;
using System;
using System.Web;

namespace MvcDatalist.Datalists
{
    public class ExampleDatalist : DefaultDatalist
    {
        public ExampleDatalist()
        {
            Columns.Clear();
            Columns.Add("FirstName", "First name");
            Columns.Add("LastName", "Last name");
            AdditionalFilters.Add("AdditionalFilterId");

            DefaultSortColumn = "LastName";
            DefaultSortOrder = DatalistSortOrder.Desc;
            DefaultRecordsPerPage = 5;

            DatalistUrl = String.Format("{0}://{1}{2}{3}/{4}",
                HttpContext.Current.Request.Url.Scheme,
                HttpContext.Current.Request.Url.Authority,
                HttpContext.Current.Request.ApplicationPath,
                AbstractDatalist.Prefix,
                "DifferentUrlExample");

            DialogTitle = "Normal dialog title";
        }
    }
}