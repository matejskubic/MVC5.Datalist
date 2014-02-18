using Datalist;
using System;

namespace DatalistTests.Objects.Models
{
    public class NonNumericIdModel
    {
        [DatalistColumn]
        public Guid Id { get; set; }
    }
}
