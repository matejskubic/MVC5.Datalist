using System;

namespace Datalist
{
    public class DatalistColumn
    {
        public String Key { get; }
        public String Header { get; set; }
        public String CssClass { get; set; }

        public DatalistColumn(String key, String header)
        {
            if (key == null)
                throw new ArgumentNullException(nameof(key));

            Key = key;
            Header = header;
        }
    }
}
