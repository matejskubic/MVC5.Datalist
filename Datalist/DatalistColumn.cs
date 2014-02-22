using System;

namespace Datalist
{
    public class DatalistColumn
    {
        public String Key
        {
            get;
            set;
        }
        public String Header
        {
            get;
            set;
        }
        public String CssClass
        {
            get;
            set;
        }

        public DatalistColumn()
        {
        }
        public DatalistColumn(String key, String header, String cssClass = "")
        {
            Key = key;
            Header = header;
            CssClass = cssClass;
        }
    }
}
