using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;

namespace Datalist
{
    public abstract class AbstractDatalist
    {
        public const String Prefix = "Datalist";
        public const String IdKey = "DatalistIdKey";
        public const String AcKey = "DatalistAcKey";

        public String Url { get; set; }
        public String DialogTitle { get; set; }

        public DatalistColumns Columns { get; set; }
        public DatalistFilter CurrentFilter { get; set; }
        public IList<String> AdditionalFilters { get; set; }

        public String DefaultSortColumn { get; set; }
        public UInt32 DefaultRecordsPerPage { get; set; }
        public DatalistSortOrder DefaultSortOrder { get; set; }

        protected AbstractDatalist() : this(new UrlHelper(HttpContext.Current.Request.RequestContext))
        {
        }
        protected AbstractDatalist(UrlHelper url)
        {
            Url = url.Action(GetType().Name.Replace(Prefix, ""), Prefix, new { area = "" });
            AdditionalFilters = new List<String>();
            CurrentFilter = new DatalistFilter();
            Columns = new DatalistColumns();
            DefaultRecordsPerPage = 20;
        }

        public abstract DatalistData GetData();
    }
}
