using Datalist;
using System;
using System.ComponentModel.DataAnnotations;

namespace DatalistTests.TestContext.Models
{
    public class NoRelationModel
    {
        [Key]
        public String Id { get; set; }

        [DatalistColumn(Relation = "None")]
        public String NoRelation { get; set; }
    }
}
