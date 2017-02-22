using System;
using System.ComponentModel.DataAnnotations;

namespace Datalist.Tests.Objects
{
    public class NumericModel
    {
        [Key]
        [DatalistColumn]
        public Int32 Value { get; set; }
    }
}
