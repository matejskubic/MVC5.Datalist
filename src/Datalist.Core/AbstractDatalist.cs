using System;
using System.Collections.Generic;
using System.Web;

namespace Datalist
{
    public abstract class AbstractDatalist
    {
        public const String Prefix = "Datalist";
        public const String IdKey = "DatalistIdKey";
        public const String AcKey = "DatalistAcKey";

        public String DialogTitle
        {
            get;
            protected set;
        }
        public String DatalistUrl
        {
            get;
            protected set;
        }
        public String DefaultSortColumn
        {
            get;
            protected set;
        }
        public UInt32 DefaultRecordsPerPage
        {
            get;
            protected set;
        }
        public List<String> AdditionalFilters
        {
            get;
            protected set;
        }
        public DatalistSortOrder DefaultSortOrder
        {
            get;
            protected set;
        }
        public DatalistColumns Columns
        {
            get;
            protected set;
        }

        public DatalistFilter CurrentFilter
        {
            get;
            set;
        }

        protected AbstractDatalist()
        {
            String sanitizedName = GetType().Name.Replace(Prefix, String.Empty);
            AdditionalFilters = new List<String>();
            CurrentFilter = new DatalistFilter();
            Columns = new DatalistColumns();
            DialogTitle = sanitizedName;
            DefaultRecordsPerPage = 20;

            String applicationPath = HttpContext.Current.Request.ApplicationPath ?? "/";
            if (!applicationPath.EndsWith("/"))
                applicationPath += "/";

            DatalistUrl = String.Format("{0}://{1}{2}{3}/{4}",
                HttpContext.Current.Request.Url.Scheme,
                HttpContext.Current.Request.Url.Authority,
                applicationPath,
                Prefix,
                sanitizedName);
        }

        public abstract DatalistData GetData();
    }
}
