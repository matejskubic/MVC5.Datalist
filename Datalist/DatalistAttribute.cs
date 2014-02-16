using System;

namespace Datalist
{
    public class DatalistAttribute : Attribute
    {
        private static Type DatalistType = typeof(AbstractDatalist);

        public Type Type
        {
            get;
            private set;
        }

        public DatalistAttribute(Type type)
        {
            if (type == null)
                throw new ArgumentNullException("type");
            if (!DatalistType.IsAssignableFrom(type))
                throw new ArgumentException(String.Format("Type {0} cannot be assigned from {1} type.", DatalistType.Name, type.Name));

            Type = type;
        }
    }
}
