using Datalist;
using System;

namespace DatalistTests.Objects.Models
{
    public class NumericIdModel
    {
        [DatalistColumn]
        public Decimal Id { get; set; }
    }
}
