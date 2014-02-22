using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Datalist
{
    public sealed class DatalistColumns : IEnumerable<DatalistColumn>
    {
        private List<DatalistColumn> columns;

        public IEnumerable<String> Keys
        {
            get
            {
                return columns.Select(column => column.Key);
            }
        }

        public DatalistColumns()
        {
            columns = new List<DatalistColumn>();
        }

        public void Add(DatalistColumn column)
        {
            if (column == null)
                throw new ArgumentNullException("column");
            if (columns.Any(col => col.Key == column.Key))
                throw new DatalistException(String.Format(@"Can not add datalist column with the same key ""{0}""", column.Key));

            columns.Add(column);
        }
        public void Add(String key, String header, String cssClass = "")
        {
            Add(new DatalistColumn(key, header, cssClass));
        }
        public Boolean Remove(DatalistColumn column)
        {
            return columns.Remove(column);
        }
        public Boolean Remove(String key)
        {
            return columns.Remove(columns.FirstOrDefault(column => column.Key == key));
        }
        public void Clear()
        {
            foreach (var column in this)
                Remove(column);
        }

        public IEnumerator<DatalistColumn> GetEnumerator()
        {
            return columns.ToList().GetEnumerator();
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
