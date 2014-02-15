using Datalist;
using System;

namespace DatalistTests.GenericDatalistTests.Stubs
{
    public class NoRelationModel
    {
        [DatalistColumn(Relation = "None")]
        public String NoRelation { get; set; }
    }
}
