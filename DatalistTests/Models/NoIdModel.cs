using Datalist;
using System;

namespace DatalistTests.Models
{
    public class NoIdModel
    {
        [DatalistColumn]
        public String Title { get; set; }
    }
}
