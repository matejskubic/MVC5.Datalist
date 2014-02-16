using Datalist;
using System;

namespace DatalistTests.Models
{
    public class NonNumericIdModel
    {
        [DatalistColumn]
        public Guid Id { get; set; }
    }
}
