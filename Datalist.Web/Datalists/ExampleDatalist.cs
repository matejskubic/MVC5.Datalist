using Datalist;
using System;
using System.Reflection;
using System.Web;

namespace Datalist.Web.Datalists
{
    public class ExampleDatalist : DefaultDatalist
    {
        public ExampleDatalist()
        {
            Columns.Clear();
            Columns.Add("FirstName", "First name", "text-cell");
            Columns.Add("LastName", "Last name", "text-cell");
            AdditionalFilters.Add("AdditionalFilterId");

            DefaultSortColumn = "LastName";
            DefaultSortOrder = DatalistSortOrder.Desc;
            DefaultRecordsPerPage = 5;

            DatalistUrl = String.Format("{0}://{1}{2}{3}/{4}",
                HttpContext.Current.Request.Url.Scheme,
                HttpContext.Current.Request.Url.Authority,
                HttpContext.Current.Request.ApplicationPath ?? "/",
                AbstractDatalist.Prefix,
                "DifferentUrlExample");

            DialogTitle = "Normal dialog title";
        }
        protected override String GetColumnCssClass(PropertyInfo property)
        {
            if (property.PropertyType == typeof(String))
                return "text-cell";

            return String.Empty;
        }
    }
}