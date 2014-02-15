using System;
using System.ComponentModel.DataAnnotations;

namespace DatalistTests.GenericDatalistTests.Stubs
{
    public class DatalistRelationModel
    {
        public const String DisplayValue = "Value of relation";

        [Display(Name = DisplayValue)]
        public String Value { get; set; }

        public String NoValue { get; set; }
    }
}
