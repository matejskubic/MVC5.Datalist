using Datalist;
using System;

namespace DatalistTests.TestContext.Models
{
    public class NoRelationModel
    {
        [DatalistColumn(Relation = "None")]
        public String NoRelation { get; set; }
    }
}
