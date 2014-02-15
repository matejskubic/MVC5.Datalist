using System;
using System.ComponentModel.DataAnnotations;

namespace DatalistTests.TestContext.Models
{
    public class TestRelationModel
    {
        public const String DisplayValue = "Value of relation";

        [Key]
        public String Id { get; set; }

        [Display(Name = DisplayValue)]
        public String Value { get; set; }

        public String NoValue { get; set; }
    }
}
