using System;

namespace Datalist
{
    [Serializable]
    public class DatalistException : Exception
    {
        public DatalistException(String message)
            : base(message)
        {
        }
    }
}
