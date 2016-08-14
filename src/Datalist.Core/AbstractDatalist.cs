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
        public String Title { get; set; }

        public DatalistFilter Filter { get; set; }
        public DatalistColumns Columns { get; set; }
        public IList<String> AdditionalFilters { get; set; }

        public UInt32 DefaultRows { get; set; }
        public String DefaultSortColumn { get; set; }
        public DatalistSortOrder DefaultSortOrder { get; set; }

        protected AbstractDatalist() : this(new UrlHelper(HttpContext.Current.Request.RequestContext))
        {
        }
        protected AbstractDatalist(UrlHelper url)
        {
            Url = url.Action(GetType().Name.Replace(Prefix, ""), Prefix, new { area = "" });
            AdditionalFilters = new List<String>();
            Columns = new DatalistColumns();
            Filter = new DatalistFilter();
            DefaultRows = 20;
        }

        public abstract DatalistData GetData();
    }
}
