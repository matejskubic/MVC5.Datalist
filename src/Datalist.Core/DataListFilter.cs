using System;
using System.Collections.Generic;

namespace Datalist
{
    public class DatalistFilter
    {
        public String Id { get; set; }
        public Int32 Page { get; set; }
        public String SearchTerm { get; set; }
        public String SortColumn { get; set; }
        public Int32 RecordsPerPage { get; set; }
        public DatalistSortOrder SortOrder { get; set; }

        public Dictionary<String, Object> AdditionalFilters { get; set; }

        public DatalistFilter()
        {
            AdditionalFilters = new Dictionary<String, Object>();
        }
    }
}
