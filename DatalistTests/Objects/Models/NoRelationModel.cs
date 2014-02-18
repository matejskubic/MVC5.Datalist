using Datalist;
using System;

namespace DatalistTests.Objects.Models
{
    public class NoRelationModel
    {
        [DatalistColumn(Relation = "None")]
        public String NoRelation { get; set; }
    }
}
