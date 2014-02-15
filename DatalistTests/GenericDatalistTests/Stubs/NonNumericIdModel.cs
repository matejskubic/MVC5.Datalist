using Datalist;
using System;

namespace DatalistTests.GenericDatalistTests.Stubs
{
    public class NonNumericIdModel
    {
        public Char Id { get; set; }

        [DatalistColumn]
        public String IdString { get; set; }

        public NonNumericIdModel(Int32 id)
        {
            Id = (Char)id;
            IdString = Id.ToString();
        }
    }
}
