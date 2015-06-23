using System;

namespace Datalist
{
    public class DatalistAttribute : Attribute
    {
        public Type Type { get; protected set; }

        public DatalistAttribute(Type type)
        {
            if (type == null)
                throw new ArgumentNullException("type");

            if (!typeof(AbstractDatalist).IsAssignableFrom(type))
                throw new ArgumentException(
                    String.Format("'{0}' type does not implement '{1}'.", type.Name, typeof(AbstractDatalist).Name));

            Type = type;
        }
    }
}
