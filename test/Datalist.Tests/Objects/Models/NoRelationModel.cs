using System;

namespace Datalist.Tests.Objects
{
    public class NoRelationModel
    {
        [DatalistColumn(Relation = "None")]
        public String NoRelation { get; set; }
    }
}
