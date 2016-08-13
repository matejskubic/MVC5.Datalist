﻿using System;

namespace Datalist
{
    public class DatalistAttribute : Attribute
    {
        public Type Type { get; }

        public DatalistAttribute(Type type)
        {
            if (!typeof(AbstractDatalist).IsAssignableFrom(type))
                throw new ArgumentException($"'{type.Name}' type does not implement '{typeof(AbstractDatalist).Name}'.");

            Type = type;
        }
    }
}
