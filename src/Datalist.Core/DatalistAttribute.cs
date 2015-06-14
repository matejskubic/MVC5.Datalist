using System;

namespace Datalist
{
    public class DatalistAttribute : Attribute
    {
        public Type Type { get; private set; }

        public DatalistAttribute(Type type)
        {
            if (type == null)
                throw new ArgumentNullException("type");

            if (!typeof(AbstractDatalist).IsAssignableFrom(type))
                throw new ArgumentException(
                    String.Format(
                        "Type {0} cannot be assigned from {1} type.",
                        typeof(AbstractDatalist).Name,
                        type.Name));

            Type = type;
        }
    }
}
