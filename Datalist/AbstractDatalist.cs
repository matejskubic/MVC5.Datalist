using System;
using System.Collections.Generic;

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
        public IDictionary<String, String> Columns
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
            DefaultRecordsPerPage = 20;
            CurrentFilter = new DatalistFilter();
            AdditionalFilters = new List<String>();
            Columns = new Dictionary<String, String>();
        }

        public abstract DatalistData GetData();
    }
}
