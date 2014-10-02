using System;

namespace Datalist
{
    public class DatalistColumn
    {
        public String Key
        {
            get;
            private set;
        }
        public String Header
        {
            get;
            private set;
        }
        public String CssClass
        {
            get;
            private set;
        }

        public DatalistColumn(String key, String header, String cssClass = "")
        {
            if (key == null)
                throw new ArgumentNullException("key");
            if (header == null)
                throw new ArgumentNullException("header");
            if (cssClass == null)
                throw new ArgumentNullException("cssClass");

            Key = key;
            Header = header;
            CssClass = cssClass;
        }
    }
}
