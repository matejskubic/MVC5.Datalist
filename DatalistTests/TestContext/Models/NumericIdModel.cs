using Datalist;
using System;
using System.ComponentModel.DataAnnotations;

namespace DatalistTests.TestContext.Models
{
    public class NumericIdModel
    {
        [Key]
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
