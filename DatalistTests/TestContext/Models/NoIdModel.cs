using Datalist;
using System;
using System.ComponentModel.DataAnnotations;

namespace DatalistTests.TestContext.Models
{
    public class NoIdModel
    {
        [Key]
        public String CustomId { get; set; }

        [DatalistColumn]
        public String Title { get; set; }
    }
}
