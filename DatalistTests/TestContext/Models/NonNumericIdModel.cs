using Datalist;
using System;
using System.ComponentModel.DataAnnotations;

namespace DatalistTests.TestContext.Models
{
    public class NonNumericIdModel
    {
        [Key]
        public Guid Id { get; set; }

        [DatalistColumn]
        public String IdString { get; set; }

        public NonNumericIdModel(Int32 id)
        {
            Id = Guid.NewGuid();
            IdString = Id.ToString();
        }
    }
}
