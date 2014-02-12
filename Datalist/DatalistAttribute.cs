using System;

namespace Datalist
{
    public class DatalistAttribute : Attribute
    {
        private Type type;
        public Type Type
        {
            get
            {
                return type;
            }
            set
            {
                if (value == null)
                    throw new ArgumentException("Type cannot be null.");
                if (!typeof(AbstractDatalist).IsAssignableFrom(value))
                    throw new ArgumentException(String.Format("Type {0} cannot be assigned from {1} type.", typeof(AbstractDatalist).Name), value.GetType().Name);
                type = value;
            }
        }
    }
}
