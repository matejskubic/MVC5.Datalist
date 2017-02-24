using System;
using System.Collections.Generic;

namespace Datalist
{
    public class DatalistFilter
    {
        public String Id { get; set; }

        public Int32 Page { get; set; }
        public Int32 Rows { get; set; }
        public String Search { get; set; }

        public String Sort { get; set; }
        public DatalistSortOrder Order { get; set; }

        public IDictionary<String, Object> AdditionalFilters { get; set; }

        public DatalistFilter()
        {
            AdditionalFilters = new Dictionary<String, Object>();
        }
    }
}
