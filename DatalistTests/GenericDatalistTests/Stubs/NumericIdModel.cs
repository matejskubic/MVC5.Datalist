using Datalist;
using System;

namespace DatalistTests.GenericDatalistTests.Stubs
{
    public class NumericIdModel
    {
        public Decimal Id { get; set; }

        [DatalistColumn]
        public String IdString { get; set; }

        public NumericIdModel(Decimal id)
        {
            Id = id;
            IdString = id.ToString();
        }
    }
}
