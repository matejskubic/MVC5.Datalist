using System;
using System.Collections.Generic;

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
        public IList<DatalistColumn> Columns { get; set; }
        public IList<String> AdditionalFilters { get; set; }

        protected AbstractDatalist()
        {
            AdditionalFilters = new List<String>();
            Columns = new List<DatalistColumn>();
            Filter = new DatalistFilter();
            Filter.Rows = 20;
        }

        public abstract DatalistData GetData();
    }
}
