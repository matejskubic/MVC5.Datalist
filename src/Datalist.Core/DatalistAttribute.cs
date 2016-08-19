using System;

namespace Datalist
{
    public class DatalistAttribute : Attribute
    {
        public Type Type { get; }

        public DatalistAttribute(Type type)
        {
            if (!typeof(MvcDatalist).IsAssignableFrom(type))
                throw new ArgumentException($"'{type.Name}' type does not implement '{typeof(MvcDatalist).Name}'.");

            Type = type;
        }
    }
}
