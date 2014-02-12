using System;
using System.Collections.Generic;

namespace Datalist
{
    public class DatalistData
    {
        public Int64 FilteredRecords
        {
            get;
            set;
        }
        public IDictionary<String, String> Columns
        {
            get;
            set;
        }
        public List<IDictionary<String, String>> Rows
        {
            get;
            set;
        }

        public DatalistData()
        {
            FilteredRecords = 0;
            Columns = new Dictionary<String, String>();
            Rows = new List<IDictionary<String, String>>();
        }
    }
}