using System;
using System.ComponentModel.DataAnnotations;

namespace Datalist.Tests.Objects
{
    public class TestModel
    {
        [DatalistColumn(0)]
        public String Id { get; set; }

        [DatalistColumn]
        [Display(Name = "TestDisplay")]
        public Int32 Number { get; set; }

        [Datalist(typeof(TestDatalistProxy))]
        public String ParentId { get; set; }

        [DatalistColumn(-5, Format = "{0:d}")]
        public DateTime CreationDate { get; set; }

        public Decimal Sum { get; set; }
        public String NullableString { get; set; }

        public String RelationId { get; set; }
        public virtual TestRelationModel Relation { get; set; }
    }
}
