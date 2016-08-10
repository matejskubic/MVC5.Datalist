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

        public String Url { get; protected set; }
        public String DialogTitle { get; protected set; }

        public DatalistFilter CurrentFilter { get; set; }
        public DatalistColumns Columns { get; protected set; }
        public IList<String> AdditionalFilters { get; protected set; }

        public String DefaultSortColumn { get; protected set; }
        public UInt32 DefaultRecordsPerPage { get; protected set; }
        public DatalistSortOrder DefaultSortOrder { get; protected set; }

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
