using System;
using System.Collections.Generic;

namespace Datalist
{
    public class DatalistData
    {
        public Int32 FilteredRows { get; set; }
        public IList<DatalistColumn> Columns { get; set; }
        public List<Dictionary<String, String>> Rows { get; set; }

        public DatalistData()
        {
            Columns = new List<DatalistColumn>();
            Rows = new List<Dictionary<String, String>>();
        }
    }
}
