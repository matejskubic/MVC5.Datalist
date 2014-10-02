using Datalist;
using System;

namespace Datalist.Tests.Objects.Models
{
    public class NumericIdModel
    {
        [DatalistColumn]
        public Decimal Id { get; set; }
    }
}
