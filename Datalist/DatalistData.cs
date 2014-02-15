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
        public Dictionary<String, String> Columns
        {
            get;
            set;
        }
        public List<Dictionary<String, String>> Rows
        {
            get;
            set;
        }

        public DatalistData()
        {
            FilteredRecords = 0;
            Columns = new Dictionary<String, String>();
            Rows = new List<Dictionary<String, String>>();
        }
    }
}