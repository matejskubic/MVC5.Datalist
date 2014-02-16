using Datalist;
using System;

namespace DatalistTests.Models
{
    public class NumericIdModel
    {
        [DatalistColumn]
        public Decimal Id { get; set; }
    }
}
