using System;

namespace Datalist
{
    public class DatalistAttribute : Attribute
    {
        public Type Type { get; protected set; }

        public DatalistAttribute(Type type)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            if (!typeof(AbstractDatalist).IsAssignableFrom(type))
                throw new ArgumentException($"'{type.Name}' type does not implement '{typeof(AbstractDatalist).Name}'.");

            Type = type;
        }
    }
}
